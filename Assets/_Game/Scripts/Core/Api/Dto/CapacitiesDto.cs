using Newtonsoft.Json;

namespace Assets._Game.Scripts.Core.Api.Dto
{
    public class CapacitiesDto
    {
        [JsonProperty("insPeakFlow")] public float InsPeakFlow { get; set; }

        [JsonProperty("expPeakFlow")] public float ExpPeakFlow { get; set; }

        [JsonProperty("insFlowDuration")] public float InsFlowDuration { get; set; }

        [JsonProperty("expFlowDuration")] public float ExpFlowDuration { get; set; }

        [JsonProperty("respiratoryRate")] public float RespiratoryRate { get; set; }
    }
}