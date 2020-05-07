using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assets._Game.Scripts.Core.Api;
using Assets._Game.Scripts.Core.Api.Dto;
using Ibit.Core.Data.Constants;

namespace Ibit.Core.Data.Manager
{
    public class RemoteDataManager
    {
        public static RemoteDataManager Instance { get; } = new RemoteDataManager();

        static RemoteDataManager() { }

        private RemoteDataManager() { }

        public async Task<bool> FlushRemoteData()
        {
            var pacientDirectoriesPath = Directory.GetDirectories($"{GameDataPaths.remoteDataPath}/Pacients");

            var remoteDataTasks = pacientDirectoriesPath.Select(AnalyzePacientRemoteDirectory).ToList();

            var result = await Task.WhenAll(remoteDataTasks);

            return result.All(x => x);
        }

        private async Task<bool> AnalyzePacientRemoteDirectory(string pacientRemotePath)
        {
            var pacientId = Path.GetFileName(pacientRemotePath);
            var pacientFilePath = Directory.GetFiles(pacientRemotePath).FirstOrDefault();
            if (pacientFilePath != null)
            {
                ApiResponse<PacientDto> apiResponse;
                var jsonContent = DataManagerUtil.LoadJsonFile(pacientFilePath);
                if (Path.GetFileName(Path.GetDirectoryName(pacientRemotePath)).StartsWith("n"))
                {
                    apiResponse = await ApiClient.Instance.SavePacient(null, jsonContent);
                }
                else
                {
                    apiResponse = await ApiClient.Instance.UpdatePacient(null, pacientId, jsonContent);
                }

                if (!apiResponse.Success)
                    return false;

                pacientId = apiResponse.Data.Id;

                File.Delete(pacientFilePath);
            }

            var minigamesDirectory = Directory.GetDirectories(pacientRemotePath).FirstOrDefault(directoryName => string.Equals(Path.GetFileName(directoryName), "MinigamesData"));
            var plataformsDirectory = Directory.GetDirectories(pacientRemotePath).FirstOrDefault(directoryName => string.Equals(Path.GetFileName(directoryName), "PlataformsData"));
            var calibrationsDirectory = Directory.GetDirectories(pacientRemotePath).FirstOrDefault(directoryName => string.Equals(Path.GetFileName(directoryName), "CalibrationsData"));

            var remoteDataTasks = new List<Task<bool>>();

            if (minigamesDirectory != null)
                remoteDataTasks.AddRange(AnalyzeMinigamesDirectory(minigamesDirectory, pacientId));
            if (plataformsDirectory != null)
                remoteDataTasks.AddRange(AnalyzePlataformsDirectory(plataformsDirectory, pacientId));
            if (calibrationsDirectory != null)
                remoteDataTasks.AddRange(AnalyzeCalibrationsDirectory(calibrationsDirectory, pacientId));

            var result = await Task.WhenAll(remoteDataTasks);

            if (!result.All(x => x)) return false;

            Directory.Delete(pacientRemotePath, true);

            return true;
        }

        private List<Task<bool>> AnalyzePlataformsDirectory(string plataformDirectoryPath, string pacientId)
        {
            var savePlataformOverviewTasks = new List<Task<bool>>();

            var plataformFilesPath = Directory.GetFiles(plataformDirectoryPath);

            if (!plataformFilesPath.Any()) return savePlataformOverviewTasks;

            savePlataformOverviewTasks.AddRange(plataformFilesPath.Select(path=> ProcessPlataformRemoteData(path, pacientId)));

            return savePlataformOverviewTasks;
        }

        private List<Task<bool>> AnalyzeMinigamesDirectory(string minigamesDirectoryPath, string pacientId)
        {
            var saveMinigameOverviewTasks = new List<Task<bool>>();

            var minigameFilesPath = Directory.GetFiles(minigamesDirectoryPath);

            if (!minigameFilesPath.Any()) return saveMinigameOverviewTasks;

            saveMinigameOverviewTasks.AddRange(minigameFilesPath.Select(path=>ProcessMinigameRemoteData(path, pacientId)));

            return saveMinigameOverviewTasks;
        }

        private List<Task<bool>> AnalyzeCalibrationsDirectory(string calibrationsDirectoryPath, string pacientId)
        {
            var saveCalibrationOverviewTasks = new List<Task<bool>>();

            var calibrationFilesPath = Directory.GetFiles(calibrationsDirectoryPath);

            if (!calibrationFilesPath.Any()) return saveCalibrationOverviewTasks;

            saveCalibrationOverviewTasks.AddRange(calibrationFilesPath.Select(path=>ProcessCalibrationRemoteData(path, pacientId)));

            return saveCalibrationOverviewTasks;
        }

        private async Task<bool> ProcessPlataformRemoteData(string plataformOverviewPath, string pacientId)
        {
            var plataformOverviewSendDto = await DataManagerUtil.LoadJsonFileAsync<PlataformOverviewSendDto>(plataformOverviewPath);
            plataformOverviewSendDto.PacientId = pacientId;

            var apiResponse = await ApiClient.Instance.SavePlataformOverview(plataformOverviewSendDto);

            if (!apiResponse.Success) return false;

            File.Delete(plataformOverviewPath);
            return true;
        }

        private async Task<bool> ProcessMinigameRemoteData(string mingameOverviewPath, string pacientId)
        {
            var plataformOverviewSendDto = await DataManagerUtil.LoadJsonFileAsync<MinigameOverviewSendDto>(mingameOverviewPath);
            plataformOverviewSendDto.PacientId = pacientId;

            var apiResponse = await ApiClient.Instance.SaveMinigameOverview(plataformOverviewSendDto);

            if (!apiResponse.Success) return false;

            File.Delete(mingameOverviewPath);
            return true;
        }

        private async Task<bool> ProcessCalibrationRemoteData(string calibrationOverviewPath, string pacientId)
        {
            var calibrationOverviewSendDto = await DataManagerUtil.LoadJsonFileAsync<CalibrationOverviewSendDto>(calibrationOverviewPath);
            calibrationOverviewSendDto.PacientId = pacientId;

            var apiResponse = await ApiClient.Instance.SaveCalibrationOverview(calibrationOverviewSendDto);

            if (!apiResponse.Success) return false;

            File.Delete(calibrationOverviewPath);
            return true;
        }
    }
}