using System.Collections.Generic;
using Ibit.Core.Data;
using Newtonsoft.Json;

namespace Assets._Game.Scripts.Core.Api.Dto
{
    public class MinigameOverviewSendDto
    {
        [JsonProperty("pacientId")] public string PacientId { get; set; }
        [JsonProperty("minigameName")] public string MinigameName { get; set; }
        [JsonProperty("respiratoryExercise")] public string Exercise { get; set; }
        [JsonProperty("flowDataRounds")] public List<FlowDataMinigame> FlowDataRound { get; set; }
    }
}