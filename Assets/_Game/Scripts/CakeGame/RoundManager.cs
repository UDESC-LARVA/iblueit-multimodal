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

        private SerialControllerPitaco scp;
        //private SerialControllerMano scm;
        //private SerialControllerCinta scc;


        public float SpicoExpiratorio;       // adicionado 09/09/19


        public Slider slider; // adicionado 09/09/19

        public Slider sliderpico; // adicionado 16/10/19
        float picomomento = 0;      // adicionado 16/10/19



        [SerializeField] private Stars score;
        [SerializeField] public GameObject TextPanel;


        [SerializeField] private Candles candle;
        [SerializeField] private Text displayHowTo, displayTimer;
        [SerializeField] private int[] finalScore = new int[3];
        [SerializeField] private ScoreMenu finalScoreMenu;
        [SerializeField] private Player player;

        private void Awake()
        {
            scp = FindObjectOfType<SerialControllerPitaco>();
            //scm = FindObjectOfType<SerialControllerMano>();
            //scc = FindObjectOfType<SerialControllerCinta>();
        }

        private IEnumerator PlayGame()
        {
            while (!partidaCompleta)
            {
                if (ppasso)
                {
                    CleanScene();
                    //Para voltar os outros controladores, voltar a condição AND com scm e scc
                    while (!scp.IsConnected)
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

                                while (player.sensorValue <= Pacient.Loaded.PitacoThreshold * 2 && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValue > Pacient.Loaded.PitacoThreshold && jogou)
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
                                while (player.sensorValue <= Pacient.Loaded.PitacoThreshold * 2 && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValue > Pacient.Loaded.PitacoThreshold && jogou)
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

                                player.picoExpiratorio = 0;
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
                                while (player.sensorValue <= Pacient.Loaded.PitacoThreshold * 2 && jogou)
                                    yield return null;

                                StopCountdown();
                                paraTempo = true;

                                //saiu do 0
                                while (player.sensorValue > Pacient.Loaded.PitacoThreshold && jogou)
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

                                player.picoExpiratorio = 0;

                                partidaCompleta = true;
                                break;


                        }
                        ppasso = false;
                    }

                    #region Other Controllers

                    //else
                    //{ ////////////////////
                    //  // Se o Mano estiver conectado
                    //    if (scm.IsConnected)
                    //    {
                    //        switch (passo)
                    //        {
                    //            case 1:
                    //                displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir dentro do tempo.";
                    //                break;

                    //            case 2:

                    //                scm.StartSampling();
                    //                scm.Recalibrate();

                    //                displayHowTo.text = "";

                    //                TextPanel.SetActive(false);

                    //                while (player.sensorValue <= Pacient.Loaded.ManoThreshold * 2 && jogou)
                    //                    yield return null;

                    //                StopCountdown();
                    //                paraTempo = true;

                    //                //saiu do 0
                    //                while (player.sensorValue > Pacient.Loaded.ManoThreshold && jogou)
                    //                {
                    //                    FlowAction(player.picoExpiratorio, 1);
                    //                    yield return null;
                    //                }

                    //                //voltou pro 0
                    //                finalScoreMenu.pikeString[0] = player.picoExpiratorio.ToString();
                    //                TextPanel.SetActive(true);

                    //                if (jogou)
                    //                {
                    //                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                    //                    SoundManager.Instance.PlaySound("Success");
                    //                }

                    //                player.picoExpiratorio = 0;
                    //                break;

                    //            case 3:
                    //                RestauraVariaveis();
                    //                break;

                    //            case 4:
                    //                displayHowTo.text = "";
                    //                TextPanel.SetActive(false);
                    //                while (player.sensorValue <= Pacient.Loaded.ManoThreshold * 2 && jogou)
                    //                    yield return null;

                    //                StopCountdown();
                    //                paraTempo = true;

                    //                //saiu do 0
                    //                while (player.sensorValue > Pacient.Loaded.ManoThreshold && jogou)
                    //                {
                    //                    FlowAction(player.picoExpiratorio, 2);
                    //                    yield return null;
                    //                }

                    //                //voltou pro 0
                    //                finalScoreMenu.pikeString[1] = player.picoExpiratorio.ToString();
                    //                TextPanel.SetActive(true);

                    //                if (jogou)
                    //                {
                    //                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                    //                    SoundManager.Instance.PlaySound("Success");
                    //                }

                    //                player.picoExpiratorio = 0;
                    //                break;

                    //            case 5:
                    //                RestauraVariaveis();
                    //                break;

                    //            case 6:
                    //                displayHowTo.text = "";
                    //                TextPanel.SetActive(false);
                    //                while (player.sensorValue <= Pacient.Loaded.ManoThreshold * 2 && jogou)
                    //                    yield return null;

                    //                StopCountdown();
                    //                paraTempo = true;

                    //                //saiu do 0
                    //                while (player.sensorValue > Pacient.Loaded.ManoThreshold && jogou)
                    //                {
                    //                    FlowAction(player.picoExpiratorio, 3);
                    //                    yield return null;
                    //                }

                    //                //voltou pro 0
                    //                finalScoreMenu.pikeString[2] = player.picoExpiratorio.ToString();
                    //                TextPanel.SetActive(true);

                    //                if (jogou)
                    //                {
                    //                    displayHowTo.text = "Parabéns!\nPressione [Enter] para ver a sua pontuação.";
                    //                    SoundManager.Instance.PlaySound("Success");
                    //                }

                    //                player.picoExpiratorio = 0;
                    //                scm.StopSampling();
                    //                FindObjectOfType<ManoLogger>().StopLogging();

                    //                break;
                    //        }
                    //        ppasso = false;
                    //    }
                    //    else
                    //    {
                    //        // Se a CINTA Extensora estiver conectada
                    //        if (scc.IsConnected)
                    //        {
                    //            switch (passo)
                    //            {
                    //                case 1:
                    //                    displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir dentro do tempo.";
                    //                    break;

                    //                case 2:

                    //                    scc.StartSampling();
                    //                    scc.Recalibrate();


                    //                    displayHowTo.text = "";

                    //                    TextPanel.SetActive(false);

                    //                    while (player.sensorValue <= Pacient.Loaded.CintaThreshold * 2 && jogou)
                    //                        yield return null;

                    //                    StopCountdown();
                    //                    paraTempo = true;

                    //                    //saiu do 0
                    //                    while (player.sensorValue > Pacient.Loaded.CintaThreshold && jogou)
                    //                    {
                    //                        FlowAction(player.picoExpiratorio, 1);
                    //                        yield return null;
                    //                    }

                    //                    //voltou pro 0
                    //                    finalScoreMenu.pikeString[0] = player.picoExpiratorio.ToString();
                    //                    TextPanel.SetActive(true);

                    //                    if (jogou)
                    //                    {
                    //                        displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                    //                        SoundManager.Instance.PlaySound("Success");
                    //                    }

                    //                    player.picoExpiratorio = 0;
                    //                    break;

                    //                case 3:
                    //                    RestauraVariaveis();
                    //                    break;

                    //                case 4:
                    //                    displayHowTo.text = "";
                    //                    TextPanel.SetActive(false);
                    //                    while (player.sensorValue <= Pacient.Loaded.CintaThreshold * 2 && jogou)
                    //                        yield return null;

                    //                    StopCountdown();
                    //                    paraTempo = true;

                    //                    //saiu do 0
                    //                    while (player.sensorValue > Pacient.Loaded.CintaThreshold && jogou)
                    //                    {
                    //                        FlowAction(player.picoExpiratorio, 2);
                    //                        yield return null;
                    //                    }

                    //                    //voltou pro 0
                    //                    finalScoreMenu.pikeString[1] = player.picoExpiratorio.ToString();
                    //                    TextPanel.SetActive(true);

                    //                    if (jogou)
                    //                    {
                    //                        displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                    //                        SoundManager.Instance.PlaySound("Success");
                    //                    }

                    //                    player.picoExpiratorio = 0;
                    //                    break;

                    //                case 5:
                    //                    RestauraVariaveis();
                    //                    break;

                    //                case 6:
                    //                    displayHowTo.text = "";
                    //                    TextPanel.SetActive(false);
                    //                    while (player.sensorValue <= Pacient.Loaded.CintaThreshold * 2 && jogou)
                    //                        yield return null;

                    //                    StopCountdown();
                    //                    paraTempo = true;

                    //                    //saiu do 0
                    //                    while (player.sensorValue > Pacient.Loaded.CintaThreshold && jogou)
                    //                    {
                    //                        FlowAction(player.picoExpiratorio, 3);
                    //                        yield return null;
                    //                    }

                    //                    //voltou pro 0
                    //                    finalScoreMenu.pikeString[2] = player.picoExpiratorio.ToString();
                    //                    TextPanel.SetActive(true);

                    //                    if (jogou)
                    //                    {
                    //                        displayHowTo.text = "Parabéns!\nPressione [Enter] para ver a sua pontuação.";
                    //                        SoundManager.Instance.PlaySound("Success");
                    //                    }

                    //                    player.picoExpiratorio = 0;
                    //                    scc.StopSampling();
                    //                    FindObjectOfType<CintaLogger>().StopLogging();

                    //                    break;
                    //            }
                    //            ppasso = false;
                    //        }
                    //    }
                    //}

                    #endregion

                }
                yield return null;
            }
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
                picoJogador = Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow;

            }
            //else
            //{
            //    if (scm.IsConnected) // Se Mano conectado
            //    {
            //        picoJogador = Pacient.Loaded.CapacitiesMano.ExpPeakFlow;

            //    }
            //    else
            //    {
            //        if (scc.IsConnected) // Se CINTA conectada
            //        {
            //            picoJogador = Pacient.Loaded.CapacitiesCinta.ExpPeakFlow;

            //        }
            //    }
            //}

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
            if (percentage > 1.00f)
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

        private void Start()
        {
            passo = 0;
            ppasso = false;
            partidaCompleta = false;
            displayHowTo.text = "Pressione [Enter] para começar.";
            StartCoroutine(PlayGame());

            slider.minValue = 0;    //adicionado 09/09/19



            if (scp.IsConnected) // Se PITACO conectado
            {
                SpicoExpiratorio = Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow;

            }
            //else
            //{
            //    if (scm.IsConnected) // Se Mano conectado
            //    {
            //        SpicoExpiratorio = Pacient.Loaded.CapacitiesMano.ExpPeakFlow;

            //    }
            //    else
            //    {
            //        if (scc.IsConnected) // Se CINTA conectada
            //        {
            //            SpicoExpiratorio = Pacient.Loaded.CapacitiesCinta.ExpPeakFlow;

            //        }
            //    }
            //}



            slider.maxValue = SpicoExpiratorio;         //adicionado 09/09/19


            sliderpico.minValue = 0;    //adicionado 16/10/19
            sliderpico.maxValue = SpicoExpiratorio;         //adicionado 16/10/19
        }




        // Update is called once per frame
        private void Update()
        {

            slider.value = player.sensorValue;  //adicionado 09/09/19

            if (passo == 2 || passo == 4 || passo == 6)                     //adicionado 16/10/19
            {
                if (player.sensorValue > picomomento)                     //adicionado 16/10/19
                    picomomento = player.sensorValue;                     //adicionado 16/10/19
                sliderpico.value = picomomento;                      //adicionado 16/10/19
            }
            else                                                    //adicionado 16/10/19
            {
                sliderpico.value = 0;                     //adicionado 16/10/19
                picomomento = 0;
            }


            //print(passo);
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