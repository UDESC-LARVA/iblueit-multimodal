using System;

namespace Ibit.Core.Data
{
    [Serializable]
    public class Pacient
    {
        public static Pacient Loaded;

        public int Id;
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
                    ExpPeakFlow = 60, //203.85 L/min
                    InsPeakFlow = -100,  //-135.90 L/min
                    ExpFlowDuration = 18000,   //18 segundos
                    InsFlowDuration = 10000   //10 segundos
                    },

                    CapacitiesMano = new Capacities
                    {
                    ExpPeakFlow = 8451, //86.17 cmH2O
                    InsPeakFlow = -7537,  //-76.85 cmH2O
                    ExpFlowDuration = 18000,   //18 segundos
                    InsFlowDuration = 10000   //10 segundos
                    },

                    CapacitiesCinta = new Capacities
                    {
                    RespiratoryRate = 0.3f,
                    ExpPeakFlow = 700, //valor original 1600
                    InsPeakFlow = -500,  //valor original -330
                    ExpFlowDuration = 18000,   //valor original
                    InsFlowDuration = 10000   //valor original
                    }
                };
        }
#endif

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