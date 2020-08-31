using System;
using System.Collections;
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
        //public int id;
        public static Parameters parameters;
        // public Parameters currentParameters;
        private readonly string filePath = @"Config/_parametersList.csv";

        private SerialControllerPitaco scp;
        private SerialControllerMano scm;
        private SerialControllerCinta scc;
        private SerialControllerOximetro sco;

        private void OnEnable()
        {
            //parameters = null;
            // id = Pacient.Loaded.Id;

            LoadParameters();
        }

        private void LoadParameters()
        {
            if (scp == null)
                scp = FindObjectOfType<SerialControllerPitaco>();
    
            if (scm == null)
                scm = FindObjectOfType<SerialControllerMano>();

            if (scc == null)
                scc = FindObjectOfType<SerialControllerCinta>();

            if (sco == null)
                sco = FindObjectOfType<SerialControllerOximetro>();

            if(scp.IsConnected || scm.IsConnected || scc.IsConnected || sco.IsConnected)
            {
                StartCoroutine(Load());
            }
        }

        public IEnumerator Load()
        {
          if (File.Exists(filePath))
          {

            var csvData = FileManager.ReadCsv(filePath);
            var grid = CsvParser2.Parse(csvData);
            string device = "";
            
            // id = Pacient.Loaded.Id;

            if (scp.IsConnected && !scm.IsConnected && !scc.IsConnected && !sco.IsConnected) // PITACO
            {
                Debug.Log("Dispositivo(s): P");
                device = "P";
            } else {
                    
            if (!scp.IsConnected && scm.IsConnected && !scc.IsConnected && !sco.IsConnected) // MANO
            {
                Debug.Log("Dispositivo(s): M");
                device = "M";
            } else {
                    
            if (!scp.IsConnected && !scm.IsConnected && scc.IsConnected && !sco.IsConnected) // CINTA
            {
                Debug.Log("Dispositivo(s): C");
                device = "C";
            } else {
                    
            if (scp.IsConnected && !scm.IsConnected && scc.IsConnected && !sco.IsConnected) // PITACO + CINTA
            {
                Debug.Log("Dispositivo(s): PC");
                device = "PC";
            } else {
                    
            if (!scp.IsConnected && scm.IsConnected && scc.IsConnected && !sco.IsConnected) // MANO + CINTA
            {
                Debug.Log("Dispositivo(s): MC");
                device = "MC";
            } else {
            
            if (scp.IsConnected && !scm.IsConnected && !scc.IsConnected && sco.IsConnected) // PITACO + OXÍMETRO
            {
                Debug.Log("Dispositivo(s): PO");
                device = "PO";
            } else {

            if (!scp.IsConnected && scm.IsConnected && !scc.IsConnected && sco.IsConnected) // MANO + OXÍMETRO
            {
                Debug.Log("Dispositivo(s): MO");
                device = "MO";
            } else {
                    
            if (!scp.IsConnected && !scm.IsConnected && scc.IsConnected && sco.IsConnected) // CINTA + OXÍMETRO
            {
                Debug.Log("Dispositivo(s): CO");
                device = "CO";
            } else {
                    
            if (scp.IsConnected && !scm.IsConnected && scc.IsConnected && sco.IsConnected) // PITACO + CINTA + OXÍMETRO
            {
                Debug.Log("Dispositivo(s): PCO");
                device = "PCO";
            } else {
            
            if (!scp.IsConnected && scm.IsConnected && scc.IsConnected && sco.IsConnected) // MANO + CINTA + OXÍMETRO
            {
                Debug.Log("Dispositivo(s): MCO");
                device = "MCO";
            } else {

            if (!scp.IsConnected && !scm.IsConnected && !scc.IsConnected && sco.IsConnected) // OXÍMETRO
            {
                Debug.Log("Dispositivo(s): O [Combinação incorreta, o dispositivo Oxímetro não pode ser usado sozinho]");
                device = "O";
            } else {

            if (scp.IsConnected && scm.IsConnected && !scc.IsConnected && !sco.IsConnected) // PITACO + MANO
            {
                Debug.Log("Dispositivo(s): PM [Combinação incorreta, somente Pitaco será considerado]");
                device = "P";
            } else {

            if (scp.IsConnected && scm.IsConnected && scc.IsConnected && !sco.IsConnected) // PITACO + MANO + CINTA
            {
                Debug.Log("Dispositivo(s): PMC [Combinação incorreta, somente Pitaco e Cinta serão considerados]");
                device = "PC";
            } else {

            if (scp.IsConnected && scm.IsConnected && !scc.IsConnected && sco.IsConnected) // PITACO + MANO + OXÍMETRO
            {
                Debug.Log("Dispositivo(s): PMO [Combinação incorreta, somente Pitaco e Oxímetro serão considerados]");
                device = "PO";
            } else {

            if (scp.IsConnected && scm.IsConnected && scc.IsConnected && sco.IsConnected) // PITACO + MANO + CINTA + OXÍMETRO
            {
                Debug.Log("Dispositivo(s): PMCO [Combinação incorreta, somente Pitaco, Cinta e Oxímetro serão considerados]");
                device = "PCO";
            }}}}}}}}}}}}}}}


            for (var i = 1; i < grid.Length; i++)
            {
                if (string.IsNullOrEmpty(grid[i][0]))
                    continue;

                if (device == grid[i][0])
                {

                    var par = new Parameters
                    {
                        device = grid[i][0], // Dispositivo
                        lostWtimes = int.Parse(grid[i][1]), // Perdeu W vezes (Alt. e Tam.) %%%%%%%%%%%%%
                        decreaseHeight = Parsers.Float(grid[i][2]), // Fator de decremento da ALTURA dos Alvos %%%%%%%%%%%%%
                        decreaseSize = Parsers.Float(grid[i][3]), // Fator de decremento do TAMANHO dos Obstáculos %%%%%%%%%%%%%
                        lostXtimes = int.Parse(grid[i][4]), // Perdeu X vezes (Recalibrar disp.) %%%%%%%%%%%%%
                        AdditionalDistance = Parsers.Float(grid[i][5]), // Distância adicional entre Obstáculos %%%%%%%%%%%%%
                        ObjectsSpeedFactor = Parsers.Float(grid[i][6]), // Fator de cálculo da velocidade de movimento dos objetos de jogo (Alvos e Obstáculos)
                        FusionType = grid[i][7], // Fusão de Sinais
                        FusionPrefPitaco = Parsers.Float(grid[i][8]), // Porcentagem Preferencial Pitaco
                        FusionPrefMano = Parsers.Float(grid[i][9]), // Porcentagem Preferencial Mano
                        FusionPrefCinta = Parsers.Float(grid[i][10]), // Porcentagem Preferencial Cinta
                        FusionSubDevice = grid[i][11],
                        FusionFunctIns = grid[i][12],
                        FusionFunctExp = grid[i][13],
                        ScoreCalculationFactor = Parsers.Float(grid[i][14]), // Fator de Cálculo da Pontuação %%%%%%%%%%%%%
                        MinimumExtensionBelt = Parsers.Float(grid[i][15]), // Valor mínimo exigido da Cinta Extensora
                        MinimumNormalOxygenation = int.Parse(grid[i][16]), // Oxigenação Normal Mínima
                        MinimumRegularOxygenation = int.Parse(grid[i][17]) // Oxigenação Regular Mínima
                    };

                    parameters = par;
                    // currentParameters = parameters;

                }
            }

            yield return new WaitForSeconds(10f); // Espera 10 segundos depois de carregar os parâmetros
            LoadParameters();
          }
        }
    }
}