using Ibit.Core.Data;
using Ibit.Core.Serial;
using Ibit.Core.Util;
using Ibit.Core.Audio;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core;

namespace Ibit.CakeGame
{
    public class RoundManager : MonoBehaviour
    {
        private bool jogou = true;
        private bool paraTempo;
        private bool partidaCompleta;
        private bool ppasso;
        private int passo;
        private float timer = 10;

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;
        

        public float SpicoExpiratorio;       // adicionado 09/09/19


        public Slider slider; // adicionado 09/09/19

        public Slider sliderpico; // adicionado 16/10/19
        float picomomento=0;      // adicionado 16/10/19
        int flag = 0;



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
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();

        }

        private void Start()
        {
            passo = 0;
            ppasso = false;
            partidaCompleta = false;
            displayHowTo.text = "Pressione [Enter] para começar.";
            StartCoroutine(PlayGame());

            slider.minValue = 0;    //adicionado 11/04/20


            if (scp.IsConnected) // Se Pitaco conectado
            {
                SpicoExpiratorio = Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow;

            } else {
            if (scm.IsConnected) // Se Mano conectado
            {
                SpicoExpiratorio = Pacient.Loaded.CapacitiesMano.ExpPeakFlow;

            } else {
            if (scc.IsConnected) // Se Cinta conectada
            {
                SpicoExpiratorio = Pacient.Loaded.CapacitiesCinta.ExpPeakFlow;

            }}}



            slider.maxValue = SpicoExpiratorio;         //adicionado 09/09/19


            sliderpico.minValue = 0;    //adicionado 16/10/19
            sliderpico.maxValue = SpicoExpiratorio;         //adicionado 16/10/19
        }

