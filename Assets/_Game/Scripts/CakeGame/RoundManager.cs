using Ibit.Core.Data;
using Ibit.Core.Serial;
using Ibit.Core.Audio;
using System.Collections;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core;
using Ibit.Core.Data.Enums;
using Ibit.Core.Util;
using UnityEditor;
using Ibit.Core.Game;

namespace Ibit.CakeGame
{
    public class RoundManager : MonoBehaviour
    {
        private bool jogou = true;
        private bool paraTempo;
        private bool partidaCompleta;
        private bool ppasso;
        [SerializeField] private int passo;
        private float timer = 10;

        private float gameMultiplier = GameManager.CapacityMultiplierMinigames;

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;
        private SerialControllerOximetro sco;

        public float SpicoExpiratorio;       // adicionado 09/09/19
        public Slider slider; // adicionado 09/09/19
        public Slider sliderpico; // adicionado 16/10/19
        float picomomento = 0;      // adicionado 16/10/19

        [SerializeField] private Stars score;
        [SerializeField] public GameObject TextPanel;

        [SerializeField] private Candles candle;
        [SerializeField] private Text displayHowTo, displayTimer;
        [SerializeField] private int[] finalScore = new int[3];
        [SerializeField] public ScoreMenu finalScoreMenu;
        [SerializeField] private Player player;

        private void Awake()
        {
            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();
            sco = FindObjectOfType<SerialControllerOximetro>();
        }

        private void Start()
        {
            passo = 0;
            ppasso = false;
            partidaCompleta = false;
            displayHowTo.text = "Pressione [Enter] para começar.";
            StartCoroutine(PlayGame());
            slider.minValue = 0;    //adicionado 09/09/19

            if (scp.IsConnected) // Se Pitaco conectado
            {
                SpicoExpiratorio = Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier;

            } else {
            if (scm.IsConnected) // Se Mano conectado
            {
                SpicoExpiratorio = Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier;

            } else {
            if (scc.IsConnected) // Se Cinta conectada
            {
                SpicoExpiratorio = Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier;
            }}}

            slider.maxValue = SpicoExpiratorio;         //adicionado 09/09/19
            sliderpico.minValue = 0;    //adicionado 16/10/19
            sliderpico.maxValue = SpicoExpiratorio;         //adicionado 16/10/19
        }

