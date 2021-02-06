using System.IO; // Comando File
using Ibit.Core.Util;  // Comando FileManager
using UnityEngine;  // Comando Debug.Log

namespace Ibit.Core.Database
{
    public class SignalTreatmentDb
    {
        // public static int SamplesSignalPitaco = LoadSignalParameters("P");
        // public static int SamplesSignalMano = LoadSignalParameters("M");
        // public static int SamplesSignalCinta = LoadSignalParameters("C");
        // public static int SamplesSignalOximetro = LoadSignalParameters("O");

        public static string filePath = @"Config/_signalTreatment.csv";

        // private void Start()
        // {
        //     LoadSignalParameters();
        // }

        public static int LoadSignalParameters(string device)
        {
            if (File.Exists(filePath))
            {
                var csvData = FileManager.ReadCsv(filePath);
                var grid = CsvParser2.Parse(csvData);

            
                if (device == "P" && !string.IsNullOrEmpty(grid[1][1])){ // Pitaco
                    return int.Parse(grid[1][1]);
                } else {

                if (device == "M" && !string.IsNullOrEmpty(grid[2][1])){ // Mano-BD
                    return int.Parse(grid[2][1]);
                } else {

                if (device == "C" && !string.IsNullOrEmpty(grid[3][1])){ // Cinta de Pressão
                    return int.Parse(grid[3][1]);
                } else {

                if (device == "O" && !string.IsNullOrEmpty(grid[4][1])){ // Oxímetro
                    return int.Parse(grid[4][1]);
                } else {
                    return 0; // Inserir novos dispositivos aqui...
                }}}}

            } else { // Caso não exista arquivo, os valores default abaixo são enviados ao serial controller do dispositivo

                if (device == "P"){ // Pitaco
                    return 500;

                } else {
                if (device == "M"){ // Mano-BD
                    return 500;

                } else {
                if (device == "C"){ // Cinta de Pressão
                    return 500;

                } else {
                if (device == "O"){ // Oxímetro
                    return 10;

                } else {
                    return 0; // Inserir novos dispositivos aqui...
                }}}}
            }
        }
    }
}