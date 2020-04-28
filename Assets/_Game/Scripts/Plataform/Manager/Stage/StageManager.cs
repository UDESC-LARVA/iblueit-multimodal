using Ibit.Core.Game;
using Ibit.Core.Serial;
using Ibit.Core.Util;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Score;
using Ibit.Plataform.Manager.Spawn;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace Ibit.Plataform.Manager.Stage
{
    public partial class StageManager : MonoBehaviour
    {
        #region Events

        public event Action OnStageStart;
        public event Action OnStageEnd;

        #endregion Events

        #region Properties

        public bool IsRunning { get; private set; }
        public float Duration { get; private set; }

        #endregion Properties

        private Spawner spawner;


        [SerializeField]
        private SerialControllerPitaco serialControllerPitaco;

        [SerializeField]
        private SerialControllerMano serialControllerMano;

        [SerializeField]
        private SerialControllerCinta serialControllerCinta;





        private void Awake()
        {

#if UNITY_EDITOR

            if (StageModel.Loaded == null)
                StageModel.Loaded = testStage;
            else
                testStage = StageModel.Loaded;
#endif

            spawner = FindObjectOfType<Spawner>();

            serialControllerPitaco = FindObjectOfType<SerialControllerPitaco>();
            serialControllerPitaco.OnSerialConnected += StartStage;

            serialControllerMano = FindObjectOfType<SerialControllerMano>();
            serialControllerMano.OnSerialConnected += StartStage;

            serialControllerCinta = FindObjectOfType<SerialControllerCinta>();
            serialControllerCinta.OnSerialConnected += StartStage;

#if !UNITY_EDITOR

        // Caso algum dispositivo de controle seja disconectado.
        serialControllerPitaco.OnSerialDisconnected += PauseOnDisconnect;
        serialControllerMano.OnSerialDisconnected += PauseOnDisconnect;
        serialControllerCinta.OnSerialDisconnected += PauseOnDisconnect;
#endif

            FindObjectOfType<Player>().OnPlayerDeath += GameOver;

            Time.timeScale = 1f;
        }

        [Button("Start Stage")]
        private void StartStage()
        {


            if (IsRunning)
            {
                if (GameManager.GameIsPaused)
                    GameManager.UnPauseGame();
                // else
                //     return;
            }


            // Conectando/reconectando dispositivos "OnTheFly", ou seja, em tempo de execução.
            Debug.Log("CONECTANDO/RECONECTANDO DISPOSITIVOS...");

            serialControllerPitaco.Connect();
            serialControllerMano.Connect();
            serialControllerCinta.Connect();

            serialControllerPitaco.StartSamplingDelayed();
            serialControllerMano.StartSamplingDelayed();
            serialControllerCinta.StartSamplingDelayed();      

            IsRunning = true;
            OnStageStart?.Invoke();
        }

#if !UNITY_EDITOR
    private void PauseOnDisconnect() // Executada assim que um dispositivo é desconectado
    {
        if (GameManager.GameIsPaused)
            return;


        // Caso todos os dispositivos de controle não estejam conectados.
        if (!serialControllerPitaco.IsConnected && !serialControllerMano.IsConnected && !serialControllerCinta.IsConnected)
        {
            // Debug.Log("Nenhum dispositivo de controle conectado.\nReconecte ao menos um controle.");
            FindObjectOfType<Ibit.Plataform.UI.CanvasManager>().PauseGame();
            SysMessage.Warning("Nenhum dispositivo de controle conectado.\nReconecte ao menos um controle.");
        }

        
    }
#endif

        [Button("End Stage")]
        private void EndStage()
        {
            GameOver();
        }

        private void GameOver()
        {
            IsRunning = false;
            FindObjectOfType<Scorer>().CalculateResult(FindObjectOfType<Player>().HeartPoins < 1);
            
            FindObjectOfType<SerialControllerPitaco>().StopSampling();
            FindObjectOfType<PitacoLogger>().StopLogging();

            FindObjectOfType<SerialControllerMano>().StopSampling();
            FindObjectOfType<ManoLogger>().StopLogging();

            FindObjectOfType<SerialControllerCinta>().StopSampling();
            FindObjectOfType<CintaLogger>().StopLogging();
            
            OnStageEnd?.Invoke();
        }

        private void Update()
        {
            if (!IsRunning)
                return;

            Duration += Time.deltaTime;

            if (spawner.ObjectsOnScene < 1)
                EndStage();
        }
    }
}