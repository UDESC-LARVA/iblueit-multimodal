using Newtonsoft.Json;

namespace Assets._Game.Scripts.Core.Api.Dto
{
    public class CapacitiesDto
    {
        [JsonProperty("insPeakFlow")] public float RawInsPeakFlow { get; set; }

        [JsonProperty("expPeakFlow")] public float RawExpPeakFlow { get; set; }

        [JsonProperty("insFlowDuration")] public float RawInsFlowDuration { get; set; }

        [JsonProperty("expFlowDuration")] public float RawExpFlowDuration { get; set; }

        [JsonProperty("respiratoryRate")] public float RawRespiratoryRate { get; set; }
    }
}