using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Assets._Game.Scripts.Core.Api.Dto;
using Assets._Game.Scripts.Core.Configuration;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets._Game.Scripts.Core.Api
{
    public sealed class ApiClient
    {
        public static ApiClient Instance { get; } = new ApiClient();
        private static readonly HttpClient HTTP_CLIENT;

        private readonly string _baseUrl = ConfigurationManager.ApiEndpoint;
        private const string PacientUrl = "pacients";
        private const string MinigamesUrl = "minigames";
        private const string PlataformsUrl = "plataforms";
        private const string CalibrationsUrl = "calibrations";
        private readonly string _gameToken = ConfigurationManager.GameApiToken;

        static ApiClient()
        {
            HTTP_CLIENT = new HttpClient { Timeout = TimeSpan.FromMinutes(2) };
            HTTP_CLIENT.DefaultRequestHeaders.Accept.Clear();
            HTTP_CLIENT.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private ApiClient()
        {
        }

        public async Task<ApiResponse<PacientDto>> SavePacient(PacientSendDto pacient, string jsonText = null)
        {
            var requestUrl = $"{_baseUrl}/{PacientUrl}";

            var jsonContent = jsonText ?? JsonConvert.SerializeObject(pacient);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                request.Headers.Add("GameToken", _gameToken);
                request.Content = contentString;
                response = await HTTP_CLIENT.SendAsync(request);
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.LogWarning($"No internet connection!. Error: {httpRequestException}");
                return new ApiResponse<PacientDto>();
            }

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<PacientDto>();

            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<PacientDto>>(stringResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<CalibrationOverviewDto>> SaveCalibrationOverview(CalibrationOverviewSendDto calibrationOverviewSendDto)
        {
            var requestUrl = $"{_baseUrl}/{CalibrationsUrl}";

            var jsonContent = JsonConvert.SerializeObject(calibrationOverviewSendDto);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                request.Headers.Add("GameToken", _gameToken);
                request.Content = contentString;
                response = await HTTP_CLIENT.SendAsync(request);
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.LogWarning($"No internet connection!. Error: {httpRequestException}");
                return new ApiResponse<CalibrationOverviewDto>();
            }

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<CalibrationOverviewDto>();

            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<CalibrationOverviewDto>>(stringResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<MinigameOverviewDto>> SaveMinigameOverview(MinigameOverviewSendDto minigameOverview)
        {
            var requestUrl = $"{_baseUrl}/{MinigamesUrl}";

            var jsonContent = JsonConvert.SerializeObject(minigameOverview);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                request.Headers.Add("GameToken", _gameToken);
                request.Content = contentString;
                response = await HTTP_CLIENT.SendAsync(request);
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.LogWarning($"No internet connection!. Error: {httpRequestException}");
                return new ApiResponse<MinigameOverviewDto>();
            }

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<MinigameOverviewDto>();

            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MinigameOverviewDto>>(stringResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<PlataformOverviewDto>> SavePlataformOverview(PlataformOverviewSendDto plataformOverview)
        {
            var requestUrl = $"{_baseUrl}/{PlataformsUrl}";

            var jsonContent = JsonConvert.SerializeObject(plataformOverview);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                request.Headers.Add("GameToken", _gameToken);
                request.Content = contentString;
                response = await HTTP_CLIENT.SendAsync(request);
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.LogWarning($"No internet connection!. Error: {httpRequestException}");
                return new ApiResponse<PlataformOverviewDto>();
            }

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<PlataformOverviewDto>();

            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<PlataformOverviewDto>>(stringResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<List<PacientDto>>> GetPacients()
        {
            var requestUrl = $"{_baseUrl}/{PacientUrl}";

            HttpResponseMessage response;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Add("GameToken", _gameToken);
                response = await HTTP_CLIENT.SendAsync(request);
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.LogWarning($"No internet connection!. Error: {httpRequestException}");
                return new ApiResponse<List<PacientDto>>{Data = new List<PacientDto>()};
            }

            if(!response.IsSuccessStatusCode)
                return new ApiResponse<List<PacientDto>> { Data = new List<PacientDto>() };

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<PacientDto>>>(content);
            
            return apiResponse;
        }

        public async Task<ApiResponse<PacientDto>> UpdatePacient(PacientSendDto pacient, string idPacient, string jsonText = null)
        {
            var requestUrl = $"{_baseUrl}/{PacientUrl}/{idPacient}";

            var jsonContent = jsonText ?? JsonConvert.SerializeObject(pacient);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                request.Headers.Add("GameToken", _gameToken);
                request.Content = contentString;
                response = await HTTP_CLIENT.SendAsync(request);
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.LogWarning($"No internet connection!. Error: {httpRequestException}");
                return new ApiResponse<PacientDto>();
            }

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<PacientDto>();

            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<PacientDto>>(stringResponse);

            return apiResponse;
        }

        public async Task<bool> HasInternetConnection()
        {
            var response = await HTTP_CLIENT.GetAsync("http://google.com/generate_204");
            return response.IsSuccessStatusCode;
        }
    }
}