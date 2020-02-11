using System.Collections;
using System.Collections.Generic;
using Ibit.Core.Data;
using Ibit.Core.Game;
using Ibit.Core.Serial;
using Ibit.Core.Util;
using Ibit.Core.Audio;
using UnityEngine;
using Ibit.Plataform.Camera;
using System;

namespace Ibit.LeavesGame
{
    public class Player : MonoBehaviour
    {
        #region Events

        /*Events Declaration*/
        public delegate void HaveStarDelegate(int roundScore, int roundNumber, float pikeValue);
        public event HaveStarDelegate HaveStarEvent;

        public delegate void EnablePlayDelegate();
        public event EnablePlayDelegate EnablePlayEvent;

        #endregion

        /*Player Variables*/
        public float maximumPeak;
        public float sensorValue;
        public bool flowStoped;

        /*Utility Variables*/
        public bool stop;
        public static bool waitSignal;
        public int x;
        public int _roundNumber;
        Coroutine lastCoroutine;
        public static Dictionary<float, float> playerRespiratoryInfo = new Dictionary<float, float>();


        [SerializeField]
        private Animator _animator;
        private Spawner _spawner;
        private Scorer _scorer;
        public int score = 0;

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();


            _spawner = FindObjectOfType<Spawner>();
            _scorer = FindObjectOfType<Scorer>();
            stop = false;
            waitSignal = true;
            x = 0;
            lastCoroutine = null;
            FindObjectOfType<RoundManager>().AuthorizePlayerFlowEvent += ReceivedMessage;

            if (scp.IsConnected) // Se Pitaco conectado
            {
                scp.OnSerialMessageReceived += OnMessageReceived;
                scp.StartSamplingDelayed();

            } else {
            if (scm.IsConnected) // Se Mano conectado
            {
                scm.OnSerialMessageReceived += OnMessageReceived;
                scm.StartSamplingDelayed();

            } else {
            if (scc.IsConnected) // Se Cinta conectada
            {
                scc.OnSerialMessageReceived += OnMessageReceived;
                scc.StartSamplingDelayed();

            }}}

        }

        //Sending HaveStar Event
        protected virtual void OnHaveStar(int roundScore, int roundNumber, float pikeValue)
        {
            HaveStarEvent?.Invoke(roundScore, roundNumber, pikeValue);
        }

        //Authorize RoundManager
        protected virtual void OnAuthorize() => EnablePlayEvent?.Invoke();

        //Flow values being received by the serial controller
        private void OnMessageReceived(string msg)
        {
            if (msg.Length < 1)
                return;

            sensorValue = Parsers.Float(msg);
        }

        public void ReceivedMessage(bool hasPlayed)
        {
            //Se é pra jogar waitSignal = true. Senão waitSignal = false
            waitSignal = hasPlayed;

            if (hasPlayed)
            {
                this.gameObject.GetComponent<Collider2D>().enabled = true;
                stop = false;
                lastCoroutine = StartCoroutine(Flow());
            }
            if (!hasPlayed)
            {
                this.gameObject.GetComponent<Collider2D>().enabled = false;
                stop = true;
                StopCoroutine(lastCoroutine);
            }
        }
        public void ExecuteNextStep()
        {
            OnAuthorize();
        }

        private IEnumerator Flow()
        {
            while (!stop)
            {

                var peak = 0f;
                scp = FindObjectOfType<SerialControllerPitaco>();
                scm = FindObjectOfType<SerialControllerMano>();
                scc = FindObjectOfType<SerialControllerCinta>();


                if (scp.IsConnected) // Se Pitaco conectado
                {
                    sensorValue = sensorValue < -Pacient.Loaded.PitacoThreshold || sensorValue > Pacient.Loaded.PitacoThreshold ? sensorValue : 0f;
                    peak = sensorValue > 0 ? Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * 0.5f : -Pacient.Loaded.CapacitiesPitaco.InsPeakFlow;

                } else {
                if (scm.IsConnected) // Se Mano conectado
                {
                    sensorValue = sensorValue < -Pacient.Loaded.ManoThreshold || sensorValue > Pacient.Loaded.ManoThreshold ? sensorValue : 0f;
                    peak = sensorValue > 0 ? Pacient.Loaded.CapacitiesMano.ExpPeakFlow * 0.5f : -Pacient.Loaded.CapacitiesMano.InsPeakFlow;

                } else {
                if (scc.IsConnected) // Se Cinta conectada
                {
                    sensorValue = sensorValue < -Pacient.Loaded.CintaThreshold || sensorValue > Pacient.Loaded.CintaThreshold ? sensorValue : 0f;
                    peak = sensorValue > 0 ? Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * 0.5f : -Pacient.Loaded.CapacitiesCinta.InsPeakFlow;

                }}}


                var nextPosition = sensorValue * CameraLimits.Boundary / peak;

                nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary);

                var from = this.transform.position;
                var to = new Vector3(this.transform.position.x, -nextPosition);

                this.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 15f);

                playerRespiratoryInfo.Add(Time.time, sensorValue);

                yield return null;
            }
        }

        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && waitSignal == false)
            {
                ExecuteNextStep();
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            SoundManager.Instance.PlaySound("ItemGrab");
            PoolManager.Instance.DestroyObjectPool(other.gameObject);
            _spawner.SpawnObject();
            _scorer.PutScore();
            Debug.Log(score);
        }
    }

}
