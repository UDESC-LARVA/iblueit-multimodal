using System.Collections;
using System.Threading.Tasks;
using Ibit.Core.Audio;
using Ibit.Core.Serial;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Data;

namespace Ibit.WaterGame
{
    public class RoundManager : MonoBehaviour
    {

        /*Events Declaration*/
        public delegate void PlayerFlowDelegate(bool hasPlayed, int roundNumber);
        public event PlayerFlowDelegate AuthorizePlayerFlowEvent;

        public delegate void FinalScoreDelegate();
        public event FinalScoreDelegate ShowFinalScoreEvent;

        public delegate void CleanRoundDelegate();
        public event CleanRoundDelegate CleanRoundEvent;

        /*RoundManager Variables*/
        [SerializeField] private Text displayHowTo, displayTimer;
        [SerializeField] private GameObject TextPanel;


        [SerializeField] private Player player;         // adicionado 11/09/19

        private bool playable, finished, toBackup;
        [SerializeField] private int state, backupState, _roundNumber;
        private float countdownTimer;

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;

        public float SpicoInspiratorio;       // adicionado 11/09/19
        public float picomomento = 0;       // adicionado 10/16/19

        public Slider slider; // adicionado 11/09/19
        public Slider sliderpico; // adicionado 10/16/19
        bool resetsliderpico = true; //adicionado 21/10/19


        private Task _writeLogsTask;
        private Core.MinigameLogger _logger;

        private void Awake()
        {
            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();
            _logger = FindObjectOfType<Core.MinigameLogger>();
        }

        private void Start()
        {
            state = 1; // Player start point on State Machine
            finished = false; //To Verify if the player have finished the game
            playable = true; //To keep player at state
            toBackup = false; //Use old state value(Player haven't played->default state->continue to next state)
            countdownTimer = 10; //Time the player has to play(Flow only)
            _roundNumber = 0; //Defines in which round the the player is.
            FindObjectOfType<Player>().EnablePlayEvent += NotPlayable;
            StartCoroutine(PlayGame()); //Starts the Gameplay State Machine

            slider.minValue = 0;    //adicionado 11/04/20


            if (scp.IsConnected) // Se Pitaco conectado
            {
                SpicoInspiratorio = -Pacient.Loaded.CapacitiesPitaco.InsPeakFlow;
                Debug.Log($"SpicoInspiratorio: {SpicoInspiratorio}");

            } else {
            if (scm.IsConnected) // Se Mano conectado
            {
                SpicoInspiratorio = -Pacient.Loaded.CapacitiesMano.InsPeakFlow;
                Debug.Log($"SpicoInspiratorio: {SpicoInspiratorio}");

            } else {
            if (scc.IsConnected) // Se Cinta conectada
            {
                SpicoInspiratorio = -Pacient.Loaded.CapacitiesCinta.InsPeakFlow;

            }}}


            slider.maxValue = SpicoInspiratorio;         //adicionado 11/09/19

            sliderpico.minValue = 0;    //adicionado 10/16/19
            sliderpico.maxValue = SpicoInspiratorio;         //adicionado 10/16/19


        }

        //Sending Event Area
        protected virtual void EnablePlayerFlow(bool hasPlayed, int roundNumber)
        {
            AuthorizePlayerFlowEvent?.Invoke(hasPlayed, roundNumber);
            resetsliderpico = true;  //add 21/10/19
        }

        protected virtual void CleanRound()
        {
            CleanRoundEvent?.Invoke();
        }

        protected virtual void ShowFinalScore()
        {
            ShowFinalScoreEvent?.Invoke();
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
            state = 99; // Player haven't played -> default state
        }

        //Start the Countdown Timer
        private void StartCountdown()
        {

            countdownTimer -= Time.deltaTime;
            displayTimer.text = "Timer: " + countdownTimer.ToString("f0");
        }

        //Stop the Countdown Timer
        private void ResetCountDown()
        {
            //  resetsliderpico = true;  //add 21/10/19
            countdownTimer = 10;
            displayTimer.text = "Timer: 10";
        }

