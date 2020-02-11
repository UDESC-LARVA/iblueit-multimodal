using System.IO;
using Ibit.Core.Data;
using Ibit.Core.Database;
using Ibit.Core.Game;
using Ibit.Core.Util;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Score;
using Ibit.Plataform.Manager.Spawn;
using Ibit.Plataform.Manager.Stage;
using UnityEngine;

namespace Ibit.Plataform.Logger
{
    public class PlataformLogger : Logger<PlataformLogger>
    {
        private Player plr;
        private Scorer scr;
        private Spawner spwn;

        protected override void Awake()
        {
            sb.AppendLine("time;tag;instanceId;posX;posY");
            plr = FindObjectOfType<Player>();
            spwn = FindObjectOfType<Spawner>();
            scr = FindObjectOfType<Scorer>();
            FindObjectOfType<StageManager>().OnStageEnd += StopLogging;
        }

        protected override void Save()
        {
            var path = @"savedata/pacients/" + Pacient.Loaded.Id + @"/" + $"{recordStart:yyyyMMdd-HHmmss}_" + FileName + ".csv";
            FileManager.WriteAllText(path, sb.ToString());
            LogPlaySession();
        }

        private void LogPlaySession()
        {
            if (StageModel.Loaded.Id == Pacient.Loaded.UnlockedLevels)
            {
                if (scr.Result == GameResult.Success)
                {

                    Pacient.Loaded.UnlockedLevels++;
                }
                else
                {
                    if (scr.Score < scr.MaxScore * 0.3f)
                        Pacient.Loaded.UnlockedLevels--;

                    if (Pacient.Loaded.UnlockedLevels <= 0)
                        Pacient.Loaded.UnlockedLevels = 1;
                }
            }

            Pacient.Loaded.PlaySessionsDone++;
            Pacient.Loaded.AccumulatedScore += scr.Score;
            PacientDb.Instance?.Save();

            var historyPath = @"savedata/pacients/" + Pacient.Loaded.Id + @"/Plataform-History.csv";

            var data = $"{recordStart};{recordStop};{FindObjectOfType<StageManager>().Duration};{scr.Result};" +
                $"{StageModel.Loaded.Id};{StageModel.Loaded.Phase};{StageModel.Loaded.Level};{spwn.RelaxTimeSpawned};" +
                $"{scr.Score};{scr.MaxScore};{scr.Score / scr.MaxScore};" +
                $"{spwn.TargetsSucceeded + spwn.TargetsFailed};{spwn.TargetsSucceeded};{spwn.TargetsInsSucceeded};{spwn.TargetsExpSucceeded};{spwn.TargetsFailed};{spwn.TargetsInsFailed};{spwn.TargetsExpFailed};" +
                $"{spwn.ObstaclesSucceeded + spwn.ObstaclesFailed};{spwn.ObstaclesSucceeded};{spwn.ObstaclesInsSucceeded};{spwn.ObstaclesExpSucceeded};{spwn.ObstaclesFailed};{spwn.ObstaclesInsFailed};{spwn.ObstaclesExpFailed};" +
                $"{plr.HeartPoins};";

            sb.Clear();

            if (!File.Exists(historyPath))
            {
                sb.AppendLine($"Pacient: {Pacient.Loaded.Id}");
                sb.AppendLine($"Name: {Pacient.Loaded.Name}");
                sb.AppendLine($"Condition: {Pacient.Loaded.Condition}");
                sb.AppendLine();
                sb.AppendLine("playStart;playFinish;duration;result;" +
                    "stageId;phase;level;relaxTimeSpawned;" +
                    "score;maxScore;scoreRatio;" +
                    "targetsSpawned;targetsSuccess;targetsInsSuccess;targetsExpSuccess;targetsFail;targetsInsFail;targetsExpFail;" +
                    "obstaclesSpawned;obstaclesSuccess;obstaclesInsSuccess;obstaclesExpSuccess;obstaclesFail;obstaclesInsFail;obstaclesExpFail;" +
                    "playerHP;");

                sb.AppendLine(data);
                FileManager.WriteAllText(historyPath, sb.ToString());
            }
            else
            {
                sb.AppendLine(data);
                FileManager.AppendAllText(historyPath, sb.ToString());
            }

            GameObject.Find("Canvas").transform.Find("Result Panel").gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!isLogging || GameManager.GameIsPaused)
                return;

            sb.AppendLine($"{Time.time};{plr.tag};{plr.GetInstanceID()};{plr.transform.position.x:F};{plr.transform.position.y:F}");

            foreach (var o in spwn.SpawnedObjects)
            {
                if (o != null)
                    sb.AppendLine($"{Time.time};{o.tag};{o.GetInstanceID()};{o.position.x:F};{o.position.y:F}");
            }
        }
    }
}