        private IEnumerator PlayGame()
        {
            while (!partidaCompleta)
            {
                sco.StartSampling();

                if (ppasso)
                {
                    CleanScene();
                    
                    while (!scp.IsConnected && !scm.IsConnected && !scc.IsConnected)
                    {
                        passo = -1;
                        TextPanel.SetActive(true);
                        displayHowTo.text = "Nenhum dispositivo de controle conectado! Conecte e volte ao menu principal.";
                        yield return null;
                    }

                    // Se o PITACO estiver conectado
                    if (scp.IsConnected)
                    {
                        switch (passo)
                        {
                            case 1:
                                displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir dentro do tempo.";
                                break;

                            case 2:

                                scp.StartSampling();
                                scp.Recalibrate();

                                displayHowTo.text = "";

                                TextPanel.SetActive(false);

                                while (player.sensorValuePitaco <= Pacient.Loaded.PitacoThreshold && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValuePitaco > Pacient.Loaded.PitacoThreshold && jogou)
                                {
                                    FlowAction(player.picoExpiratorio, 1);
                                    yield return null;
                                }

                                scp.StopSampling();

                                //voltou pro 0
                                finalScoreMenu.pikeString[0] = player.picoExpiratorio.ToString();
                                TextPanel.SetActive(true);

                                if (jogou)
                                {
                                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                                    SoundManager.Instance.PlaySound("Success");
                                }

                                break;

                            case 3:
                                FindObjectOfType<MinigameLogger>().WriteMinigameRound(1, finalScore[0], player.picoExpiratorio);
                                RestauraVariaveis();
                                displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir";
                                break;

                            case 4:

                                scp.StartSampling();
                                scp.Recalibrate();

                                displayHowTo.text = "";
                                TextPanel.SetActive(false);
                                while (player.sensorValuePitaco <= Pacient.Loaded.PitacoThreshold && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValuePitaco > Pacient.Loaded.PitacoThreshold && jogou)
                                {
                                    FlowAction(player.picoExpiratorio, 2);
                                    yield return null;
                                }

                                scp.StopSampling();

                                //voltou pro 0
                                finalScoreMenu.pikeString[1] = player.picoExpiratorio.ToString();
                                TextPanel.SetActive(true);

                                if (jogou)
                                {
                                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                                    SoundManager.Instance.PlaySound("Success");
                                }

                                break;

                            case 5:
                                FindObjectOfType<MinigameLogger>().WriteMinigameRound(2, finalScore[1], player.picoExpiratorio);
                                RestauraVariaveis();
                                displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir";
                                break;

                            case 6:

                                scp.StartSampling();
                                scp.Recalibrate();

                                displayHowTo.text = "";
                                TextPanel.SetActive(false);
                                while (player.sensorValuePitaco <= Pacient.Loaded.PitacoThreshold && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValuePitaco > Pacient.Loaded.PitacoThreshold && jogou)
                                {
                                    FlowAction(player.picoExpiratorio, 3);
                                    yield return null;
                                }

                                scp.StopSampling();

                                //voltou pro 0
                                finalScoreMenu.pikeString[2] = player.picoExpiratorio.ToString();
                                TextPanel.SetActive(true);

                                if (jogou)
                                {
                                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ver a sua pontuação.";
                                    SoundManager.Instance.PlaySound("Success");
                                }

                                player.picoExpiratorio = 0;
                                break;

                            case 7:
                                FindObjectOfType<MinigameLogger>().WriteMinigameRound(3, finalScore[2], player.picoExpiratorio);

                                TextPanel.SetActive(false);
                                finalScoreMenu.DisplayFinalScore(finalScore[0], finalScore[1], finalScore[2]);
                                finalScoreMenu.ToggleScoreMenu();

                                Debug.Log("Saving minigame data...");
                                GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(true);
                                FindObjectOfType<MinigameLogger>().Save(GameDevice.Pitaco, RespiratoryExercise.ExpiratoryPeak, Minigame.CakeGame);
                                GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(false);
                                Debug.Log("Minigame logs saved.");

                                player.picoExpiratorio = 0;

                                partidaCompleta = true;
                                break;


                        }
                        ppasso = false;
                    }

                    #region Other Controllers

                    else {
                    // Se o MANO estiver conectado
                    if (scm.IsConnected)
                    {
                        switch (passo)
                        {
                            case 1:
                                displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir dentro do tempo.";
                                break;

                            case 2:

                                scm.StartSampling();
                                scm.Recalibrate();

                                displayHowTo.text = "";

                                TextPanel.SetActive(false);

                                while (player.sensorValueMano <= Pacient.Loaded.ManoThreshold && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValueMano > Pacient.Loaded.ManoThreshold && jogou)
                                {
                                    FlowAction(player.picoExpiratorio, 1);
                                    yield return null;
                                }

                                scm.StopSampling();

                                //voltou pro 0
                                finalScoreMenu.pikeString[0] = player.picoExpiratorio.ToString();
                                TextPanel.SetActive(true);

                                if (jogou)
                                {
                                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                                    SoundManager.Instance.PlaySound("Success");
                                }

                                break;

                            case 3:
                                FindObjectOfType<MinigameLogger>().WriteMinigameRound(1, finalScore[0], player.picoExpiratorio);
                                RestauraVariaveis();
                                displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir";
                                break;

                            case 4:

                                scm.StartSampling();
                                scm.Recalibrate();

                                displayHowTo.text = "";
                                TextPanel.SetActive(false);
                                while (player.sensorValueMano <= Pacient.Loaded.ManoThreshold && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValueMano > Pacient.Loaded.ManoThreshold && jogou)
                                {
                                    FlowAction(player.picoExpiratorio, 2);
                                    yield return null;
                                }

                                scm.StopSampling();

                                //voltou pro 0
                                finalScoreMenu.pikeString[1] = player.picoExpiratorio.ToString();
                                TextPanel.SetActive(true);

                                if (jogou)
                                {
                                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                                    SoundManager.Instance.PlaySound("Success");
                                }

                                break;

                            case 5:
                                FindObjectOfType<MinigameLogger>().WriteMinigameRound(2, finalScore[1], player.picoExpiratorio);
                                RestauraVariaveis();
                                displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir";
                                break;

                            case 6:

                                scm.StartSampling();
                                scm.Recalibrate();

                                displayHowTo.text = "";
                                TextPanel.SetActive(false);
                                while (player.sensorValueMano <= Pacient.Loaded.ManoThreshold && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValueMano > Pacient.Loaded.ManoThreshold && jogou)
                                {
                                    FlowAction(player.picoExpiratorio, 3);
                                    yield return null;
                                }

                                scm.StopSampling();

                                //voltou pro 0
                                finalScoreMenu.pikeString[2] = player.picoExpiratorio.ToString();
                                TextPanel.SetActive(true);

                                if (jogou)
                                {
                                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ver a sua pontuação.";
                                    SoundManager.Instance.PlaySound("Success");
                                }

                                player.picoExpiratorio = 0;
                                break;

                            case 7:
                                FindObjectOfType<MinigameLogger>().WriteMinigameRound(3, finalScore[2], player.picoExpiratorio);

                                TextPanel.SetActive(false);
                                finalScoreMenu.DisplayFinalScore(finalScore[0], finalScore[1], finalScore[2]);
                                finalScoreMenu.ToggleScoreMenu();

                                Debug.Log("Saving minigame data...");
                                GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(true);
                                FindObjectOfType<MinigameLogger>().Save(GameDevice.Mano, RespiratoryExercise.ExpiratoryPeak, Minigame.CakeGame);
                                GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(false);
                                Debug.Log("Minigame logs saved.");

                                player.picoExpiratorio = 0;

                                partidaCompleta = true;
                                break;


                        }
                        ppasso = false;
                    }
                    else {

                    // Se o CINTA estiver conectado
                    if (scc.IsConnected)
                    {
                        switch (passo)
                        {
                            case 1:
                                displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir dentro do tempo.";
                                break;

                            case 2:

                                scc.StartSampling();
                                scc.Recalibrate();

                                displayHowTo.text = "";

                                TextPanel.SetActive(false);

                                while (player.sensorValueCinta <= Pacient.Loaded.CintaThreshold && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValueCinta > Pacient.Loaded.CintaThreshold && jogou)
                                {
                                    FlowAction(player.picoExpiratorio, 1);
                                    yield return null;
                                }

                                scc.StopSampling();

                                //voltou pro 0
                                finalScoreMenu.pikeString[0] = player.picoExpiratorio.ToString();
                                TextPanel.SetActive(true);

                                if (jogou)
                                {
                                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                                    SoundManager.Instance.PlaySound("Success");
                                }

                                break;

                            case 3:
                                FindObjectOfType<MinigameLogger>().WriteMinigameRound(1, finalScore[0], player.picoExpiratorio);
                                RestauraVariaveis();
                                displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir";
                                break;

                            case 4:

                                scc.StartSampling();
                                scc.Recalibrate();

                                displayHowTo.text = "";
                                TextPanel.SetActive(false);
                                while (player.sensorValueCinta <= Pacient.Loaded.CintaThreshold && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValueCinta > Pacient.Loaded.CintaThreshold && jogou)
                                {
                                    FlowAction(player.picoExpiratorio, 2);
                                    yield return null;
                                }

                                scc.StopSampling();

                                //voltou pro 0
                                finalScoreMenu.pikeString[1] = player.picoExpiratorio.ToString();
                                TextPanel.SetActive(true);

                                if (jogou)
                                {
                                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                                    SoundManager.Instance.PlaySound("Success");
                                }

                                break;

                            case 5:
                                FindObjectOfType<MinigameLogger>().WriteMinigameRound(2, finalScore[1], player.picoExpiratorio);
                                RestauraVariaveis();
                                displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir";
                                break;

                            case 6:

                                scc.StartSampling();
                                scc.Recalibrate();

                                displayHowTo.text = "";
                                TextPanel.SetActive(false);
                                while (player.sensorValueCinta <= Pacient.Loaded.CintaThreshold && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValueCinta > Pacient.Loaded.CintaThreshold && jogou)
                                {
                                    FlowAction(player.picoExpiratorio, 3);
                                    yield return null;
                                }

                                scc.StopSampling();

                                //voltou pro 0
                                finalScoreMenu.pikeString[2] = player.picoExpiratorio.ToString();
                                TextPanel.SetActive(true);

                                if (jogou)
                                {
                                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ver a sua pontuação.";
                                    SoundManager.Instance.PlaySound("Success");
                                }

                                player.picoExpiratorio = 0;
                                break;

                            case 7:
                                FindObjectOfType<MinigameLogger>().WriteMinigameRound(3, finalScore[2], player.picoExpiratorio);

                                TextPanel.SetActive(false);
                                finalScoreMenu.DisplayFinalScore(finalScore[0], finalScore[1], finalScore[2]);
                                finalScoreMenu.ToggleScoreMenu();

                                Debug.Log("Saving minigame data...");
                                GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(true);
                                FindObjectOfType<MinigameLogger>().Save(GameDevice.Cinta, RespiratoryExercise.ExpiratoryPeak, Minigame.CakeGame);
                                GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(false);
                                Debug.Log("Minigame logs saved.");

                                player.picoExpiratorio = 0;

                                partidaCompleta = true;
                                break;


                        }
                        ppasso = false;
                    }}}

                    #endregion

                }
                yield return null;
            }
            sco.StopSampling();
        }

