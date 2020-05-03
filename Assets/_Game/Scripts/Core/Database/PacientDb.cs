using System.Collections.Generic;
using System.Threading.Tasks;
using Assets._Game.Scripts.Core.Api;
using Assets._Game.Scripts.Core.Api.Dto;

namespace Ibit.Core.Database
{
    public class PacientDb
    {
        public static PacientDb Instance = new PacientDb();

        public PacientDb()
        {
            
        }
        //private PacientDb()
        //{
        //    Instance = this;
        //    PacientList = new List<PacientDto>();
        //    LoadFromApi();
        //}

        //public List<PacientDto> PacientList { get; set; }

        //public async Task LoadFromApi()
        //{
        //    PacientList.Clear();

        //    var pacients = await ApiClient.Instance.GetPacients();
        //    PacientList = pacients;
        //}

        public void Save()
        {
            //var items = new[]
            //{
            //    "Id", // 0
            //    "Name", // 1
            //    "Birthday", // 2
            //    "Observations", // 3
            //    "Condition", // 4
            //    "PitacoInsPeakFlow", // 5
            //    "PitacoExpPeakFlow", // 6
            //    "PitacoInsFlowDuration", // 7
            //    "PitacoExpFlowDuration", // 8
            //    "PitacoRespiratoryRate", // 9
            //    "ManoInsPeakFlow", // 10
            //    "ManoExpPeakFlow", // 11
            //    "ManoInsFlowDuration", // 12
            //    "ManoExpFlowDuration", // 13
            //    "CintaInsPeakFlow", // 14
            //    "CintaExpPeakFlow", // 15
            //    "CintaInsFlowDuration", // 16
            //    "CintaExpFlowDuration", // 17
            //    "CintaRespiratoryRate", // 18
            //    "UnlockedLevels", // 19
            //    "AccumulatedScore", // 20
            //    "PlaySessionsDone", // 21
            //    "CalibrationPitacoDone", // 22
            //    "CalibrationManoDone", // 23
            //    "CalibrationCintaDone", // 24
            //    "HowToPlayDone", // 25
            //    "Ethnicity", // 26
            //    "Height", // 27
            //    "Weight", // 28
            //    "PitacoThreshold", // 29
            //    "ManoThreshold", // 30
            //    "CintaThreshold", // 31
            //    "Sex", // 32
            //    "CreatedOn" // 33
            //};

            //var sb = new StringBuilder();
            //sb.AppendLine(items.Aggregate((a, b) => a + ";" + b));

            //for (var i = 0; i < PacientList.Count; i++)
            //{
            //    var pacient = GetAt(i);

            //    sb.AppendLine(
            //        $"{pacient.Id};{pacient.Name};{pacient.Birthday:dd/MM/yyyy};{pacient.Observations};{pacient.Condition};" +
            //        $"{pacient.CapacitiesPitaco.RawInsPeakFlow};{pacient.CapacitiesPitaco.RawExpPeakFlow};{pacient.CapacitiesPitaco.RawInsFlowDuration};{pacient.CapacitiesPitaco.RawExpFlowDuration};{pacient.CapacitiesPitaco.RawRespRate};" +
            //        $"{pacient.CapacitiesMano.RawInsPeakFlow};{pacient.CapacitiesMano.RawExpPeakFlow};{pacient.CapacitiesMano.RawInsFlowDuration};{pacient.CapacitiesMano.RawExpFlowDuration};" +
            //        $"{pacient.CapacitiesCinta.RawInsPeakFlow};{pacient.CapacitiesCinta.RawExpPeakFlow};{pacient.CapacitiesCinta.RawInsFlowDuration};{pacient.CapacitiesCinta.RawExpFlowDuration};{pacient.CapacitiesCinta.RawRespRate};" +
            //        $"{pacient.UnlockedLevels};{pacient.AccumulatedScore};{pacient.PlaySessionsDone};{pacient.CalibrationPitacoDone};{pacient.CalibrationManoDone};{pacient.CalibrationCintaDone};{pacient.HowToPlayDone};" +
            //        $"{pacient.Ethnicity};{pacient.Height};{pacient.Weight};{pacient.PitacoThreshold};{pacient.ManoThreshold};{pacient.CintaThreshold};{pacient.Sex};{pacient.CreatedOn};"
            //    );
            //}

            //FileManager.WriteAllText(filePath, sb.ToString());
        }
    }
}