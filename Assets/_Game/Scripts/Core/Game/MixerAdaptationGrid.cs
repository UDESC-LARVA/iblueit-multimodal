using Ibit.Core.Database;
using Ibit.Plataform.UI;
using Ibit.Plataform.Manager.Spawn;
using Ibit.Plataform.Data;
using UnityEngine;

namespace Ibit.Core.Game
{
    public partial class MixerManager
    {
        private float SignalSamplesRateOxi;
        private float timeBetweenSamplesOxi;
        private float currenttimeOxi;

        int limitSPORegular = 2; // Número de leituras regulares para fazer adaptação
        int limitSPODanger = 2; // Número de leituras perigosas para fazer adaptação
        int countSPORegular = 0; // Contador de leituras com Oxigenação Regular
        int countSPODanger = 0; // Contador de leituras com Oxigenação Perigosa

        [SerializeField] private CanvasManager _canvasManager;
        [SerializeField] private Spawner _spawner;

        private void OnEnable()
        {
            // Taxa de amostragem do "Tratamento de Sinais" por minuto do Oxímetro
            SignalSamplesRateOxi = SignalTreatmentDb.LoadSignalParameters("O");
            timeBetweenSamplesOxi = 60 / SignalSamplesRateOxi;  // 60 segundos / amostragem desejada = tempo de intervalo entre cada amostra do sinal
        }

        void AdaptationGrid(float signalPitaco, float signalMano, float signalCinta, float signalOxiSPO, float signalOxiHR)
        {
            if (_spawner == null){
                _spawner = FindObjectOfType<Spawner>();
            }

            if (_spawner.SpawnedObjects.Count < 1) // Se não tiver objetos não os testes não são feitos
                return;


            if (Time.time > currenttimeOxi + timeBetweenSamplesOxi) // Executa a quantidade de leituras escolhida pelo usuário no arquivo "_signalTreatment.csv" por minuto.
            {
                //* AvaliationNode
                if (signalOxiSPO != 0 && signalOxiSPO >= 0 && signalOxiSPO < ParametersDb.parameters.MinimumNormalOxygenation) // ParametersDb.parameters.MinimumRegularOxygenation
                {
                    countSPORegular += 1;
                    if (countSPORegular >= limitSPORegular)
                    {
                        GameObject.Find("Canvas").transform.Find("Security Panel 1").gameObject.SetActive(true);
                        if (_canvasManager == null){
                            _canvasManager = FindObjectOfType<CanvasManager>();
                        }
                        _canvasManager.PauseGametoShowAlert(); // Pausa jogo

                        if (_spawner == null){
                            _spawner = FindObjectOfType<Spawner>();
                        }

                        if ((StageModel.Loaded.ObjectSpeedFactor * ParametersDb.parameters.ObjectsSpeedFactor) >= 2)
                            _spawner.speedReductionAdaptation = 1;  // Diminuir velocidade em um nível
                        // TODO: Todos os objetos (Alvos e Obst.) são spawnados no início do nível, então não tem como mudar a velocidade no meio da fase, tentar fazer algo...
                        // TODO: speedReductionAdaptation = 1 não se mantém para próximos níveis, pois ele é zerado novamente.


                    }
                } else {
                    if (signalOxiSPO != 0 && signalOxiSPO < ParametersDb.parameters.MinimumRegularOxygenation)
                    {

                        //  if (_spawner == null){
                        //     _spawner = FindObjectOfType<Spawner>();
                        // }
                        // _spawner.speedReductionAdaptation = 2;  // Diminuir velocidade pela metade

                        countSPODanger += 1;
                        if (countSPODanger >= limitSPODanger)
                        {
                            GameObject.Find("Canvas").transform.Find("Security Panel 2").gameObject.SetActive(true);
                            if (_canvasManager == null){
                                _canvasManager = FindObjectOfType<CanvasManager>();
                            }
                            _canvasManager.PauseGametoShowAlert(); // Pausa jogo
                        }
                    } else {
                        countSPORegular = 0;
                        countSPODanger = 0;

                        // if (_spawner == null){
                        //     _spawner = FindObjectOfType<Spawner>();
                        // }
                        // _spawner.speedReductionAdaptation = 1;  // Volta velocidade ao normal
                    }
                }
                currenttimeOxi = Time.time; // currenttimeOxi é atualizado para o tempo atual (Time.time é um contador de segundos que começa quando o jogo é executado)
            }

            //* FlowNode
        }

        // void AvaliationNode()
        // {

        //     //if()
        // }

        // void FlowNode()
        // {

        // }
    }
}