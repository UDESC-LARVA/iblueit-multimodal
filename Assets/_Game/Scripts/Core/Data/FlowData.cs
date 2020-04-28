using System;
using Newtonsoft.Json;

namespace Ibit.Core.Data
{
    public class FlowData
    {
        [JsonProperty("timestamp")]
        public DateTime Date { get; set; }

        [JsonProperty("flowValue")]
        public float Value { get; set; }
    }
}