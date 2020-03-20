using System.Collections;
using Ibit.Core.Data;
using Ibit.Core.Serial;
using Ibit.Core.Util;
using Ibit.Core.Audio;
using UnityEngine;
using Ibit.Core;

namespace Ibit.WaterGame
{
    public class Player : MonoBehaviour
    {
        /*Events Declaration*/
        public delegate void HaveStarDelegate(int roundScore, int roundNumber, float pikeValue);
        public event HaveStarDelegate HaveStarEvent;

        public delegate void EnablePlayDelegate();
        public event EnablePlayDelegate EnablePlayEvent;

        /*Player Variables*/
        public float maximumPeak;
        public float sensorValue;
        public bool flowStoped;

        /*Utility Variables*/
        public bool stop;
        public bool waitSignal;
        public int x;
        public int _roundNumber;
        Coroutine lastCoroutine;

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;


        private void Start()
        {
            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();
            


            stop = false;
            waitSignal = false;
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

        public void ReceivedMessage(bool hasPlayed, int roundNumber)
        {
            //Se é pra jogar waitSignal = true. Senão waitSignal = false
            waitSignal = hasPlayed;
            _roundNumber = roundNumber;

            if (hasPlayed)
                lastCoroutine = StartCoroutine(Flow());

            if (!hasPlayed)
            {
                StopCoroutine(lastCoroutine);
                OnHaveStar(0, roundNumber, 0);
            }
        }
        public void ExecuteNextStep()
        {
            //Debug.Log("tetris");
            OnAuthorize();
        }
        private IEnumerator Flow()
        {
            
            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();
            



            if (scp.IsConnected) // Se Pitaco conectado
            {
                //While player does not blow.
                while (sensorValue >= -Pacient.Loaded.PitacoThreshold * 2f)
                {
                //Debug.Log($"Wait: {sensorValue}");
                    yield return null;
                }

                //Player is blowing, take the highest value.
                while (sensorValue < -Pacient.Loaded.PitacoThreshold)
                {
                    //Debug.Log($"Blow: {sensorValue}");

                    if (sensorValue < maximumPeak)
                    {
                        maximumPeak = sensorValue;
                        //Debug.Log("Novo pico máximo: " + maximumPeak);
                    }

                    //calculate the percentage of the pike.
                    yield return null;
                }

            } else {
            if (scm.IsConnected) // Se Mano conectado
            {
                //While player does not blow.
                while (sensorValue >= -Pacient.Loaded.ManoThreshold * 2f)
                {
                //Debug.Log($"Wait: {sensorValue}");
                    yield return null;
                }

                //Player is blowing, take the highest value.
                while (sensorValue < -Pacient.Loaded.ManoThreshold)
                {
                    //Debug.Log($"Blow: {sensorValue}");

                    if (sensorValue < maximumPeak)
                    {
                        maximumPeak = sensorValue;
                        //Debug.Log("Novo pico máximo: " + maximumPeak);
                    }

                    //calculate the percentage of the pike.
                    yield return null;
                }

            } else {
            if (scc.IsConnected) // Se Cinta conectada
            {
                //While player does not blow.
                while (sensorValue >= -Pacient.Loaded.CintaThreshold * 2f)
                {
                //Debug.Log($"Wait: {sensorValue}");
                    yield return null;
                }

                //Player is blowing, take the highest value.
                while (sensorValue < -Pacient.Loaded.CintaThreshold)
                {
                    //Debug.Log($"Blow: {sensorValue}");

                    if (sensorValue < maximumPeak)
                    {
                        maximumPeak = sensorValue;
                        //Debug.Log("Novo pico máximo: " + maximumPeak);
                    }

                    //calculate the percentage of the pike.
                    yield return null;
                }

            }}}


            SoundManager.Instance.PlaySound("Success");

            FindObjectOfType<MinigameLogger>().Write(PitacoFlowMath.ToLitresPerMinute(sensorValue));

            CalculateFlowPike(maximumPeak);
            waitSignal = false;
            OnAuthorize();
        }

        private void CalculateFlowPike(float pikeValue)
        {
            var playerPike = 0f;

            if (scp.IsConnected) // Se Pitaco conectado
            {
                playerPike = -Pacient.Loaded.CapacitiesPitaco.InsPeakFlow;  //originalmente RawInspeakFlow; alterado->02/10/19

            } else {
            if (scm.IsConnected) // Se Mano conectado
            {
                playerPike = -Pacient.Loaded.CapacitiesMano.InsPeakFlow;  //originalmente RawInspeakFlow; alterado->02/10/19

            } else {
            if (scc.IsConnected) // Se Cinta conectada
            {
                playerPike = -Pacient.Loaded.CapacitiesCinta.InsPeakFlow;  //originalmente RawInspeakFlow; alterado->02/10/19

            }}}


            
            
            var percentage = -pikeValue / playerPike;
            
            Debug.Log("Porcentagem: " + percentage);

            if (percentage > 1.00f)   //Originalmente seria 25,50 e 75%, Foi modificado em 02/10/19 por Diogo e Jhonatan para 33, 67 e 100%
            {
                OnHaveStar(3, _roundNumber, pikeValue);
            }
            else if (percentage > 0.667f)
            {
                OnHaveStar(2, _roundNumber, pikeValue);
            }
            else if (percentage > 0.333f)
            {
                OnHaveStar(1, _roundNumber, pikeValue);
            }

            maximumPeak = 0f;
        }

        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && waitSignal == false)
            {
                ExecuteNextStep();
            }
        }
    }
}
