using Newtonsoft.Json;

namespace Assets._Game.Scripts.Core.Configuration
{
    public class ConfigurationManager
    {
        public static ConfigurationManager Instance { get; } = new ConfigurationManager();

        static ConfigurationManager() { }

        private ConfigurationManager() { }

        [JsonProperty("gameVolume")]
        public static float GameVolume { get; set; }
        [JsonProperty("gameApiToken")]
        public static string GameApiToken { get; set; }
        [JsonProperty("gameSendRemoteData")]
        public static bool SendRemoteData { get; set; }
        [JsonProperty("apiEndpoint")]
        public static string ApiEndpoint { get; set; }
    }
}