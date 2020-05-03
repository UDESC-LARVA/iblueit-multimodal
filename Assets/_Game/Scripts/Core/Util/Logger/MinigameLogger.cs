using System.Collections.Generic;
using Assets._Game.Scripts.Core.Api.Dto;
using Assets._Game.Scripts.Core.Api.Extensions;
using Ibit.Core.Data;
using Ibit.Core.Data.Enums;
using Ibit.Core.Data.Manager;
using Ibit.Core.Util;
using UnityEngine;

namespace Ibit.Core
{
    public class MinigameLogger : MonoBehaviour
    {
        private MinigameOverviewSendDto _minigameOverviewSendDto;
        private PitacoLogger _pitacoLogger;

        private void Awake()
        {
            _minigameOverviewSendDto = new MinigameOverviewSendDto { FlowDataRound = new List<FlowDataMinigame>() };
            _pitacoLogger = FindObjectOfType<PitacoLogger>();

            Debug.Log("MinigameLogger loaded.");
        }

        public void WriteMinigameRound(int round, int score, float flowScore)
        {
            var flowDataMinigame = new FlowDataMinigame
            {
                MinigameRound = round,
                RoundScore = score,
                RoundFlowScore = flowScore,
            };

            flowDataMinigame.FlowDataDevices.Add(new FlowDataDevice
            {
                DeviceName = GameDevice.Pitaco.GetDescription(),
                FlowData = new List<FlowData>(_pitacoLogger.flowDataDevice.FlowData)
            });

            _minigameOverviewSendDto.FlowDataRound.Add(new FlowDataMinigame
            {
                MinigameRound = round,
                RoundScore = score,
                RoundFlowScore = flowScore,
            });

            _pitacoLogger.flowDataDevice.FlowData.Clear();
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