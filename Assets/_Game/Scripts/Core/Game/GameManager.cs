using System;
using System.Collections;
using Assets._Game.Scripts.Core.Api;
using Ibit.Core.Data;
using Ibit.Core.Data.Manager;
using Ibit.Core.Util;
using UnityEngine;

namespace Ibit.Core.Game
{
    public partial class GameManager : MonoBehaviour
    {
        private static bool isLoaded;
        public static DateTime GameStart;
        public static float CapacityMultiplierPlataform { get; private set; } = 0.4f;
        public static float CapacityMultiplierMinigames { get; private set; } = 0.4f;
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

            CapacityMultiplierPlataform = Parsers.Float(grid[1][0]);
            CapacityMultiplierMinigames = Parsers.Float(grid[1][1]);
            LevelUnlockScoreThreshold = Mathf.Clamp(Parsers.Float(grid[1][2]), 0.5f, 1f);
            Pitaco.AirViscosity = Parsers.Float(grid[1][3]);
            Pitaco.Lenght = Parsers.Float(grid[1][4]);
            Pitaco.Radius = Parsers.Float(grid[1][5]);
            Mano.AirViscosity = Parsers.Float(grid[1][6]);
            Mano.Lenght = Parsers.Float(grid[1][7]);
            Mano.Radius = Parsers.Float(grid[1][8]);
            Cinta.AirViscosity = Parsers.Float(grid[1][9]);
            Cinta.Lenght = Parsers.Float(grid[1][10]);
            Cinta.Radius = Parsers.Float(grid[1][11]);
        }

        public async void FlushLocalDataToCloudAction()
        {
            var hasInternetConnection = await ApiClient.Instance.HasInternetConnection();
            if (!hasInternetConnection)
            {
                SysMessage.Info("Sem conexão com a internet!");
                return;
            }
            GameObject.Find("Canvas").transform.Find("SendingBgPanel").gameObject.SetActive(true);
            await DataManager.Instance.SendRemoteData();
            StartCoroutine(Delay());
            
        }

        private IEnumerator Delay() // Após enviar dados para a nuvem, aguarda 1 segundo antes de desativar o painel de envio.
        {
            yield return new WaitForSeconds(1.0f);
            GameObject.Find("Canvas").transform.Find("SendingBgPanel").gameObject.SetActive(false);
        }
    }
}