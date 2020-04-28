using System;
using Assets._Game.Scripts.Core.Api.Dto;
using Assets._Game.Scripts.Core.Api.Extensions;

namespace Ibit.Core.Data
{
    [Serializable]
    public class Pacient
    {
        public static Pacient Loaded;

        public int Id;
        public string IdApi;
        public string Name;
        public Sex Sex;
        public DateTime Birthday;
        public Capacities CapacitiesPitaco;
        public Capacities CapacitiesMano;
        public Capacities CapacitiesCinta;

        public string Observations;
        public ConditionType Condition;
        public int UnlockedLevels;
        public float AccumulatedScore;
        public int PlaySessionsDone;
        public bool CalibrationPitacoDone;
        public bool CalibrationManoDone;
        public bool CalibrationCintaDone;
        public bool HowToPlayDone;
        public float Weight;
        public float Height;
        public float PitacoThreshold;
        public float ManoThreshold;
        public float CintaThreshold;
        public string Ethnicity;
        public DateTime CreatedOn;

        public bool IsCalibrationPitacoDone => this.CapacitiesPitaco.RawInsPeakFlow < 0 &&
            this.CapacitiesPitaco.RawInsFlowDuration > 0 &&
            this.CapacitiesPitaco.RawExpPeakFlow > 0 &&
            this.CapacitiesPitaco.RawExpFlowDuration > 0 &&
            this.CapacitiesPitaco.RawRespRate > 0;

        public bool IsCalibrationManoDone => this.CapacitiesMano.RawInsPeakFlow < 0 &&
            this.CapacitiesMano.RawInsFlowDuration > 0 &&
            this.CapacitiesMano.RawExpPeakFlow > 0 &&
            this.CapacitiesMano.RawExpFlowDuration > 0;

        public bool IsCalibrationCintaDone => this.CapacitiesCinta.RawInsPeakFlow < 0 &&
            this.CapacitiesCinta.RawInsFlowDuration > 0 &&
            this.CapacitiesCinta.RawExpPeakFlow > 0 &&
            this.CapacitiesCinta.RawExpFlowDuration > 0 &&
            this.CapacitiesCinta.RawRespRate > 0;

#if UNITY_EDITOR
        static Pacient()
        {
            if (Loaded == null)
                Loaded = new Pacient
                {
                    IdApi = "00000000-0000-0000-0000-000000000000",
                    Id = -1,
                    CalibrationPitacoDone = true,
                    CalibrationManoDone = true,
                    CalibrationCintaDone = true,
                    HowToPlayDone = true,
                    Condition = ConditionType.Healthy,
                    Name = "NetRunner",
                    PlaySessionsDone = 0,
                    UnlockedLevels = 15,
                    AccumulatedScore = 0,
                    PitacoThreshold = 7.5f,
                    ManoThreshold = 7.5f,
                    CintaThreshold = 7.5f,

                    CapacitiesPitaco = new Capacities
                    {
                        RespiratoryRate = 0.3f,
                        ExpPeakFlow = 200, //valor original 1600
                        InsPeakFlow = -80,  //valor original -330
                        ExpFlowDuration = 18000,   //valor original
                        InsFlowDuration = 10000   //valor original
                    },

                    CapacitiesMano = new Capacities
                    {
                        ExpPeakFlow = 400, //valor original 1600
                        InsPeakFlow = -200,  //valor original -330
                        ExpFlowDuration = 18000,   //valor original
                        InsFlowDuration = 10000   //valor original
                    },

                    CapacitiesCinta = new Capacities
                    {
                        RespiratoryRate = 0.3f,
                        ExpPeakFlow = 200, //valor original 1600
                        InsPeakFlow = -80,  //valor original -330
                        ExpFlowDuration = 18000,   //valor original
                        InsFlowDuration = 10000   //valor original
                    }
                };
        }
#endif

