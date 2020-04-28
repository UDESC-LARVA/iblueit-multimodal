using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Ibit.Core.Audio;
using Ibit.Core.Serial;
using Ibit.Core.Util;

namespace Ibit.LeavesGame
{
    public class RoundManager : MonoBehaviour
    {
        #region Events

        /*Events Declaration*/
        public delegate void PlayerFlowDelegate(bool hasPlayed);
        public event PlayerFlowDelegate AuthorizePlayerFlowEvent;

        public delegate void FinalScoreDelegate();
        public event FinalScoreDelegate ShowFinalScoreEvent;

        public delegate void CleanRoundDelegate();
        public event CleanRoundDelegate CleanRoundEvent;

        #endregion

        /*RoundManager Variables*/
        [SerializeField] private Text displayHowTo, displayTimer;
        [SerializeField] private GameObject TextPanel;
        [SerializeField] private GameObject GameTimePanel;
        [SerializeField] private InputField timeField;

        private bool playable, finished, toBackup;
        [SerializeField] private int state, backupState, _roundNumber;
        [SerializeField] private float countdownTime, timer;

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;

        private bool timeOver, hasTime;
        private Scorer _scorer;

        private void Awake()
        {
            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();
        }

        private void Start()
        {
            state = 0; // Player start point on State Machine
            finished = false; //To Verify if the player have finished the game
            playable = true; //To keep player at state
            timeOver = false;
            toBackup = false; //Use old state value(Player haven't played->default state->continue to next state)
            hasTime = false;
            timer = 1; //Time the player has to play(Flow only)
            countdownTime = 60;
            _roundNumber = 0; //Defines in which round the the player is.
            _scorer = FindObjectOfType<Scorer>();
            FindObjectOfType<Player>().EnablePlayEvent += NotPlayable;
            StartCoroutine(PlayGame());//Starts the Gameplay State Machine
            TextPanel.SetActive(false);
            SoundManager.Instance.PlaySound("BGM");
        }

        #region Sending Event Area

        protected virtual void EnablePlayerFlow(bool hasPlayed)
        {
            AuthorizePlayerFlowEvent?.Invoke(hasPlayed);
        }

        protected virtual void CleanRound()
        {
            CleanRoundEvent?.Invoke();
        }

        protected virtual void ShowFinalScore()
        {
            ShowFinalScoreEvent?.Invoke();
        }

        #endregion

        public void OnSubmitTime()
        {
            string inputTime = timeField.text;
            if (inputTime != "")
            {
                countdownTime = float.Parse(inputTime);
                if (countdownTime > 300)
                    countdownTime = 300;
            }
            GameTimePanel.SetActive(false);
            Player.waitSignal = false;
            NotPlayable();
        }

        private void NotPlayable()
        {
            playable = false;
            if (toBackup)
            {
                state = backupState;
                toBackup = false;
            }
            state++;
        }

        private void NotPlayedState()
        {
            playable = false;
            backupState = state;
            toBackup = true;
            state = 99;// Player haven't played -> default state
        }

        //Start the Countdown Timer
        private void StartCountdown()
        {
            timer -= Time.deltaTime;
            displayTimer.text = "Timer: " + timer.ToString("f0");
        }

        //Reset Countdown Timer
        private void ResetCountDown()
        {
            timer = countdownTime;
            displayTimer.text = "Timer: " + timer.ToString();
        }

        private void PlayerWakeUp()
        {
            Debug.Log(PitacoFlowMath.RespiratoryRate(Player.playerRespiratoryInfo,(int)countdownTime) + " bps");
            displayHowTo.text = "Acabou o tempo![Enter] para continuar.";
            //Se não jogou mandar sinal false para a permissão do jogador.
            EnablePlayerFlow(false);
            ResetCountDown();
            NotPlayedState();
        }

        #region Gameplay State Machine