        private IEnumerator PlayGame()
        {
            while (!partidaCompleta)
            {
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

                    // Se o Pitaco estiver conectado
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

                             while (player.sensorValue <= Pacient.Loaded.PitacoThreshold && jogou)
                                 yield return null;

                             StopCountdown();
                             paraTempo = true;

                             //saiu do 0
                             while (player.sensorValue > Pacient.Loaded.PitacoThreshold && jogou)
                             {
                                 FlowAction(player.picoExpiratorio);
                                 yield return null;
                             }

                             //voltou pro 0
                             finalScoreMenu.pikeString[0] = player.picoExpiratorio.ToString();
                             TextPanel.SetActive(true);

                             if (jogou)
                             {
                                 displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                                 SoundManager.Instance.PlaySound("Success");
                             }

                             player.picoExpiratorio = 0;
                             break;

                         case 3:
                             RestauraVariaveis();
                             break;

                         case 4:
                             displayHowTo.text = "";
                             TextPanel.SetActive(false);
                             while (player.sensorValue <= Pacient.Loaded.PitacoThreshold && jogou)
                                 yield return null;

                             StopCountdown();
                             paraTempo = true;

                             //saiu do 0
                             while (player.sensorValue > Pacient.Loaded.PitacoThreshold && jogou)
                             {
                                 FlowAction(player.picoExpiratorio);
                                 yield return null;
                             }

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
                             RestauraVariaveis();
                             break;

                         case 6:
                             displayHowTo.text = "";
                             TextPanel.SetActive(false);
                             while (player.sensorValue <= Pacient.Loaded.PitacoThreshold && jogou)
                                 yield return null;

                             StopCountdown();
                             paraTempo = true;

                             //saiu do 0
                             while (player.sensorValue > Pacient.Loaded.PitacoThreshold && jogou)
                             {
                                 FlowAction(player.picoExpiratorio);
                                 yield return null;
                             }

                             //voltou pro 0
                             finalScoreMenu.pikeString[2] = player.picoExpiratorio.ToString();
                             TextPanel.SetActive(true);

                             if (jogou)
                             {
                                 displayHowTo.text = "Parabéns!\nPressione [Enter] para ver a sua pontuação.";
                                 SoundManager.Instance.PlaySound("Success");
                             }

                             player.picoExpiratorio = 0;
                             scp.StopSampling();
                             FindObjectOfType<PitacoLogger>().StopLogging();

                             break;
                     }
                     ppasso = false;
                    } else { ////////////////////
                    // Se o Mano estiver conectado
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

                             while (player.sensorValue <= Pacient.Loaded.ManoThreshold && jogou)
                                 yield return null;

                             StopCountdown();
                             paraTempo = true;

                             //saiu do 0
                             while (player.sensorValue > Pacient.Loaded.ManoThreshold && jogou)
                             {
                                 FlowAction(player.picoExpiratorio);
                                 yield return null;
                             }

                             //voltou pro 0
                             finalScoreMenu.pikeString[0] = player.picoExpiratorio.ToString();
                             TextPanel.SetActive(true);

                             if (jogou)
                             {
                                 displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                                 SoundManager.Instance.PlaySound("Success");
                             }

                             player.picoExpiratorio = 0;
                             break;

                         case 3:
                             RestauraVariaveis();
                             break;

                         case 4:
                             displayHowTo.text = "";
                             TextPanel.SetActive(false);
                             while (player.sensorValue <= Pacient.Loaded.ManoThreshold && jogou)
                                 yield return null;

                             StopCountdown();
                             paraTempo = true;

                             //saiu do 0
                             while (player.sensorValue > Pacient.Loaded.ManoThreshold && jogou)
                             {
                                 FlowAction(player.picoExpiratorio);
                                 yield return null;
                             }

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
                             RestauraVariaveis();
                             break;

                         case 6:
                             displayHowTo.text = "";
                             TextPanel.SetActive(false);
                             while (player.sensorValue <= Pacient.Loaded.ManoThreshold && jogou)
                                 yield return null;

                             StopCountdown();
                             paraTempo = true;

                             //saiu do 0
                             while (player.sensorValue > Pacient.Loaded.ManoThreshold && jogou)
                             {
                                 FlowAction(player.picoExpiratorio);
                                 yield return null;
                             }

                             //voltou pro 0
                             finalScoreMenu.pikeString[2] = player.picoExpiratorio.ToString();
                             TextPanel.SetActive(true);

                             if (jogou)
                             {
                                 displayHowTo.text = "Parabéns!\nPressione [Enter] para ver a sua pontuação.";
                                 SoundManager.Instance.PlaySound("Success");
                             }

                             player.picoExpiratorio = 0;
                             scm.StopSampling();
                             FindObjectOfType<ManoLogger>().StopLogging();

                             break;
                     }
                     ppasso = false;
                    } else {
                    // Se a Cinta Extensora estiver conectada
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

                             while (player.sensorValue <= Pacient.Loaded.CintaThreshold && jogou)
                                 yield return null;

                             StopCountdown();
                             paraTempo = true;

                             //saiu do 0
                             while (player.sensorValue > Pacient.Loaded.CintaThreshold && jogou)
                             {
                                 FlowAction(player.picoExpiratorio);
                                 yield return null;
                             }

                             //voltou pro 0
                             finalScoreMenu.pikeString[0] = player.picoExpiratorio.ToString();
                             TextPanel.SetActive(true);

                             if (jogou)
                             {
                                 displayHowTo.text = "Parabéns!\nPressione [Enter] para ir para a proxima rodada.";
                                 SoundManager.Instance.PlaySound("Success");
                             }

                             player.picoExpiratorio = 0;
                             break;

                         case 3:
                             RestauraVariaveis();
                             break;

                         case 4:
                             displayHowTo.text = "";
                             TextPanel.SetActive(false);
                             while (player.sensorValue <= Pacient.Loaded.CintaThreshold && jogou)
                                 yield return null;

                             StopCountdown();
                             paraTempo = true;

                             //saiu do 0
                             while (player.sensorValue > Pacient.Loaded.CintaThreshold && jogou)
                             {
                                 FlowAction(player.picoExpiratorio);
                                 yield return null;
                             }

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
                             RestauraVariaveis();
                             break;

                         case 6:
                             displayHowTo.text = "";
                             TextPanel.SetActive(false);
                             while (player.sensorValue <= Pacient.Loaded.CintaThreshold && jogou)
                                 yield return null;

                             StopCountdown();
                             paraTempo = true;

                             //saiu do 0
                             while (player.sensorValue > Pacient.Loaded.CintaThreshold && jogou)
                             {
                                 FlowAction(player.picoExpiratorio);
                                 yield return null;
                             }

                             //voltou pro 0
                             finalScoreMenu.pikeString[2] = player.picoExpiratorio.ToString();
                             TextPanel.SetActive(true);

                             if (jogou)
                             {
                                 displayHowTo.text = "Parabéns!\nPressione [Enter] para ver a sua pontuação.";
                                 SoundManager.Instance.PlaySound("Success");
                             }

                             player.picoExpiratorio = 0;
                             scc.StopSampling();
                             FindObjectOfType<CintaLogger>().StopLogging();

                             break;
                     }
                     ppasso = false;
                    }}} ////////////////////////////////////////
                    
                }
                yield return null;
            }
        }

        private void RestauraVariaveis()
        {
            displayHowTo.text = "Pressione [Enter] e assopre o mais forte que conseguir";
            timer = 10;
            jogou = true;
            paraTempo = false;
        }

        #region Calculating Flow Percentage

        public void FlowAction(float flowValue)
        {
            var picoJogador = 0f;

            if (scp.IsConnected) // Se Pitaco conectado
            {
                picoJogador = Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow;

            } else {
            if (scm.IsConnected) // Se Mano conectado
            {
                picoJogador = Pacient.Loaded.CapacitiesMano.ExpPeakFlow;

            } else {
            if (scc.IsConnected) // Se Cinta conectada
            {
                picoJogador = Pacient.Loaded.CapacitiesCinta.ExpPeakFlow;

            }}}
            
            var percentage = flowValue / picoJogador;

            // Debug.Log($"flowValue: {flowValue}");
            // Debug.Log($"picoJogador: {picoJogador}");
            // Debug.Log($"percentage: {percentage}");

            if (percentage >= 0.333f)  //Modificado 02/10/19 Diogo. Original: 25,50 e 75%
            {
                candle.TurnOff(0);
                candle.TurnOff(1);
                candle.TurnOff(2);
                candle.TurnOff(3);
                candle.TurnOff(4);
                candle.TurnOff(5);
                score.FillStars(0);
                finalScore[(passo / 2) - 1] = 1;
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
                finalScore[(passo / 2) - 1] = 2;
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
                finalScore[(passo / 2) - 1] = 3;
            }

            FindObjectOfType<Core.MinigameLogger>().Write(flowValue);
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
 
            slider.value = player.sensorValue;  //adicionado 09/09/19
            //Debug.Log($"Slider Value: {slider.value}");

            if (passo == 2 || passo == 4 || passo == 6)                     //adicionado 16/10/19
            {
                if (player.sensorValue > picomomento)                     //adicionado 16/10/19
                    picomomento = player.sensorValue;                     //adicionado 16/10/19
                    sliderpico.value = picomomento;                      //adicionado 16/10/19
            }
            else                                                    //adicionado 16/10/19
            {
                sliderpico.value =  0;                     //adicionado 16/10/19
                picomomento = 0;
            }
            

            //print(passo);
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ppasso = true;
                passo++;
            }

            if (passo > 6)
            {
                partidaCompleta = true;
                passo = 6;
                displayHowTo.text = "";
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

            //ToDo - Isso aqui é executado somente uma vez, mas dentro do update é executado a cada frame
            // Colocar em algum metodo depois para que seja chamado somente uma vez
            if (partidaCompleta)
            {

                // Band aid fix para não ficar chamando várias vezes
                if(flag < 2)
                {
                   //SoundManager.Instance.PlaySound("Finished");
                    TextPanel.SetActive(false);
                    finalScoreMenu.DisplayFinalScore(finalScore[0], finalScore[1], finalScore[2]);
                    finalScoreMenu.ToggleScoreMenu();

                    if(flag < 1)
                    {
                        Debug.Log("Saving minigame data...");
                        FindObjectOfType<Core.MinigameLogger>().Save();
                        Debug.Log("Minigame logs saved.");
                    }
                    

                } else {

                    partidaCompleta = false;

                }

                passo = 0;
                flag++;
            }
        }
    }
}