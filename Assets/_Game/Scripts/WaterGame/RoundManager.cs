using System.Collections;
using Ibit.Core.Audio;
using Ibit.Core.Serial;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Data;
using Ibit.Core.Data.Enums;
using Ibit.Core.Util;
using Ibit.Core.Game;

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

        private float gameMultiplier = GameManager.CapacityMultiplierMinigames;

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;
        private SerialControllerOximetro sco;

        public float SpicoInspiratorio;       // adicionado 11/09/19
        public float picomomento = 0;       // adicionado 10/16/19

        public Slider slider; // adicionado 11/09/19
        public Slider sliderpico; // adicionado 10/16/19
        bool resetsliderpico = true; //adicionado 21/10/19

        private void Awake()
        {
            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();
            sco = FindObjectOfType<SerialControllerOximetro>();
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
            slider.minValue = 0;    //adicionado 11/09/19


            if (scp.IsConnected) // Se PITACO conectado
            {
                SpicoInspiratorio = (-Pacient.Loaded.CapacitiesPitaco.InsPeakFlow * gameMultiplier);
            }
            else
            {
                if (scm.IsConnected) // Se Mano conectado
                {
                    SpicoInspiratorio = (-Pacient.Loaded.CapacitiesMano.InsPeakFlow * gameMultiplier);
                }
                else
                {
                    if (scc.IsConnected) // Se CINTA conectada
                    {
                        SpicoInspiratorio = (-Pacient.Loaded.CapacitiesCinta.InsPeakFlow * gameMultiplier);
                    }
                }
            }

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
                sco.StartSampling();

                while (!scp.IsConnected && !scm.IsConnected && !scc.IsConnected)
                {
                    state = -1;
                    TextPanel.SetActive(true);
                    displayHowTo.text = "Nenhum dispositivo de controle conectado! Conecte e volte ao menu principal.";
                    yield return null;
                }

                // Se o PITACO estiver conectado
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
                            scp.StartSampling();
                            scp.Recalibrate();
                            displayHowTo.text = "";
                            EnablePlayerFlow(true, _roundNumber);
                            _roundNumber++;

                            while (playable)
                            {
                                StartCountdown();
                                yield return null;
                            }
                            scp.StopSampling();
                            ResetCountDown();
                            playable = true;
                            break;
                        case 8:
                            TextPanel.SetActive(true);
                            displayHowTo.text = "Pressione [Enter] para visualizar sua pontuação.";
                            break;
                        case 9:
                            Debug.Log("Saving minigame data...");
                            GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(true);
                            FindObjectOfType<Core.MinigameLogger>().Save(GameDevice.Pitaco, RespiratoryExercise.InspiratoryPeak, Minigame.WaterGame);
                            GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(false);
                            Debug.Log("Minigame logs saved.");
                            
                            NotPlayable();
                            break;
                        case 10:
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
                                scm.StartSampling();
                                scm.Recalibrate();
                                displayHowTo.text = "";
                                EnablePlayerFlow(true, _roundNumber);
                                _roundNumber++;

                                while (playable)
                                {
                                    StartCountdown();
                                    yield return null;
                                }
                                scm.StopSampling();
                                ResetCountDown();
                                playable = true;
                                break;
                            case 8:
                                TextPanel.SetActive(true);
                                displayHowTo.text = "Pressione [Enter] para visualizar sua pontuação.";
                                break;
                            case 9:
                                Debug.Log("Saving minigame data...");
                                GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(true);
                                FindObjectOfType<Core.MinigameLogger>().Save(GameDevice.Mano, RespiratoryExercise.InspiratoryPeak, Minigame.WaterGame);
                                GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(false);
                                Debug.Log("Minigame logs saved.");
                            
                                NotPlayable();
                                break;
                            case 10:
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
                        // Se a CINTA estiver conectada
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
                                    scc.StartSampling();
                                    scc.Recalibrate();
                                    displayHowTo.text = "";
                                    EnablePlayerFlow(true, _roundNumber);
                                    _roundNumber++;

                                    while (playable)
                                    {
                                        StartCountdown();
                                        yield return null;
                                    }
                                    scc.StopSampling();
                                    ResetCountDown();
                                    playable = true;
                                    break;
                                case 8:
                                    TextPanel.SetActive(true);
                                    displayHowTo.text = "Pressione [Enter] para visualizar sua pontuação.";
                                    break;
                                case 9:
                                    Debug.Log("Saving minigame data...");
                                    GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(true);
                                    FindObjectOfType<Core.MinigameLogger>().Save(GameDevice.Cinta, RespiratoryExercise.InspiratoryPeak, Minigame.WaterGame);
                                    GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(false);
                                    Debug.Log("Minigame logs saved.");
                            
                                    NotPlayable();
                                    break;
                                case 10:
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
                }

                yield return null;
            }
            sco.StopSampling();
        }

        #endregion;

        private void Update()
        {
            if (scp.IsConnected) // Se PITACO conectado
            {
                slider.value = -player.sensorValuePitaco;  //adicionado 11/09/19

                if (resetsliderpico == false)                     //adicionado 16/10/19
                {

                    if (player.sensorValuePitaco < picomomento)       //adicionado 16/10/19
                    {
                        picomomento = player.sensorValuePitaco;                     //adicionado 16/10/19
                        sliderpico.value = -picomomento;            //adicionado 16/10/19   
                    }
                }
                else                                                    //adicionado 16/10/19
                {
                    sliderpico.value = 0;                     //adicionado 16/10/19
                    picomomento = 0;
                    resetsliderpico = false;                // add 21/10/19
                }

            }
            else
            {
                if (scm.IsConnected) // Se Mano conectado
                {
                    slider.value = -player.sensorValueMano;  //adicionado 11/09/19

                    if (resetsliderpico == false)                     //adicionado 16/10/19
                    {

                        if (player.sensorValueMano < picomomento)       //adicionado 16/10/19
                        {
                            picomomento = player.sensorValueMano;                     //adicionado 16/10/19
                            sliderpico.value = -picomomento;            //adicionado 16/10/19   
                        }
                    }
                    else                                                    //adicionado 16/10/19
                    {
                        sliderpico.value = 0;                     //adicionado 16/10/19
                        picomomento = 0;
                        resetsliderpico = false;                // add 21/10/19
                    }
                }
               else
                {
                    if (scc.IsConnected) // Se CINTA conectada
                    {
                        slider.value = -player.sensorValueCinta;  //adicionado 11/09/19

                        if (resetsliderpico == false)                     //adicionado 16/10/19
                        {

                            if (player.sensorValueCinta < picomomento)       //adicionado 16/10/19
                            {
                                picomomento = player.sensorValueCinta;                     //adicionado 16/10/19
                                sliderpico.value = -picomomento;            //adicionado 16/10/19   
                            }
                        }
                        else                                                    //adicionado 16/10/19
                        {
                            sliderpico.value = 0;                     //adicionado 16/10/19
                            picomomento = 0;
                            resetsliderpico = false;                // add 21/10/19
                        }
                    }
                }
            }

            if (countdownTimer <= 0)
            {
                SoundManager.Instance.PlaySound("Failed");
                PlayerWakeUp();
            }
        }
    }
}