        //Incremental states(put them into the correct order)
        private IEnumerator PlayGame()
        {
            while (!finished)
            {
                
                
                while (!scp.IsConnected && !scm.IsConnected && !scc.IsConnected)
                {
                    state = -1;
                    TextPanel.SetActive(true);
                    displayHowTo.text = "Nenhum dispositivo de controle conectado! Conecte e volte ao menu principal.";
                    yield return null;
                }
                
                // Se o Pitaco estiver conectado
                if (scp.IsConnected)
                {
                    switch (state)
                    {

                        case 1://Introduction
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Bem-Vindo ao jogo Coletando as Folhas! Pressione [ENTER] para continuar.";
                            while (playable)
                                yield return null;

                            playable = true;
                            break;


                        case 2:
                        case 4:
                        case 6://Pre-flow
                            scp.Recalibrate();
                            scp.StartSamplingDelayed();
                            ResetCountDown();
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Pressione [Enter] e RESPIRE normalmente dentro do tempo para coletar as folhas.";
                            while (playable)
                                yield return null;
                            CleanRound();
                            playable = true;
                            TextPanel.SetActive(false);
                            break;
                        case 3:
                        case 5:
                        case 7://Player's Flow
                            displayHowTo.text = "";

                            EnablePlayerFlow(true);
                            while (!timeOver && playable)
                            {
                                StartCountdown();
                                yield return null;
                            }
                            Debug.Log("Terminei a jogada!");

                            _scorer.PutRoundScore(_roundNumber);
                            _roundNumber++;
                            ResetCountDown();
                            timeOver = false;
                            playable = true;
                            break;
                        case 8:
                            scp.StopSampling();
                            FindObjectOfType<Core.Util.PitacoLogger>().StopLogging();
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Pressione [Enter] para visualizar sua pontuação.";
                            break;
                        case 9:
                            TextPanel.SetActive(false);
                            SoundManager.Instance.PlaySound("Finished");
                            ShowFinalScore();
                            break;
                        case 99:
                            TextPanel.SetActive(true);
                            while (playable)
                                yield return null;
                            playable = true;
                            break;
                    }
                } else { ////////////////////
                // Se o Mano estiver conectado
                if (scm.IsConnected)
                {
                    switch (state)
                    {

                        case 1://Introduction
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Bem-Vindo ao jogo Coletando as Folhas! Pressione [ENTER] para continuar.";
                            while (playable)
                                yield return null;

                            playable = true;
                            break;


                        case 2:
                        case 4:
                        case 6://Pre-flow
                            scm.Recalibrate();
                            scm.StartSamplingDelayed();
                            ResetCountDown();
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Pressione [Enter] e RESPIRE normalmente dentro do tempo para coletar as folhas.";
                            while (playable)
                                yield return null;
                            CleanRound();
                            playable = true;
                            TextPanel.SetActive(false);
                            break;
                        case 3:
                        case 5:
                        case 7://Player's Flow
                            displayHowTo.text = "";

                            EnablePlayerFlow(true);
                            while (!timeOver && playable)
                            {
                                StartCountdown();
                                yield return null;
                            }
                            Debug.Log("Terminei a jogada!");

                            _scorer.PutRoundScore(_roundNumber);
                            _roundNumber++;
                            ResetCountDown();
                            timeOver = false;
                            playable = true;
                            break;
                        case 8:
                            scm.StopSampling();
                            FindObjectOfType<Core.Util.ManoLogger>().StopLogging();
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Pressione [Enter] para visualizar sua pontuação.";
                            break;
                        case 9:
                            TextPanel.SetActive(false);
                            SoundManager.Instance.PlaySound("Finished");
                            ShowFinalScore();
                            break;
                        case 99:
                            TextPanel.SetActive(true);
                            while (playable)
                                yield return null;
                            playable = true;
                            break;
                    }
                } else { ////////////////////
                // Se a Cinta Extensora estiver conectada
                if (scc.IsConnected)
                {
                    switch (state)
                    {

                        case 1://Introduction
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Bem-Vindo ao jogo Coletando as Folhas! Pressione [ENTER] para continuar.";
                            while (playable)
                                yield return null;

                            playable = true;
                            break;


                        case 2:
                        case 4:
                        case 6://Pre-flow
                            scc.Recalibrate();
                            scc.StartSamplingDelayed();
                            ResetCountDown();
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Pressione [Enter] e RESPIRE normalmente dentro do tempo para coletar as folhas.";
                            while (playable)
                                yield return null;
                            CleanRound();
                            playable = true;
                            TextPanel.SetActive(false);
                            break;
                        case 3:
                        case 5:
                        case 7://Player's Flow
                            displayHowTo.text = "";

                            EnablePlayerFlow(true);
                            while (!timeOver && playable)
                            {
                                StartCountdown();
                                yield return null;
                            }
                            Debug.Log("Terminei a jogada!");

                            _scorer.PutRoundScore(_roundNumber);
                            _roundNumber++;
                            ResetCountDown();
                            timeOver = false;
                            playable = true;
                            break;
                        case 8:
                            scc.StopSampling();
                            FindObjectOfType<Core.Util.CintaLogger>().StopLogging();
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Pressione [Enter] para visualizar sua pontuação.";
                            break;
                        case 9:
                            TextPanel.SetActive(false);
                            SoundManager.Instance.PlaySound("Finished");
                            ShowFinalScore();
                            break;
                        case 99:
                            TextPanel.SetActive(true);
                            while (playable)
                                yield return null;
                            playable = true;
                            break;
                    }
                }}}

                
                yield return null;
            }
        }

        #endregion;

        private void Update()
        {
            if (timer <= 0)
            {
                SoundManager.Instance.PlaySound("TimeUp");
                timeOver = true;
                PlayerWakeUp();
            }
        }
    }
}