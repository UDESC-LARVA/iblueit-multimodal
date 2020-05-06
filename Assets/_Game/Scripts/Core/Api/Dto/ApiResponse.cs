using System.Collections.Generic;
using Newtonsoft.Json;

namespace Assets._Game.Scripts.Core.Api.Dto
{
    public class ApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("authorized")]
        public bool Authorized { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("errors")]
        public List<ApiResponseError> Errors { get; set; }
    }

    public class ApiResponseError
    {
        
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

}