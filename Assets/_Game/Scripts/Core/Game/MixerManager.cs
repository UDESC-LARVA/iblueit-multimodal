using System;
using System.Collections;
using Assets._Game.Scripts.Core.Api;
using Ibit.Core.Data;
using Ibit.Core.Data.Manager;
using Ibit.Core.Util;
using Ibit.Core.Database;
using Ibit.Plataform;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Serial;
//using MixerFusion;

namespace Ibit.Core.Game
{
    public partial class MixerManager : MonoBehaviour
    {
        public Parameters CurrentParameters;

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;
        private SerialControllerOximetro sco;

        void Start()
        {
            if (scp == null)
                scp = FindObjectOfType<SerialControllerPitaco>();
    
            if (scm == null)
                scm = FindObjectOfType<SerialControllerMano>();

            if (scc == null)
                scc = FindObjectOfType<SerialControllerCinta>();
            
            if (sco == null)
                sco = FindObjectOfType<SerialControllerOximetro>();
        }

        void Update()
        {
            CurrentParameters = ParametersDb.parameters;

            if (scp.IsConnected || scm.IsConnected || scc.IsConnected) // Só executa caso algum dispositivo de controle esteja conectado
            {
                FusionManager(Player.sensorValuePitaco, Player.sensorValueMano, Player.sensorValueCinta, Player.sensorValueOxiSPO, Player.sensorValueOxiHR);
                AdaptationGrid(Player.sensorValuePitaco, Player.sensorValueMano, Player.sensorValueCinta, Player.sensorValueOxiSPO, Player.sensorValueOxiHR);
                
                // Debug.Log($"Pitaco: {Player.sensorValuePitaco}");
                // Debug.Log($"Mano: {Player.sensorValueMano}");
                // Debug.Log($"Cinta: {Player.sensorValueCinta}");
                // Debug.Log($"OxímetroManager: {Player.sensorValueOxiSPO}");

                // FusionManager();
                // Debug.Log("Passei FusionManager");
                // AdaptationGrid();
                // Debug.Log("Passei AdaptationGrid");
                // FissionManager();
                // Debug.Log("Passei FissionManager");
            }
        }
    }
}