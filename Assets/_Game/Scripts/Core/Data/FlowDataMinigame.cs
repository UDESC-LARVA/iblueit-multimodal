using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ibit.Core.Data
{
    public class FlowDataMinigame
    {
        [JsonProperty("minigameRound")] public int MinigameRound { get; set; }
        [JsonProperty("roundScore")] public float RoundScore { get; set; }
        [JsonProperty("roundFlowScore")] public float RoundFlowScore { get; set; }
        [JsonProperty("flowDataDevices")] public List<FlowDataDevice> FlowDataDevices { get; set; } = new List<FlowDataDevice>();

    }
}