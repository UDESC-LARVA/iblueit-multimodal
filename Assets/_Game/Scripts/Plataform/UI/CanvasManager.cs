using System.Linq;
using Ibit.Core.Database;
using Ibit.Core.Data;
using Ibit.Core.Game;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Score;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.Plataform.UI
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private Text _stageLevel;
        [SerializeField] private Text _stagePhase;
        [SerializeField] private GameObject _pauseMenu;
        // [SerializeField] private GameObject _pauseMenuParameters;
        [SerializeField] private GameObject _helpPanel;
        [SerializeField] private GameObject _resultPanel;
        [SerializeField] private GameObject _parametersPanel;
        [SerializeField] private GameObject _securityPanel1;
        [SerializeField] private GameObject _securityPanel2;

        public Parameters CurrentParameters;

        private void OnEnable()
        {
            _stageLevel.text = StageModel.Loaded.Level.ToString();
            _stagePhase.text = StageModel.Loaded.Phase.ToString();

            CurrentParameters = ParametersDb.parameters;
        }

        public void PauseGame()
        {
            if (_resultPanel.activeSelf)
                return;

            if (GameManager.GameIsPaused)
                return;

            _helpPanel.SetActive(true);
            _pauseMenu.SetActive(true);
            GameManager.PauseGame();
        }

        public void PauseGametoShowParameters()
        {
            if (_resultPanel.activeSelf)
                return;

            if (GameManager.GameIsPaused)
                return;

            GameManager.PauseGame();
        }

        public void PauseGametoShowAlert()
        {
            if (_resultPanel.activeSelf)
                return;

            if (GameManager.GameIsPaused)
                return;

            GameManager.PauseGame();
        }

        public void UnPauseGame()
        {
            if (_resultPanel.activeSelf)
                return;

            if (!GameManager.GameIsPaused)
                return;

            _helpPanel.SetActive(false);
            _pauseMenu.SetActive(false);
            _parametersPanel.SetActive(false);
            _securityPanel1.SetActive(false);
            // _securityPanel2.SetActive(false);
            GameManager.UnPauseGame();
        }

        public void ShowMultimodalParameters ()
        {
            GameObject.Find("Canvas").transform.Find("Parameters Panel").gameObject.SetActive(true);
            
            var InfoModal = GameObject.FindWithTag("txModal").gameObject.GetComponent<Text>();
            var Infos0 = GameObject.FindWithTag("txInfos0").gameObject.GetComponent<Text>();
            var Infos1 = GameObject.FindWithTag("txInfos1").gameObject.GetComponent<Text>();
            var Infos2 = GameObject.FindWithTag("txInfos2").gameObject.GetComponent<Text>();

            // Mostra o nome completo dos dispositivos conectados no momento
            if (CurrentParameters.device == "P")
            {
                Infos0.text =   $"Dispositivo(s): Pitaco";
            } else {
                if(CurrentParameters.device == "M")
                {
                    Infos0.text =   $"Dispositivo(s): Mano-BD";
            } else {
                if(CurrentParameters.device == "C")
                {
                    Infos0.text =   $"Dispositivo(s): Cinta";
            } else {
                if(CurrentParameters.device == "PC")
                {
                    Infos0.text =   $"Dispositivo(s): Pitaco e Cinta";
            } else {
                if(CurrentParameters.device == "MC")
                {
                    Infos0.text =   $"Dispositivo(s): Mano-BD e Cinta";
            } else {
                if(CurrentParameters.device == "PO")
                {
                    Infos0.text =   $"Dispositivo(s): Pitaco e Oxímetro";
            } else {
                if(CurrentParameters.device == "MO")
                {
                    Infos0.text =   $"Dispositivo(s): Mano-BD e Oxímetro";
            } else {
                if(CurrentParameters.device == "CO")
                {
                    Infos0.text =   $"Dispositivo(s): Cinta e Oxímetro";
            } else {
                if(CurrentParameters.device == "PCO")
                {
                    Infos0.text =   $"Dispositivo(s): Pitaco, Cinta e Oxímetro";
            } else {
                if(CurrentParameters.device == "MCO")
                {
                    Infos0.text =   $"Dispositivo(s): Mano-BD, Cinta e Oxímetro";
            }
            }}}}}}}}}


            // Mostra se é uma Interação Unimodal, ou Multimodal
            if (CurrentParameters.device == "P" || CurrentParameters.device == "M" || CurrentParameters.device == "C")
            {
                InfoModal.text = "INTERAÇÃO UNIMODAL";
            } else {
                InfoModal.text = "INTERAÇÃO MULTIMODAL";
            }

        //? ################## FUSÃO ##################
            // Os parâmetros de fusão só aparecem caso a interação seja Multimodal
            if (InfoModal.text == "INTERAÇÃO MULTIMODAL")
            {
                if (CurrentParameters.FusionType == "Soma")
                {
                    Infos1.text =   $"Tipo de Fusão: Sinais (Soma)\n" +
                                    $"  • Soma de sinais\n";
                } else {
                    if (CurrentParameters.FusionType == "Subtracao")
                    {
                        Infos1.text =   $"Tipo de Fusão: Sinais (Subtração)\n" +
                                        $"  • Subtrair dispositivo: {CurrentParameters.FusionSubDevice}\n";
                    } else {
                        if (CurrentParameters.FusionType == "Preferencial")
                        {
                            Infos1.text =   $"Tipo de Fusão: Sinais (Preferencial)\n" +
                                            $"  • Preferencia Pitaco: {CurrentParameters.FusionPrefPitaco}%\n" +
                                            $"  • Preferencia Mano: {CurrentParameters.FusionPrefMano}%\n" +
                                            $"  • Preferencia Cinta: {CurrentParameters.FusionPrefCinta}%\n";
                        } else {
                            if (CurrentParameters.FusionType == "Funcao")
                            {
                                Infos1.text =   $"Tipo de Fusão: Função (temporal)\n" +
                                                $"  • Função - INS: {CurrentParameters.FusionFunctIns}\n" +
                                                $"  • Função - EXP: {CurrentParameters.FusionFunctExp}\n";
                            }
                        }
                    }
                }
            }

        //? ################ ADAPTAÇÃO ################
            // Se a Cinta extiver conectada
            if(CurrentParameters.device == "C" || CurrentParameters.device == "PC" || CurrentParameters.device == "MC" || CurrentParameters.device == "CO" || CurrentParameters.device == "PCO" || CurrentParameters.device == "MCO")
            {
                // Se o Oxímetro estiver conectado
                if (CurrentParameters.device == "CO" || CurrentParameters.device == "PCO" || CurrentParameters.device == "MCO")
                {
                    Infos2.text =   $"Distância adicional entre Objetos: +{CurrentParameters.AdditionalDistance}\n" +
                                    $"Fator de cálculo da velocidade: *{CurrentParameters.ObjectsSpeedFactor}\n" +
                                    $"Fator de Cálculo da Pontuação: *{CurrentParameters.ScoreCalculationFactor}\n" +
                                    $"Valor mínimo exigido da Cinta de Pressão: {CurrentParameters.MinimumExtensionBelt}%\n" +
                                    $"  • Golfinho Blue fica laranja.\n\n" +

                                    $"% Oxigenação Normal Mínima: {CurrentParameters.MinimumNormalOxygenation}%\n" +
                                    $"  • Diminuir velocidade em um nível.\n" +
                                    $"% Oxigenação Regular Mínima: {CurrentParameters.MinimumRegularOxygenation}%\n" +
                                    $"  • Pausar jogo e lançar alerta.\n\n" +

                                    $"Nº derrotas p/ adapt. A: {CurrentParameters.lostWtimes}\n" +
                                    $"  • Fator de decremento da ALTURA dos Alvos: *{CurrentParameters.decreaseHeight}\n" +
                                    $"  • Fator de decremento do TAMANHO dos Obstáculos.: *{CurrentParameters.decreaseSize}\n\n" +

                                    $"Nº derrotas p/ adapt. B: {CurrentParameters.lostXtimes}\n" +
                                    $"  • Solicitar Recalibração.";
                } else {
                    Infos2.text =   $"Distância adicional entre Objetos: +{CurrentParameters.AdditionalDistance}\n" +
                                    $"Fator de cálculo da velocidade: *{CurrentParameters.ObjectsSpeedFactor}\n" +
                                    $"Fator de Cálculo da Pontuação: *{CurrentParameters.ScoreCalculationFactor}\n" +
                                    $"Valor mínimo exigido da Cinta de Pressão: {CurrentParameters.MinimumExtensionBelt}%\n" +
                                    $"  • Golfinho Blue fica laranja.\n\n" +

                                    $"Nº derrotas p/ adapt. A: {CurrentParameters.lostWtimes}\n" +
                                    $"  • Fator de decremento da ALTURA dos Alvos: *{CurrentParameters.decreaseHeight}\n" +
                                    $"  • Fator de decremento do TAMANHO dos Obstáculos.: *{CurrentParameters.decreaseSize}\n\n" +

                                    $"Nº derrotas p/ adapt. B: {CurrentParameters.lostXtimes}\n" +
                                    $"  • Solicitar Recalibração.";
                }
            } 
            
            // Qualquer combinação sem a Cinta
            if (CurrentParameters.device == "P" || CurrentParameters.device == "M" || CurrentParameters.device == "PM" || CurrentParameters.device == "PO" || CurrentParameters.device == "MO" || CurrentParameters.device == "PMO")
            {
                // Se o Oxímetro estiver conectado
                if (CurrentParameters.device == "PO" || CurrentParameters.device == "MO" || CurrentParameters.device == "PMO")
                {
                    Infos2.text =   $"Distância adicional entre Objetos: +{CurrentParameters.AdditionalDistance}\n" +
                                    $"Fator de cálculo da velocidade: *{CurrentParameters.ObjectsSpeedFactor}\n" +
                                    $"Fator de Cálculo da Pontuação: *{CurrentParameters.ScoreCalculationFactor}\n\n" +

                                    $"% Oxigenação Normal Mínima: {CurrentParameters.MinimumNormalOxygenation}%\n" +
                                    $"  • Diminuir velocidade em um nível.\n" +
                                    $"% Oxigenação Regular Mínima: {CurrentParameters.MinimumRegularOxygenation}%\n" +
                                    $"  • Pausar jogo e lançar alerta.\n\n" +

                                    $"Nº derrotas p/ adapt. A: {CurrentParameters.lostWtimes}\n" +
                                    $"  • Fator de decremento da ALTURA dos Alvos: *{CurrentParameters.decreaseHeight}\n" +
                                    $"  • Fator de decremento do TAMANHO dos Obstáculos.: *{CurrentParameters.decreaseSize}\n\n" +

                                    $"Nº derrotas p/ adapt. B: {CurrentParameters.lostXtimes}\n" +
                                    $"  • Solicitar Recalibração.";
                } else {
                    Infos2.text =   $"Distância adicional entre Objetos: +{CurrentParameters.AdditionalDistance}\n" +
                                    $"Fator de cálculo da velocidade: *{CurrentParameters.ObjectsSpeedFactor}\n" +
                                    $"Fator de Cálculo da Pontuação: *{CurrentParameters.ScoreCalculationFactor}\n\n" +

                                    $"Nº derrotas p/ adapt. A: {CurrentParameters.lostWtimes}\n" +
                                    $"  • Fator de decremento da ALTURA dos Alvos: *{CurrentParameters.decreaseHeight}\n" +
                                    $"  • Fator de decremento do TAMANHO dos Obstáculos.: *{CurrentParameters.decreaseSize}\n\n" +

                                    $"Nº derrotas p/ adapt. B: {CurrentParameters.lostXtimes}\n" +
                                    $"  • Solicitar Recalibração.";
                }
                
            }

        }

        public void SetNextStage()
        {
            StageModel.Loaded = StageDb.Instance.GetStage(
                StageModel.Loaded.Id + 1 > StageDb.Instance.StageList.Max(x => x.Id) ? 1 : StageModel.Loaded.Id + 1);
        }
    }
}