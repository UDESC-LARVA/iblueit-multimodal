using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ibit.Core.Util;
using Ibit.Plataform.Data;
using UnityEngine;

namespace Ibit.Core.Database
{
    public class StageDb
    {
        public static StageDb Instance = new StageDb();

        private static readonly string _stagesPath = Application.streamingAssetsPath + @"/Stages/";

        private StageDb()
        {
            Instance = this;
            StageList = new List<StageModel>();
        }

        /// <summary>
        /// A list with all the stages avaiable.
        /// </summary>
        public List<StageModel> StageList { get; }

        /// <summary>
        /// Gets stage data from stage list.
        /// </summary>
        /// <param name="id">Stage Id</param>
        /// <returns></returns>
        public StageModel GetStage(int id) => StageList.Find(x => x.Id == id);

        /// <summary>
        /// Load stages from csv files.
        /// </summary>
        public void LoadStages()
        {
            StageList.Clear();

            var files = Directory.GetFiles(_stagesPath);

            foreach (var path in files)
            {
                var file = new FileInfo(path);

                if (!file.Name.StartsWith("F") || !file.Name.EndsWith(".csv"))
                    continue;

                var model = LoadStageFromFile(file.Name);

                if (StageList.Any(m => m.Id == model.Id))
                    throw new Exception($"Stage ID {model.Id} already exists!");

                StageList.Add(model);
            }

            Debug.Log($"{StageList.Count} stages loaded.");
        }

        /// <summary>
        /// Loads stage from a csv file.
        /// </summary>
        /// <param name="filename">File to be loaded.</param>
        /// <returns></returns>
        public static StageModel LoadStageFromFile(string filename)
        {
            var path = _stagesPath + filename;

            if (!File.Exists(path))
                throw new FileNotFoundException($"Stage file '{path}' not found!");

            var data = FileManager.ReadAllLines(path);

            CleanData(ref data, path);

            var stageHeader = $"{data[0]}\n{data[1]}";
            var grid = CsvParser2.Parse(stageHeader);

            var stageInfo = new StageModel
            {
                //header
                Id = int.Parse(grid[1][0]),
                Phase = int.Parse(grid[1][1]),
                Level = int.Parse(grid[1][2]),
                ObjectSpeedFactor = Parsers.Float(grid[1][3]),
                HeightIncrement = Parsers.Float(grid[1][4]),
                HeightUpThreshold = int.Parse(grid[1][5]),
                HeightDownThreshold = int.Parse(grid[1][6]),
                SizeIncrement = Parsers.Float(grid[1][7]),
                SizeUpThreshold = int.Parse(grid[1][8]),
                SizeDownThreshold = int.Parse(grid[1][9]),
                Loops = int.Parse(grid[1][10]),
            };

            stageInfo.Loops = stageInfo.Loops == 0 ? 1 : stageInfo.Loops;

            var template = "ObjectType;DifficultyFactor;PositionYFactor;PositionXSpacing";

            // Read each line from the file to get objects to spawn
            for (int i = 2; i < data.Length; i++)
            {
                if (data[i].StartsWith("%") || string.IsNullOrEmpty(data[i]))
                    continue;

                var modelGrid = CsvParser2.Parse($"{template}\n{data[i]}");

                var model = new ObjectModel
                {
                    Id = i,
                    Type = (StageObjectType)Enum.Parse(typeof(StageObjectType), (modelGrid[1][0])),
                    DifficultyFactor = Parsers.Float(modelGrid[1][1]),
                    PositionYFactor = Parsers.Float(modelGrid[1][2]),
                    PositionXSpacing = Parsers.Float(modelGrid[1][3]),
                };

                stageInfo.ObjectModels.Add(model);
            }

            return stageInfo;
        }

        /// <summary>
        /// Regex Patterns to be cleaned on CleanData() call.
        /// </summary>
        private static readonly string[] _patternsToBeCleaned = {
            ";;"
        };

        /// <summary>
        /// Cleans all excel unnecessary character pattern in the file.
        /// </summary>
        /// <param name="data">Array data from the stage file.</param>
        /// <param name="path">Stage file path</param>
        private static void CleanData(ref string[] data, string path)
        {
            bool hasTrash = false;

            for (int i = 0; i < data.Length; i++)
            {
                foreach (var pattern in _patternsToBeCleaned)
                {
                    if (Regex.IsMatch(data[i], pattern))
                    {
                        data[i] = Regex.Replace(data[i], pattern, "");
                        hasTrash = true;
                    }
                }
            }

            if (hasTrash)
                FileManager.WriteAllLines(path, data);
        }
    }
}