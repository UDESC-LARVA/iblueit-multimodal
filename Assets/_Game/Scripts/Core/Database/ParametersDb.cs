using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Ibit.Core.Data;
using Ibit.Core.Util;
using Ibit.Core.Serial;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.Core.Database
{
    public class ParametersDb : MonoBehaviour
    {
        public int id;
        public Parameters parameters;
        private readonly string filePath = @"savedata/pacients/_parametersList.csv";

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;

        private void OnEnable()
        {
            parameters = null;
            id = Pacient.Loaded.Id;

            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();

            Load();
        }

        public void Load()
        {
            if (!File.Exists(filePath))
                return;

            var csvData = FileManager.ReadCsv(filePath);
            var grid = CsvParser2.Parse(csvData);
            string device = "";
            
            id = Pacient.Loaded.Id;


            if (scp.IsConnected && !scm.IsConnected && !scc.IsConnected)
            {
                Debug.Log("Dispositivo(s): P");
                device = "P";
            } else {

             if (scp.IsConnected && scm.IsConnected && !scc.IsConnected)
            {
                Debug.Log("Dispositivo(s): PM: Combinação incorreta");
                device = "P";
            } else {
                    
            if (!scp.IsConnected && scm.IsConnected && !scc.IsConnected)
            {
                Debug.Log("Dispositivo(s): M");
                device = "M";
            } else {
                    
            if (!scp.IsConnected && !scm.IsConnected && scc.IsConnected)
            {
                Debug.Log("Dispositivo(s): C");
                device = "C";
            } else {
                    
            if (scp.IsConnected && !scm.IsConnected && scc.IsConnected)
            {
                Debug.Log("Dispositivo(s): PC");
                device = "PC";
            } else {
                    
            if (!scp.IsConnected && scm.IsConnected && scc.IsConnected)
            {
                Debug.Log("Dispositivo(s): MC");
                device = "MC";
            } else {
                    
            // if (plr.device == "PO") // sco.IsConnected
            // {
            //     Debug.Log("Dispositivo(s): PO");
            //     device = "PO"; 
            // }
                    
            // if (plr.device == "MO")
            // {
            //     Debug.Log("Dispositivo(s): MO");
            //     device = "MO";
            // }
                    
            // if (plr.device == "CO")
            // {
            //     Debug.Log("Dispositivo(s): CO");
            //     device = "CO";
            // }
                    
            // if (plr.device == "PCO")
            // {
            //     Debug.Log("Dispositivo(s): PCO");
            //     device = "PCO";
            // }
                    
            // if (plr.device == "MCO")
            // {
            //     Debug.Log("Dispositivo(s): MCO");
            //     device = "MCO";
            // }
            }}}}}}


            for (var i = 1; i < grid.Length; i++)
            {
                if (string.IsNullOrEmpty(grid[i][0]))
                    continue;

                if (device == grid[i][0])
                {

                    var par = new Parameters
                    {
                        device = grid[i][0], // Dispositivos                    
                        lostWtimes = int.Parse(grid[i][1]), // Perdeu W vezes (Alt. e Tam.)
                        additionalHeight = Parsers.Float(grid[i][2]), // Altura adicional dos Alvos
                        additionalSize = Parsers.Float(grid[i][3]), // Tamanho adicional dos Obstáculos
                        LostXtimes = int.Parse(grid[i][4]), // Perdeu X vezes (Recalibrar disp.)
                        AdditionalDistance = Parsers.Float(grid[i][5]), // Distância adicional entre Objetos
                        CalibrateStartBelt = bool.Parse(grid[i][6]), // Calibrar Cinta no início?
                        FusionType = grid[i][7], // Fusão de Sinais
                        FusionPrefPitaco = Parsers.Float(grid[i][8]), // PrefPitaco
                        FusionPrefMano = Parsers.Float(grid[i][9]), // PrefMano
                        FusionPrefCinta = Parsers.Float(grid[i][10]), // PrefCinta
                        ScoreCalculationFactor = Parsers.Float(grid[i][11]), // Fator de Cálculo da Pontuação
                        MinimumExtensionBelt = Parsers.Float(grid[i][12]), // Valor mínimo exigido da Cinta Extensora
                        MinimumNormalOxygenation = int.Parse(grid[i][13]), // Oxigenação Normal Mínima
                        MinimumRegularOxygenation = int.Parse(grid[i][14]) // Oxigenação Regular Mínima
                    };

                    parameters = par;

                }
               
            }
        }
    }
}