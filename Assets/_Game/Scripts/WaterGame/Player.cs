using System.Collections;
using Ibit.Core.Data;
using Ibit.Core.Serial;
using Ibit.Core.Util;
using Ibit.Core.Audio;
using UnityEngine;
using Ibit.Core;
using Ibit.Core.Game;

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
        public float sensorValuePitaco;
        public float sensorValueMano;
        public float sensorValueCinta;
        public float sensorValueOximetro;
        public bool flowStoped;

        /*Utility Variables*/
        public bool stop;
        public bool waitSignal;
        public int x;
        public int _roundNumber;
        Coroutine lastCoroutine;

        private float gameMultiplier = GameManager.CapacityMultiplierMinigames;

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;
        private SerialControllerOximetro sco;

        public float HRValue;
        public float SPO2Value;
        public bool oxiActive = false;


        private void Start()
        {
            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();
            sco = FindObjectOfType<SerialControllerOximetro>();

            stop = false;
            waitSignal = false;
            x = 0;
            lastCoroutine = null;
            FindObjectOfType<RoundManager>().AuthorizePlayerFlowEvent += ReceivedMessage;


            if (scp.IsConnected) // Se PITACO conectado
            {
                scp.OnSerialMessageReceived += OnMessageReceivedPitaco;
                scp.StartSamplingDelayed();

            }
            else
            {
                if (scm.IsConnected) // Se MANO conectado
                {
                    scm.OnSerialMessageReceived += OnMessageReceivedMano;
                    scm.StartSamplingDelayed();

                }
                else
                {
                    if (scc.IsConnected) // Se CINTA conectada
                    {
                        scc.OnSerialMessageReceived += OnMessageReceivedCinta;
                        scc.StartSamplingDelayed();

                    }
                }
            }

            if (sco.IsConnected) // Se OXÍMETRO conectado
            {
                sco.OnSerialMessageReceived += OnMessageReceivedOximetro;

            }

        }

        //Sending HaveStar Event
        protected virtual void OnHaveStar(int roundScore, int roundNumber, float pikeValue)
        {
            HaveStarEvent?.Invoke(roundScore, roundNumber, pikeValue);
        }

        //Authorize RoundManager
        protected virtual void OnAuthorize() => EnablePlayEvent?.Invoke();

        //Flow values being received by the serial controller
        private void OnMessageReceivedPitaco(string msg)
        {
            if (msg.Length < 1)
                return;

            sensorValuePitaco = Parsers.Float(msg);
        }

        private void OnMessageReceivedMano(string msg)
        {
            if (msg.Length < 1)
                return;

            sensorValueMano = Parsers.Float(msg);
        }

        private void OnMessageReceivedCinta(string msg)
        {
            if (msg.Length < 1)
                return;

            sensorValueCinta = Parsers.Float(msg);
        }

        private void OnMessageReceivedOximetro(string msg)
        {
            if (msg.Length < 1)
                return;

            oxiActive = true;
            string[] sensorValueOxi = msg.Split(',');
            HRValue = Parsers.Float(sensorValueOxi[0]);
            SPO2Value = Parsers.Float(sensorValueOxi[1]);
        }



        public void ReceivedMessage(bool hasPlayed, int roundNumber)
        {
            //Se é pra jogar waitSignal = true. Senão waitSignal = false
            waitSignal = hasPlayed;
            _roundNumber = roundNumber;

            if (hasPlayed)
                lastCoroutine = StartCoroutine(Flow(roundNumber));

            if (!hasPlayed)
            {
                StopCoroutine(lastCoroutine);
                OnHaveStar(0, roundNumber, 0);
            }
        }

        public void ExecuteNextStep()
        {
            OnAuthorize();
        }
        private IEnumerator Flow(int round)
        {

            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();

            if (scp.IsConnected) // Se PITACO conectado
            {
                //While player does not blow.
                while (sensorValuePitaco >= -Pacient.Loaded.PitacoThreshold * 2f)
                {
                    //Debug.Log($"Wait: {sensorValue}");
                    yield return null;
                }

                //Player is blowing, take the highest value.
                while (sensorValuePitaco < -Pacient.Loaded.PitacoThreshold)
                {
                    //Debug.Log($"Blow: {sensorValue}");

                    if (sensorValuePitaco < maximumPeak)
                    {
                        maximumPeak = sensorValuePitaco;
                        //Debug.Log("Novo pico máximo: " + maximumPeak);
                    }

                    //calculate the percentage of the pike.
                    yield return null;
                }

            }
            else
            {
                if (scm.IsConnected) // Se Mano conectado
                {
                    //While player does not blow.
                    while (sensorValueMano >= -Pacient.Loaded.ManoThreshold * 2f)
                    {
                        //Debug.Log($"Wait: {sensorValue}");
                        yield return null;
                    }

                    //Player is blowing, take the highest value.
                    while (sensorValueMano < -Pacient.Loaded.ManoThreshold)
                    {
                        //Debug.Log($"Blow: {sensorValue}");

                        if (sensorValueMano < maximumPeak)
                        {
                            maximumPeak = sensorValueMano;
                            //Debug.Log("Novo pico máximo: " + maximumPeak);
                        }

                        //calculate the percentage of the pike.
                        yield return null;
                    }

                }
                else
                {
                    if (scc.IsConnected) // Se CINTA conectada
                    {
                        //While player does not blow.
                        while (sensorValueCinta >= -Pacient.Loaded.CintaThreshold * 2f)
                        {
                            //Debug.Log($"Wait: {sensorValue}");
                            yield return null;
                        }

                        //Player is blowing, take the highest value.
                        while (sensorValueCinta < -Pacient.Loaded.CintaThreshold)
                        {
                            //Debug.Log($"Blow: {sensorValue}");

                            if (sensorValueCinta < maximumPeak)
                            {
                                maximumPeak = sensorValueCinta;
                                //Debug.Log("Novo pico máximo: " + maximumPeak);
                            }

                            //calculate the percentage of the pike.
                            yield return null;
                        }

                    }
                }
            }

            SoundManager.Instance.PlaySound("Success");

            var roundScore = CalculateFlowPike(maximumPeak);
            WriteMinigameLog(round, roundScore, PitacoFlowMath.ToLitresPerMinute(maximumPeak));
            waitSignal = false;
            maximumPeak = 0f;
            OnAuthorize();
        }

        private void WriteMinigameLog(int round, int roundScore, float flowScore)
        {
            FindObjectOfType<MinigameLogger>().WriteMinigameRound(round, roundScore, flowScore);
        }

        private int CalculateFlowPike(float pikeValue)
        {
            var playerPike = 0f;
            var roundScore = 0;

            if (scp.IsConnected) // Se PITACO conectado
            {
                playerPike = (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);  //originalmente RawInspeakFlow; alterado->02/10/19

            }
            else
            {
                if (scm.IsConnected) // Se Mano conectado
                {
                    playerPike = (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);  //originalmente RawInspeakFlow; alterado->02/10/19

                }
                else
                {
                    if (scc.IsConnected) // Se CINTA conectada
                    {
                        playerPike = (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);  //originalmente RawInspeakFlow; alterado->02/10/19

                    }
                }
            }




            var percentage = -pikeValue / playerPike;

            if (percentage > 1.00f)   //Originalmente seria 25,50 e 75%, Foi modificado em 02/10/19 por Diogo e Jhonatan para 33, 67 e 100%
            {
                roundScore = 3;
                OnHaveStar(3, _roundNumber, pikeValue);
            }
            else if (percentage > 0.667f)
            {
                roundScore = 2;
                OnHaveStar(2, _roundNumber, pikeValue);
            }
            else if (percentage > 0.333f)
            {
                roundScore = 1;
                OnHaveStar(1, _roundNumber, pikeValue);
            }


            return roundScore;
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