        private void PlayerWakeUp()
        {
            displayHowTo.text = "Ei! Você esqueceu de jogar!...\n Aperte [Enter] para continuar";
            //Se não jogou mandar sinal false para a permissão do jogador.
            EnablePlayerFlow(false, _roundNumber - 1);
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
                        case 1: //Introduction
                            displayHowTo.text = "Bem-Vindo ao jogo do copo d'agua![ENTER]";

                            while (playable)
                                yield return null;

                            playable = true;
                            break;
                        case 2:
                        case 4:
                        case 6: //Pre-flow
                            scp.Recalibrate();
                            scp.StartSamplingDelayed();
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Pressione [Enter] e INSPIRE \n o mais forte que conseguir dentro do tempo.";
                            while (playable)
                                yield return null;
                            CleanRound();
                            playable = true;
                            TextPanel.SetActive(false);
                            break;
                        case 3:
                        case 5:
                        case 7: //Player's Flow
                            displayHowTo.text = "";
                            EnablePlayerFlow(true, _roundNumber);
                            _roundNumber++;

                            while (playable)
                            {
                                StartCountdown();
                                yield return null;
                            }

                            ResetCountDown();
                            playable = true;
                            break;
                        case 8:
                            scp.StopSampling();
                            FindObjectOfType<Core.Util.PitacoLogger>().StopLogging();
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Pressione [Enter] para visualizar sua pontuação.";

                            if (_writeLogsTask is null)
                            {
                                _writeLogsTask = Task.Run(() =>
                                {
                                    if (_logger != null)
                                    {
                                        Debug.Log("Saving minigame data...");
                                        _logger.Save();
                                        Debug.Log("Minigame logs saved.");
                                    }
                                });
                            }
                            break;
                        case 9:
                            TextPanel.SetActive(false);
                            ShowFinalScore();
                            break;
                        case 99:
                            TextPanel.SetActive(true);
                            while (playable)
                                yield return null;
                            playable = true;
                            break;
                    }
                }
                else
                {
                    // Se o Mano estiver conectado
                    if (scm.IsConnected)
                    {
                        switch (state)
                        {
                            case 1: //Introduction
                                displayHowTo.text = "Bem-Vindo ao jogo do copo d'agua![ENTER]";

                                while (playable)
                                    yield return null;

                                playable = true;
                                break;
                            case 2:
                            case 4:
                            case 6: //Pre-flow
                                scm.Recalibrate();
                                scm.StartSamplingDelayed();
                                TextPanel.SetActive(true);
                                displayHowTo.text = "Pressione [Enter] e INSPIRE \n o mais forte que conseguir dentro do tempo.";
                                while (playable)
                                    yield return null;
                                CleanRound();
                                playable = true;
                                TextPanel.SetActive(false);
                                break;
                            case 3:
                            case 5:
                            case 7: //Player's Flow
                                displayHowTo.text = "";
                                EnablePlayerFlow(true, _roundNumber);
                                _roundNumber++;

                                while (playable)
                                {
                                    StartCountdown();
                                    yield return null;
                                }

                                ResetCountDown();
                                playable = true;
                                break;
                            case 8:
                                scm.StopSampling();
                                FindObjectOfType<Core.Util.ManoLogger>().StopLogging();
                                TextPanel.SetActive(true);
                                displayHowTo.text = "Pressione [Enter] para visualizar sua pontuação.";

                                if (_writeLogsTask is null)
                                {
                                    _writeLogsTask = Task.Run(() =>
                                    {
                                        if (_logger != null)
                                        {
                                            Debug.Log("Saving minigame data...");
                                            _logger.Save();
                                            Debug.Log("Minigame logs saved.");
                                        }
                                    });
                                }
                                break;
                            case 9:
                                TextPanel.SetActive(false);
                                ShowFinalScore();
                                break;
                            case 99:
                                TextPanel.SetActive(true);
                                while (playable)
                                    yield return null;
                                playable = true;
                                break;
                        }
                    }
                    else
                    {
                        // Se a Cinta estiver conectada
                        if (scc.IsConnected)
                        {
                            switch (state)
                            {
                                case 1: //Introduction
                                    displayHowTo.text = "Bem-Vindo ao jogo do copo d'agua![ENTER]";

                                    while (playable)
                                        yield return null;

                                    playable = true;
                                    break;
                                case 2:
                                case 4:
                                case 6: //Pre-flow
                                    scc.Recalibrate();
                                    scc.StartSamplingDelayed();
                                    TextPanel.SetActive(true);
                                    displayHowTo.text = "Pressione [Enter] e INSPIRE \n o mais forte que conseguir dentro do tempo.";
                                    while (playable)
                                        yield return null;
                                    CleanRound();
                                    playable = true;
                                    TextPanel.SetActive(false);
                                    break;
                                case 3:
                                case 5:
                                case 7: //Player's Flow
                                    displayHowTo.text = "";
                                    EnablePlayerFlow(true, _roundNumber);
                                    _roundNumber++;

                                    while (playable)
                                    {
                                        StartCountdown();
                                        yield return null;
                                    }

                                    ResetCountDown();
                                    playable = true;
                                    break;
                                case 8:
                                    scc.StopSampling();
                                    FindObjectOfType<Core.Util.CintaLogger>().StopLogging();
                                    TextPanel.SetActive(true);
                                    displayHowTo.text = "Pressione [Enter] para visualizar sua pontuação.";

                                    if (_writeLogsTask is null)
                                    {
                                        _writeLogsTask = Task.Run(() =>
                                        {
                                            if (_logger != null)
                                            {
                                                Debug.Log("Saving minigame data...");
                                                _logger.Save();
                                                Debug.Log("Minigame logs saved.");
                                            }
                                        });
                                    }
                                    break;
                                case 9:
                                    TextPanel.SetActive(false);
                                    ShowFinalScore();
                                    break;
                                case 99:
                                    TextPanel.SetActive(true);
                                    while (playable)
                                        yield return null;
                                    playable = true;
                                    break;
                            }
                        }
                    }
                } ////////////////////////////////////////




                yield return null;
            }
        }

        #endregion;

        private void Update()
        {
            slider.value = -player.sensorValue;  //adicionado 11/09/19

            if (resetsliderpico == false)                     //adicionado 16/10/19
            {

                if (player.sensorValue < picomomento)       //adicionado 16/10/19
                {
                    picomomento = player.sensorValue;                     //adicionado 16/10/19
                    sliderpico.value = -picomomento;            //adicionado 16/10/19   
                }
            }
            else                                                    //adicionado 16/10/19
            {
                sliderpico.value = 0;                     //adicionado 16/10/19
                picomomento = 0;
                resetsliderpico = false;                // add 21/10/19
            }




            if (countdownTimer <= 0)
            {
                SoundManager.Instance.PlaySound("Failed");
                PlayerWakeUp();
            }
        }
    }
}