        private void SaveRoundData(int round, float flowValue, int roundScore)
        {
        }

        private void RestauraVariaveis()
        {
            player.picoExpiratorio = 0;
            timer = 10;
            jogou = true;
            paraTempo = false;
        }

        #region Calculating Flow Percentage

        public void FlowAction(float flowValue, int round)
        {
            var picoJogador = 0f;

            if (scp.IsConnected) // Se PITACO conectado
            {
                picoJogador = (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * gameMultiplier);
            }
            else
            {
               if (scm.IsConnected) // Se Mano conectado
               {
                   picoJogador = (Pacient.Loaded.CapacitiesMano.ExpPeakFlow * gameMultiplier);
               }
               else
               {
                   if (scc.IsConnected) // Se CINTA conectada
                   {
                       picoJogador = (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * gameMultiplier);
                   }
               }
            }

            var percentage = flowValue / picoJogador;

            if (percentage > 0.333f)  //Modificado 02/10/19 Diogo. Original: 25,50 e 75%
            {
                candle.TurnOff(0);
                candle.TurnOff(1);
                candle.TurnOff(2);
                candle.TurnOff(3);
                candle.TurnOff(4);
                candle.TurnOff(5);
                score.FillStars(0);
                finalScore[round - 1] = 1;
            }
            if (percentage > 0.667f)
            {
                candle.TurnOff(6);
                candle.TurnOff(7);
                candle.TurnOff(8);
                candle.TurnOff(9);
                candle.TurnOff(10);
                candle.TurnOff(11);
                candle.TurnOff(12);
                candle.TurnOff(13);
                candle.TurnOff(21);
                score.FillStars(1);
                finalScore[round - 1] = 2;
            }
            if (percentage >= 1.00f)
            {
                candle.TurnOff(14);
                candle.TurnOff(15);
                candle.TurnOff(16);
                candle.TurnOff(17);
                candle.TurnOff(18);
                candle.TurnOff(19);
                candle.TurnOff(20);
                score.FillStars(2);
                finalScore[round - 1] = 3;
            }

        }