        public static Pacient MapFromDto(PacientDto pacientDto)
        {
            return new Pacient
            {
                IdApi = pacientDto.Id,
                CalibrationPitacoDone = pacientDto.CalibrationPitacoDone,
                CalibrationManoDone = pacientDto.CalibrationManoDone,
                CalibrationCintaDone = pacientDto.CalibrationCintaDone,
                HowToPlayDone = pacientDto.HowToPlayDone,
                Condition = EnumExtensions.GetValueFromDescription<ConditionType>(pacientDto.Condition),
                Name = pacientDto.Name,
                PlaySessionsDone = pacientDto.PlaySessionsDone,
                UnlockedLevels = pacientDto.UnlockedLevels,
                AccumulatedScore = pacientDto.AccumulatedScore,
                PitacoThreshold = pacientDto.PitacoThreshold,
                ManoThreshold = pacientDto.ManoThreshold,
                CintaThreshold = pacientDto.CintaThreshold,

                CapacitiesPitaco = new Capacities
                {
                    RespiratoryRate = pacientDto.CapacitiesPitaco.RespiratoryRate,
                    ExpPeakFlow = pacientDto.CapacitiesPitaco.ExpPeakFlow, //valor original 1600
                    InsPeakFlow = pacientDto.CapacitiesPitaco.InsPeakFlow,  //valor original -330
                    ExpFlowDuration = pacientDto.CapacitiesPitaco.ExpFlowDuration,   //valor original
                    InsFlowDuration = pacientDto.CapacitiesPitaco.InsFlowDuration   //valor original
                },

                CapacitiesMano = new Capacities
                {
                    RespiratoryRate = pacientDto.CapacitiesMano.RespiratoryRate,
                    ExpPeakFlow = pacientDto.CapacitiesMano.ExpPeakFlow, //valor original 1600
                    InsPeakFlow = pacientDto.CapacitiesMano.InsPeakFlow,  //valor original -330
                    ExpFlowDuration = pacientDto.CapacitiesMano.ExpFlowDuration,   //valor original
                    InsFlowDuration = pacientDto.CapacitiesMano.InsFlowDuration   //valor original
                },

                CapacitiesCinta = new Capacities
                {
                    RespiratoryRate = pacientDto.CapacitiesCinta.RespiratoryRate,
                    ExpPeakFlow = pacientDto.CapacitiesCinta.ExpPeakFlow, //valor original 1600
                    InsPeakFlow = pacientDto.CapacitiesCinta.InsPeakFlow,  //valor original -330
                    ExpFlowDuration = pacientDto.CapacitiesCinta.ExpFlowDuration,   //valor original
                    InsFlowDuration = pacientDto.CapacitiesCinta.InsFlowDuration   //valor original
                },
                Sex = EnumExtensions.GetValueFromDescription<Sex>(pacientDto.Sex),
                Birthday = pacientDto.Birthday,
                Weight = pacientDto.Weight,
                Ethnicity = pacientDto.Ethnicity,
                Height = pacientDto.Height,
                Observations = pacientDto.Observations
            };
        }

        public static PacientSendDto MapToPacientSendDto()
        {
            return new PacientSendDto
            {
                Name = Loaded.Name,
                Sex = Loaded.Sex.GetDescription(),
                Birthday = Loaded.Birthday,
                Weight = Loaded.Weight,
                Height = Loaded.Height,
                Ethnicity = Loaded.Ethnicity,
                AccumulatedScore = Loaded.AccumulatedScore,
                UnlockedLevels = Loaded.UnlockedLevels,
                CapacitiesPitaco = new CapacitiesDto
                {
                    InsPeakFlow = Loaded.CapacitiesPitaco.InsPeakFlow,
                    ExpPeakFlow = Loaded.CapacitiesPitaco.ExpPeakFlow,
                    RespiratoryRate = Loaded.CapacitiesPitaco.RespiratoryRate,
                    ExpFlowDuration = Loaded.CapacitiesPitaco.ExpFlowDuration,
                    InsFlowDuration = Loaded.CapacitiesPitaco.InsFlowDuration
                },
                CapacitiesCinta = new CapacitiesDto
                {
                    InsPeakFlow = Loaded.CapacitiesCinta.InsPeakFlow,
                    ExpPeakFlow = Loaded.CapacitiesCinta.ExpPeakFlow,
                    RespiratoryRate = Loaded.CapacitiesCinta.RespiratoryRate,
                    ExpFlowDuration = Loaded.CapacitiesCinta.ExpFlowDuration,
                    InsFlowDuration = Loaded.CapacitiesCinta.InsFlowDuration
                },
                CapacitiesMano = new CapacitiesDto
                {
                    InsPeakFlow = Loaded.CapacitiesMano.InsPeakFlow,
                    ExpPeakFlow = Loaded.CapacitiesMano.ExpPeakFlow,
                    RespiratoryRate = Loaded.CapacitiesMano.RespiratoryRate,
                    ExpFlowDuration = Loaded.CapacitiesMano.ExpFlowDuration,
                    InsFlowDuration = Loaded.CapacitiesMano.InsFlowDuration
                },
                CalibrationManoDone = Loaded.CalibrationManoDone,
                CalibrationCintaDone = Loaded.CalibrationCintaDone,
                CalibrationPitacoDone = Loaded.CalibrationPitacoDone,
                CintaThreshold = Loaded.CintaThreshold,
                Condition = Loaded.Condition.GetDescription(),
                PlaySessionsDone = Loaded.PlaySessionsDone,
                HowToPlayDone = Loaded.HowToPlayDone,
                Observations = Loaded.Observations,
                PitacoThreshold = Loaded.PitacoThreshold,
                ManoThreshold = Loaded.ManoThreshold
            };
        }
    }

    public enum ConditionType
    {
        Restrictive = 1,
        Healthy = 2,
        Obstructive = 3
    }

    public enum Sex
    {
        Male,
        Female
    }
}