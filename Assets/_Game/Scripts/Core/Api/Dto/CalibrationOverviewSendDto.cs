using Newtonsoft.Json;

namespace Assets._Game.Scripts.Core.Api.Dto
{
    public class CalibrationOverviewSendDto
    {
        [JsonProperty("pacientId")] public string PacientId { get; set; }
        [JsonProperty("gameDevice")] public string GameDevice { get; set; }
        [JsonProperty("calibrationValue")] public float CalibrationValue { get; set; }
        [JsonProperty("calibrationExercise")] public string Exercise { get; set; }
    }
}