using System;
using Ibit.Core.Data;
using Ibit.Core.Util;
using UnityEngine;

namespace Ibit.Core.Game
{
    public partial class GameManager : MonoBehaviour
    {
        private static bool isLoaded;
        public static DateTime GameStart;
        public static float CapacityMultiplier { get; private set; } = 0.4f;
        public static float LevelUnlockScoreThreshold { get; private set; } = 0.7f;

#if UNITY_EDITOR
        public void QuitGame()
        {
            Debug.Log("Quit Game!");
        }
#else
        public void QuitGame()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
#endif

        private void Awake()
        {
            if (isLoaded)
                return;

            GameStart = DateTime.Now;

            LoadGlobals();

            isLoaded = true;
        }

        private void LoadGlobals()
        {
            var data = FileManager.ReadCsv(Application.streamingAssetsPath + @"/Constants.csv");
            var grid = CsvParser2.Parse(data);

            CapacityMultiplier = Parsers.Float(grid[1][0]);
            LevelUnlockScoreThreshold = Mathf.Clamp(Parsers.Float(grid[1][1]), 0.5f, 1f);
            Pitaco.AirViscosity = Parsers.Float(grid[1][2]);
            Pitaco.Lenght = Parsers.Float(grid[1][3]);
            Pitaco.Radius = Parsers.Float(grid[1][4]);
            Mano.AirViscosity = Parsers.Float(grid[1][5]);
            Mano.Lenght = Parsers.Float(grid[1][6]);
            Mano.Radius = Parsers.Float(grid[1][7]);
            Cinta.AirViscosity = Parsers.Float(grid[1][8]);
            Cinta.Lenght = Parsers.Float(grid[1][9]);
            Cinta.Radius = Parsers.Float(grid[1][10]);
        }
    }
}