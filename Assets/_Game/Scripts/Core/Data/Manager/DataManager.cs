using System.Collections.Generic;
using System.Threading.Tasks;
using Assets._Game.Scripts.Core.Api;
using Assets._Game.Scripts.Core.Api.Dto;
using Assets._Game.Scripts.Core.Configuration;

namespace Ibit.Core.Data.Manager
{
    public sealed class DataManager
    {
        public static DataManager Instance { get; } = new DataManager();

        static DataManager() { }

        private DataManager() { }

        public async Task<DataManagerReponse<PacientDto, PacientSendDto>> SavePacient(PacientSendDto pacient)
        {
            var response = new DataManagerReponse<PacientDto, PacientSendDto>();

            LocalDataManager.Instance.SaveLocalData("Pacient", pacient, Pacient.Loaded.Name);

            if (!ConfigurationManager.SendRemoteData || string.IsNullOrWhiteSpace(ConfigurationManager.GameApiToken))
                return response;

            var hasInternetConnection = await ApiClient.Instance.HasInternetConnection();
            if (!hasInternetConnection)
            {
                LocalDataManager.Instance.SaveRemoteData("Pacient", pacient, Pacient.Loaded.IdApi);
                response.LocalResponse = pacient;
                return response;
            }

            response.ApiResponse = await ApiClient.Instance.SavePacient(pacient);

            return response;
        }

        public async Task<DataManagerReponse<PacientDto, PacientSendDto>> UpdatePacient(PacientSendDto pacient)
        {
            var response = new DataManagerReponse<PacientDto, PacientSendDto>();

            LocalDataManager.Instance.SaveLocalData("Pacient", pacient, Pacient.Loaded.Name);

            if (!ConfigurationManager.SendRemoteData || string.IsNullOrWhiteSpace(ConfigurationManager.GameApiToken))
                return response;

            var hasInternetConnection = await ApiClient.Instance.HasInternetConnection();
            if (!hasInternetConnection)
            {
                LocalDataManager.Instance.SaveRemoteData("Pacient", pacient, Pacient.Loaded.IdApi);
                response.LocalResponse = pacient;
                return response;
            }

            response.ApiResponse = await ApiClient.Instance.UpdatePacient(pacient, Pacient.Loaded.IdApi);

            return response;
        }

        public async Task<DataManagerReponse<PlataformOverviewDto, PlataformOverviewSendDto>> SavePlataformOverview(PlataformOverviewSendDto plataform)
        {
            var response = new DataManagerReponse<PlataformOverviewDto, PlataformOverviewSendDto>();

            LocalDataManager.Instance.SaveLocalData("PlataformOverview", plataform, Pacient.Loaded.Name);

            if (!ConfigurationManager.SendRemoteData || string.IsNullOrWhiteSpace(ConfigurationManager.GameApiToken))
                return response;

            var hasInternetConnection = await ApiClient.Instance.HasInternetConnection();
            if (!hasInternetConnection)
            {
                LocalDataManager.Instance.SaveRemoteData("PlataformOverview", plataform, Pacient.Loaded.IdApi);
                response.LocalResponse = plataform;
                return response;
            }

            response.ApiResponse = await ApiClient.Instance.SavePlataformOverview(plataform);

            return response;
        }

        public async Task<DataManagerReponse<CalibrationOverviewDto, CalibrationOverviewSendDto>> SaveCalibrationOverview(CalibrationOverviewSendDto calibration)
        {
            var response = new DataManagerReponse<CalibrationOverviewDto, CalibrationOverviewSendDto>();

            LocalDataManager.Instance.SaveLocalData("CalibrationOverview", calibration, Pacient.Loaded.Name);

            if (!ConfigurationManager.SendRemoteData || string.IsNullOrWhiteSpace(ConfigurationManager.GameApiToken))
                return response;

            var hasInternetConnection = await ApiClient.Instance.HasInternetConnection();
            if (!hasInternetConnection)
            {
                LocalDataManager.Instance.SaveRemoteData("CalibrationOverview", calibration, Pacient.Loaded.IdApi);
                response.LocalResponse = calibration;
                return response;
            }

            response.ApiResponse = await ApiClient.Instance.SaveCalibrationOverview(calibration);

            return response;
        }

        public async Task<DataManagerReponse<MinigameOverviewDto, MinigameOverviewSendDto>> SaveMinigameOverview(MinigameOverviewSendDto minigame)
        {
            var response = new DataManagerReponse<MinigameOverviewDto, MinigameOverviewSendDto>();

            LocalDataManager.Instance.SaveLocalData("MinigameOverview", minigame, Pacient.Loaded.Name);

            if (!ConfigurationManager.SendRemoteData || string.IsNullOrWhiteSpace(ConfigurationManager.GameApiToken))
                return response;

            var hasInternetConnection = await ApiClient.Instance.HasInternetConnection();
            if (!hasInternetConnection)
            {
                LocalDataManager.Instance.SaveRemoteData("MinigameOverview", minigame, Pacient.Loaded.IdApi);
                response.LocalResponse = minigame;
                return response;
            }
            response.ApiResponse = await ApiClient.Instance.SaveMinigameOverview(minigame);

            return response;
        }

        public async Task<ApiResponse<List<PacientDto>>> GetPacients()
        {
            if (!await ApiClient.Instance.HasInternetConnection() || !ConfigurationManager.SendRemoteData || string.IsNullOrWhiteSpace(ConfigurationManager.GameApiToken))
            {
                return new ApiResponse<List<PacientDto>>
                {
                    Success = false,
                    Data = LocalDataManager.Instance.GetPacientsLocal()
                };
            }
            return await ApiClient.Instance.GetPacients();
        }

        public async Task<bool> SendRemoteData()
        {
            return await RemoteDataManager.Instance.FlushRemoteData();
        }

    }
}