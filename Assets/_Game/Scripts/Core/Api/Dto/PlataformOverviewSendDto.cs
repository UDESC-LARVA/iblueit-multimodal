using System;
using System.Collections.Generic;
using Ibit.Core.Data;
using Ibit.Plataform.Manager.Score;
using Newtonsoft.Json;

namespace Assets._Game.Scripts.Core.Api.Dto
{
    public class PlataformOverviewSendDto
    {
        [JsonProperty("pacientId")] public string PacientId { get; set; }
        [JsonProperty("flowDataDevices")] public List<FlowDataDevice> FlowDataDevices { get; set; }
        [JsonProperty("playStart")] public DateTime PlayStart { get; set; }
        [JsonProperty("playFinish")] public DateTime PlayFinish { get; set; }
        [JsonProperty("duration")] public float Duration { get; set; }
        [JsonProperty("result")] public GameResult Result { get; set; }
        [JsonProperty("stageId")] public int StageId { get; set; }
        [JsonProperty("phase")] public int Phase { get; set; }
        [JsonProperty("level")] public int Level { get; set; }
        [JsonProperty("relaxTimeSpawned")] public bool RelaxTimeSpawned { get; set; }
        [JsonProperty("maxScore")] public float MaxScore { get; set; }
        [JsonProperty("scoreRatio")] public float ScoreRatio { get; set; }
        [JsonProperty("targetsSpawned")] public int TargetsSpawned { get; set; }
        [JsonProperty("targetsSuccess")] public int TargetsSuccess { get; set; }
        [JsonProperty("TargetsInsSuccess")] public int TargetsInsSuccess { get; set; }
        [JsonProperty("TargetsExpSuccess")] public int TargetsExpSuccess { get; set; }
        [JsonProperty("TargetsFails")] public int TargetsFails { get; set; }
        [JsonProperty("TargetsInsFail")] public int TargetsInsFail { get; set; }
        [JsonProperty("TargetsExpFail")] public int TargetsExpFail { get; set; }
        [JsonProperty("ObstaclesSpawned")] public int ObstaclesSpawned { get; set; }
        [JsonProperty("ObstaclesSuccess")] public int ObstaclesSuccess { get; set; }
        [JsonProperty("ObstaclesFail")] public int ObstaclesFail { get; set; }
        [JsonProperty("ObstaclesInsSuccess")] public int ObstaclesInsSuccess { get; set; }
        [JsonProperty("ObstaclesExpSuccess")] public int ObstaclesExpSuccess { get; set; }
        [JsonProperty("ObstaclesInsFail")] public int ObstaclesInsFail { get; set; }
        [JsonProperty("ObstaclesExpFail")] public int ObstaclesExpFail { get; set; }
        [JsonProperty("PlayerHp")] public float PlayerHp { get; set; }
        [JsonProperty("Score")] public float Score { get; set; }
    }
}