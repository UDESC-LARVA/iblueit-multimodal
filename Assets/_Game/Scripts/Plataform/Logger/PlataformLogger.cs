using System.Collections.Generic;
using Assets._Game.Scripts.Core.Api.Dto;
using Ibit.Core.Data;
using Ibit.Core.Data.Manager;
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
        private PitacoLogger pitacoLogger;

        protected override void Awake()
        {
            sb.AppendLine("time;tag;instanceId;posX;posY");
            plr = FindObjectOfType<Player>();
            spwn = FindObjectOfType<Spawner>();
            scr = FindObjectOfType<Scorer>();
            pitacoLogger = FindObjectOfType<PitacoLogger>();
            FindObjectOfType<StageManager>().OnStageEnd += StopLogging;
        }

        protected override void Save()
        {
            var path = @"savedata/pacients/" + Pacient.Loaded.Id + @"/" + $"{recordStart:yyyyMMdd-HHmmss}_" + FileName + ".csv";
            FileManager.WriteAllText(path, sb.ToString());
            LogPlaySession();
        }

        private async void LogPlaySession()
        {

            //TODO: A responsabilidade do logger não é de bloquear e mostrar a tela de salvamento, porém o refatoramento a ser feito é muito grande... Deixa para depois
            GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(true);

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

            var pacientSendDto = Pacient.MapToPacientSendDto();

            var responsePacient = await DataManager.Instance.UpdatePacient(pacientSendDto);
            if (responsePacient.ApiResponse == null)
                SysMessage.Info("Erro ao atualizar o paciente na nuvem!\n Os dados poderão ser enviados posteriormente.");

            Debug.Log($"Deu update no Paciente");

            var plataformOverviewSendDto = new PlataformOverviewSendDto
            {
                Duration = FindObjectOfType<StageManager>().Duration,
                Result = scr.Result,
                StageId = StageModel.Loaded.Id,
                Phase = StageModel.Loaded.Phase,
                Level = StageModel.Loaded.Level,
                RelaxTimeSpawned = spwn.RelaxTimeSpawned,
                Score = scr.Score,
                MaxScore = scr.MaxScore,
                ScoreRatio = scr.Score / scr.MaxScore,
                TargetsSpawned = spwn.TargetsSucceeded + spwn.TargetsFailed,
                TargetsSuccess = spwn.TargetsSucceeded,
                TargetsInsSuccess = spwn.TargetsInsSucceeded,
                TargetsExpSuccess = spwn.TargetsExpSucceeded,
                TargetsFails = spwn.TargetsFailed,
                TargetsInsFail = spwn.TargetsInsFailed,
                TargetsExpFail = spwn.TargetsExpFailed,
                ObstaclesSpawned = spwn.ObstaclesSucceeded + spwn.ObstaclesFailed,
                ObstaclesSuccess = spwn.ObstaclesSucceeded,
                ObstaclesInsSuccess = spwn.ObstaclesInsSucceeded,
                ObstaclesExpSuccess = spwn.ObstaclesExpSucceeded,
                ObstaclesFail = spwn.ObstaclesFailed,
                ObstaclesInsFail = spwn.ObstaclesInsFailed,
                ObstaclesExpFail = spwn.ObstaclesExpFailed,
                PlayerHp = plr.HeartPoins,
                PacientId = Pacient.Loaded.IdApi,
                PlayStart = recordStart,
                PlayFinish = recordStop,
                FlowDataDevices = new List<FlowDataDevice>()
            };

            plataformOverviewSendDto.FlowDataDevices.Add(pitacoLogger.flowDataDevice);

            var plataformResponse = await DataManager.Instance.SavePlataformOverview(plataformOverviewSendDto);
            if (plataformResponse.ApiResponse == null)
                SysMessage.Info("Erro ao salvar dados da plataforma na nuvem!\n Os dados poderão ser enviados posteriormente.");

            GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(false);

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