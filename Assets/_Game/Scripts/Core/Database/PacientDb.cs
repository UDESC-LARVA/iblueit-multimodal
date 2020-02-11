using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Ibit.Core.Data;
using Ibit.Core.Util;

namespace Ibit.Core.Database
{
    public class PacientDb
    {
        public static PacientDb Instance = new PacientDb();

        private readonly string filePath = @"savedata/pacients/_pacientList.csv";

        private PacientDb()
        {
            Instance = this;
            PacientList = new List<Pacient>();
            Load();
        }

        public List<Pacient> PacientList { get; }

        public List<Pacient> ContainsName(string find) => PacientList.FindAll(x => x.Name.Contains(find));

        public void CreatePacient(Pacient plr)
        {
            PacientList.Add(plr);
            Save();
        }

        public Pacient GetAt(int i) => PacientList.Count <= i ? null : PacientList[i];

        public Pacient GetPacient(int id) => PacientList.Find(x => x.Id == id);

        public Pacient GetPacient(string pacientName) => PacientList.Find(x => x.Name == pacientName);

        public void Load()
        {
            if (!File.Exists(filePath))
                return;

            PacientList.Clear();

            var csvData = FileManager.ReadCsv(filePath);
            var grid = CsvParser2.Parse(csvData);

            for (var i = 1; i < grid.Length; i++)
            {
                if (string.IsNullOrEmpty(grid[i][0]))
                    continue;

                var plr = new Pacient
                {
                    Id = int.Parse(grid[i][0]),
                    Name = grid[i][1],
                    Birthday = DateTime.ParseExact(grid[i][2], @"dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Observations = grid[i][3],
                    Condition = (ConditionType)Enum.Parse(typeof(ConditionType), grid[i][4]),
                    CapacitiesPitaco = new Capacities
                    {
                    InsPeakFlow = Parsers.Float(grid[i][5]),
                    ExpPeakFlow = Parsers.Float(grid[i][6]),
                    InsFlowDuration = Parsers.Float(grid[i][7]),
                    ExpFlowDuration = Parsers.Float(grid[i][8]),
                    RespiratoryRate = Parsers.Float(grid[i][9]),
                    },
                    CapacitiesMano = new Capacities
                    {
                    InsPeakFlow = Parsers.Float(grid[i][10]),
                    ExpPeakFlow = Parsers.Float(grid[i][11]),
                    InsFlowDuration = Parsers.Float(grid[i][12]),
                    ExpFlowDuration = Parsers.Float(grid[i][13]),
                    },
                    CapacitiesCinta = new Capacities
                    {
                    InsPeakFlow = Parsers.Float(grid[i][14]),
                    ExpPeakFlow = Parsers.Float(grid[i][15]),
                    InsFlowDuration = Parsers.Float(grid[i][16]),
                    ExpFlowDuration = Parsers.Float(grid[i][17]),
                    RespiratoryRate = Parsers.Float(grid[i][18]),
                    },
                    UnlockedLevels = int.Parse(grid[i][19]),
                    AccumulatedScore = Parsers.Float(grid[i][20]),
                    PlaySessionsDone = int.Parse(grid[i][21]),
                    CalibrationPitacoDone = bool.Parse(grid[i][22]),
                    CalibrationManoDone = bool.Parse(grid[i][23]),
                    CalibrationCintaDone = bool.Parse(grid[i][24]),
                    HowToPlayDone = bool.Parse(grid[i][25]),
                    Ethnicity = grid[i][26],
                    Height = Parsers.Float(grid[i][27]),
                    Weight = Parsers.Float(grid[i][28]),
                    PitacoThreshold = Parsers.Float(grid[i][29]),
                    ManoThreshold = Parsers.Float(grid[i][30]),
                    CintaThreshold = Parsers.Float(grid[i][31]),
                    Sex = (Sex)Enum.Parse(typeof(Sex), grid[i][32]),
                    CreatedOn = DateTime.Parse(grid[i][33])
                };

                PacientList.Add(plr);
            }
        }

        public void Save()
        {
            var items = new []
            {
                "Id", // 0
                "Name", // 1
                "Birthday", // 2
                "Observations", // 3
                "Condition", // 4
                "PitacoInsPeakFlow", // 5
                "PitacoExpPeakFlow", // 6
                "PitacoInsFlowDuration", // 7
                "PitacoExpFlowDuration", // 8
                "PitacoRespiratoryRate", // 9
                "ManoInsPeakFlow", // 10
                "ManoExpPeakFlow", // 11
                "ManoInsFlowDuration", // 12
                "ManoExpFlowDuration", // 13
                "CintaInsPeakFlow", // 14
                "CintaExpPeakFlow", // 15
                "CintaInsFlowDuration", // 16
                "CintaExpFlowDuration", // 17
                "CintaRespiratoryRate", // 18
                "UnlockedLevels", // 19
                "AccumulatedScore", // 20
                "PlaySessionsDone", // 21
                "CalibrationPitacoDone", // 22
                "CalibrationManoDone", // 23
                "CalibrationCintaDone", // 24
                "HowToPlayDone", // 25
                "Ethnicity", // 26
                "Height", // 27
                "Weight", // 28
                "PitacoThreshold", // 29
                "ManoThreshold", // 30
                "CintaThreshold", // 31
                "Sex", // 32
                "CreatedOn" // 33
            };

            var sb = new StringBuilder();
            sb.AppendLine(items.Aggregate((a, b) => a + ";" + b));

            for (var i = 0; i < PacientList.Count; i++)
            {
                var pacient = GetAt(i);

                sb.AppendLine(
                    $"{pacient.Id};{pacient.Name};{pacient.Birthday:dd/MM/yyyy};{pacient.Observations};{pacient.Condition};" +
                    $"{pacient.CapacitiesPitaco.RawInsPeakFlow};{pacient.CapacitiesPitaco.RawExpPeakFlow};{pacient.CapacitiesPitaco.RawInsFlowDuration};{pacient.CapacitiesPitaco.RawExpFlowDuration};{pacient.CapacitiesPitaco.RawRespRate};" +
                    $"{pacient.CapacitiesMano.RawInsPeakFlow};{pacient.CapacitiesMano.RawExpPeakFlow};{pacient.CapacitiesMano.RawInsFlowDuration};{pacient.CapacitiesMano.RawExpFlowDuration};" +
                    $"{pacient.CapacitiesCinta.RawInsPeakFlow};{pacient.CapacitiesCinta.RawExpPeakFlow};{pacient.CapacitiesCinta.RawInsFlowDuration};{pacient.CapacitiesCinta.RawExpFlowDuration};{pacient.CapacitiesCinta.RawRespRate};" +
                    $"{pacient.UnlockedLevels};{pacient.AccumulatedScore};{pacient.PlaySessionsDone};{pacient.CalibrationPitacoDone};{pacient.CalibrationManoDone};{pacient.CalibrationCintaDone};{pacient.HowToPlayDone};" +
                    $"{pacient.Ethnicity};{pacient.Height};{pacient.Weight};{pacient.PitacoThreshold};{pacient.ManoThreshold};{pacient.CintaThreshold};{pacient.Sex};{pacient.CreatedOn};"
                );
            }

            FileManager.WriteAllText(filePath, sb.ToString());
        }
    }
}