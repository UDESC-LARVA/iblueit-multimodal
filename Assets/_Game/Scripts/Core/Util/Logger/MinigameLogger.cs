using System.Collections.Generic;
using Assets._Game.Scripts.Core.Api.Dto;
using Assets._Game.Scripts.Core.Api.Extensions;
using Ibit.Core.Data;
using Ibit.Core.Data.Enums;
using Ibit.Core.Data.Manager;
using Ibit.Core.Util;
using Ibit.Core.Serial;
using UnityEngine;

namespace Ibit.Core
{
    public class MinigameLogger : MonoBehaviour
    {

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;
        private SerialControllerOximetro sco;

        private MinigameOverviewSendDto _minigameOverviewSendDto;
        private PitacoLogger _pitacoLogger;
        private ManoLogger _manoLogger;
        private CintaLogger _cintaLogger;
        private OximetroLogger _oximetroLogger;

        private void Awake()
        {
            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();
            sco = FindObjectOfType<SerialControllerOximetro>();


            _minigameOverviewSendDto = new MinigameOverviewSendDto { FlowDataRound = new List<FlowDataMinigame>() };
            _pitacoLogger = FindObjectOfType<PitacoLogger>();
            _manoLogger = FindObjectOfType<ManoLogger>();
            _cintaLogger = FindObjectOfType<CintaLogger>();
            _oximetroLogger = FindObjectOfType<OximetroLogger>();
        }

        public void WriteMinigameRound(int round, int score, float flowScore)
        {
            var flowDataMinigame = new FlowDataMinigame
            {
                MinigameRound = round,
                RoundScore = score,
                RoundFlowScore = flowScore,
            };


            if (scp.IsConnected) // Se PITACO conectado
            {
                Debug.Log("MinigameLogger loaded - Device: Pitaco.");

                flowDataMinigame.FlowDataDevices.Add(new FlowDataDevice
                {
                    DeviceName = GameDevice.Pitaco.GetDescription(),
                    FlowData = new List<FlowData>(_pitacoLogger.flowDataDevice.FlowData)
                });

                _minigameOverviewSendDto.FlowDataRound.Add(flowDataMinigame);

                _pitacoLogger.flowDataDevice.FlowData.Clear();

            } else {
            if (scm.IsConnected) // Se MANO conectado
            {
                Debug.Log("MinigameLogger loaded - Device: Mano.");

                flowDataMinigame.FlowDataDevices.Add(new FlowDataDevice
                {
                    DeviceName = GameDevice.Mano.GetDescription(),
                    FlowData = new List<FlowData>(_manoLogger.flowDataDevice.FlowData)
                });

                _minigameOverviewSendDto.FlowDataRound.Add(flowDataMinigame);

                _manoLogger.flowDataDevice.FlowData.Clear();

            } else {
            if (scc.IsConnected) // Se CINTA conectada
            {
                Debug.Log("MinigameLogger loaded - Device: Cinta.");

                flowDataMinigame.FlowDataDevices.Add(new FlowDataDevice
                {
                    DeviceName = GameDevice.Cinta.GetDescription(),
                    FlowData = new List<FlowData>(_cintaLogger.flowDataDevice.FlowData)
                });

                _minigameOverviewSendDto.FlowDataRound.Add(flowDataMinigame);

                _cintaLogger.flowDataDevice.FlowData.Clear();
            }}}

            if (sco.IsConnected) // Se OXÍMETRO conectado
            {
                Debug.Log("MinigameLogger loaded - Device: Oxímetro.");

                flowDataMinigame.FlowDataDevices.Add(new FlowDataDevice
                {
                    DeviceName = GameDevice.Oximetro.GetDescription(),
                    FlowData = new List<FlowData>(_oximetroLogger.flowDataDevice.FlowData)
                });

                _minigameOverviewSendDto.FlowDataRound.Add(flowDataMinigame);

                _oximetroLogger.flowDataDevice.FlowData.Clear();

            }
        }

        public async void Save(GameDevice gameDevice, RespiratoryExercise exercise, Minigame minigameName)
        {
            _minigameOverviewSendDto.PacientId = Pacient.Loaded.IdApi;
            _minigameOverviewSendDto.Exercise = exercise.GetDescription();
            _minigameOverviewSendDto.MinigameName = minigameName.GetDescription();

            var response = await DataManager.Instance.SaveMinigameOverview(_minigameOverviewSendDto);

            if (response.ApiResponse == null)
                SysMessage.Info("Erro ao salvar na nuvem!\n Os dados poderão ser enviados posteriormente.");
        }
    }
}