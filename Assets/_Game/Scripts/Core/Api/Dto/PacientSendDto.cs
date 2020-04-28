using System;
using Newtonsoft.Json;

namespace Assets._Game.Scripts.Core.Api.Dto
{
    public class PacientSendDto
    {
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("birthday")] public DateTime Birthday { get; set; }
        [JsonProperty("sex")] public string Sex { get; set; }
        [JsonProperty("capacitiesPitaco")] public CapacitiesDto CapacitiesPitaco;
        [JsonProperty("capacitiesMano")] public CapacitiesDto CapacitiesMano;
        [JsonProperty("capacitiesCinta")] public CapacitiesDto CapacitiesCinta;
        [JsonProperty("observations")] public string Observations { get; set; }
        [JsonProperty("condition")] public string Condition { get; set; }
        [JsonProperty("unlockedLevels")] public int UnlockedLevels { get; set; }
        [JsonProperty("accumulatedScore")] public float AccumulatedScore { get; set; }
        [JsonProperty("playSessionsDone")] public int PlaySessionsDone { get; set; }
        [JsonProperty("calibrationPitacoDone")] public bool CalibrationPitacoDone { get; set; }
        [JsonProperty("calibrationManoDone")] public bool CalibrationManoDone { get; set; }
        [JsonProperty("calibrationCintaDone")] public bool CalibrationCintaDone { get; set; }
        [JsonProperty("howToPlayDone")] public bool HowToPlayDone { get; set; }
        [JsonProperty("weigth")] public float Weight { get; set; }
        [JsonProperty("heigth")] public float Height { get; set; }
        [JsonProperty("pitacoThreshold")] public float PitacoThreshold { get; set; }
        [JsonProperty("manoThreshold")] public float ManoThreshold { get; set; }
        [JsonProperty("cintaThreshold")] public float CintaThreshold { get; set; }
        [JsonProperty("ethnicity")] public string Ethnicity { get; set; }
    }
}