using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets._Game.Scripts.Core.Api.Dto;
using Ibit.Core.Data.Constants;
using Newtonsoft.Json;
using UnityEngine;

namespace Ibit.Core.Data.Manager
{
    public class LocalDataManager
    {
        public static LocalDataManager Instance { get; } = new LocalDataManager();

        static LocalDataManager() { }

        private LocalDataManager() { }

        public bool SaveLocalData(string type, object dataObj, string pacientName)
        {
            var path = "";
            var filename = $"info_{DateTime.Now:dd-MM-yyyy_HH-mm}.json";

            if (type == "PlataformOverview")
                path = $"{GameDataPaths.localDataPath}/Pacients/{pacientName.Replace(" ", "")}/PlataformsData";
            if (type == "MinigameOverview")
                path = $"{GameDataPaths.localDataPath}/Pacients/{pacientName.Replace(" ", "")}/MinigamesData";
            if (type == "CalibrationOverview")
                path = $"{GameDataPaths.localDataPath}/Pacients/{pacientName.Replace(" ", "")}/CalibrationsData";
            if (type == "Pacient")
            {
                path = $"{GameDataPaths.localDataPath}/Pacients/{pacientName.Replace(" ", "")}";
                filename = "info.json";
            }

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            try
            {
                using (StreamWriter file = File.CreateText($"{path}/{filename}"))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(file, dataObj);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }

            return true;
        }

        public bool SaveRemoteData(string type, object dataObj, string pacientId)
        {
            var path = "";
            var filename = $"info_{DateTime.Now:dd-MM-yyyy_HH-mm}.json";

            if (type == "PlataformOverview")
                path = $"{GameDataPaths.remoteDataPath}/Pacients/{pacientId}/PlataformsData";
            if (type == "MinigameOverview")
                path = $"{GameDataPaths.remoteDataPath}/Pacients/{pacientId}/MinigamesData";
            if (type == "CalibrationOverview")
                path = $"{GameDataPaths.remoteDataPath}/Pacients/{pacientId}/CalibrationsData";
            if (type == "Pacient")
            {
                path = $"{GameDataPaths.remoteDataPath}/Pacients/{pacientId}";
                filename = "info.json";
            }

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            try
            {
                using (StreamWriter file = File.CreateText($"{path}/{filename}"))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(file, dataObj);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }

            return true;
        }

        
        public List<PacientDto> GetPacientsLocal()
        {
            if(!Directory.Exists($"{GameDataPaths.localDataPath}/Pacients"))
                Directory.CreateDirectory($"{GameDataPaths.localDataPath}/Pacients");

            var pacientsDirectories = Directory.GetDirectories($"{GameDataPaths.localDataPath}/Pacients");
            var pacientsInfoPaths = pacientsDirectories.Select(x=>Directory.GetFiles(x).First()).ToArray();
            var pacients = pacientsInfoPaths.Select(DataManagerUtil.LoadJsonFile<PacientDto>).ToList();
            return pacients;

        }
    }
}