        #endregion Calculatin Flow Percentage

        #region Cleaning Stats

        public void CleanScene()
        {
            candle.TurnOn();
            score.UnfillStars();
        }

        #endregion Cleaning Stats

        #region Step Controllers

        public void ExecuteNextStep()
        {
            ppasso = true;
            passo++;
        }
        #endregion Step Controllers

        #region Countdown Timer

        public void StopCountdown()
        {
            timer = 10;
            displayTimer.text = "";
        }

        #endregion Countdown Timer


        // Update is called once per frame
        private void Update()
        {


            if (scp.IsConnected) // Se PITACO conectado
            {
                slider.value = player.sensorValuePitaco;  //adicionado 09/09/19

                if (passo == 2 || passo == 4 || passo == 6)                     //adicionado 16/10/19
                {
                    if (player.sensorValuePitaco > picomomento)                     //adicionado 16/10/19
                        picomomento = player.sensorValuePitaco;                     //adicionado 16/10/19
                    sliderpico.value = picomomento;                      //adicionado 16/10/19
                }
                else                                                    //adicionado 16/10/19
                {
                    sliderpico.value = 0;                     //adicionado 16/10/19
                    picomomento = 0;
                }
            }
            else
            {
               if (scm.IsConnected) // Se Mano conectado
               {
                    slider.value = player.sensorValueMano;  //adicionado 09/09/19

                    if (passo == 2 || passo == 4 || passo == 6)                     //adicionado 16/10/19
                    {
                        if (player.sensorValueMano > picomomento)                     //adicionado 16/10/19
                            picomomento = player.sensorValueMano;                     //adicionado 16/10/19
                        sliderpico.value = picomomento;                      //adicionado 16/10/19
                    }
                    else                                                    //adicionado 16/10/19
                    {
                        sliderpico.value = 0;                     //adicionado 16/10/19
                        picomomento = 0;
                    }
               }
               else
               {
                   if (scc.IsConnected) // Se CINTA conectada
                   {
                        slider.value = player.sensorValueCinta;  //adicionado 09/09/19

                        if (passo == 2 || passo == 4 || passo == 6)                     //adicionado 16/10/19
                        {
                            if (player.sensorValueCinta > picomomento)                     //adicionado 16/10/19
                                picomomento = player.sensorValueCinta;                     //adicionado 16/10/19
                            sliderpico.value = picomomento;                      //adicionado 16/10/19
                        }
                        else                                                    //adicionado 16/10/19
                        {
                            sliderpico.value = 0;                     //adicionado 16/10/19
                            picomomento = 0;
                        }
                   }
               }
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ppasso = true;
                passo++;
            }

            if ((passo == 2 || passo == 4 || passo == 6) && paraTempo == false)
            {
                timer -= Time.deltaTime;
                displayTimer.text = "Timer: " + timer.ToString("f0");
                if (timer <= 0)
                {
                    timer = 0;
                    jogou = false;
                    player.picoExpiratorio = 0;
                    displayHowTo.text = "Ei! Você esqueceu de jogar!...\n[Enter] para continuar";
                    SoundManager.Instance.PlaySound("Failed");
                }
            }

        }
    }
}