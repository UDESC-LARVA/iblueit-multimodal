using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ibit.Core.Data
{
    public class FlowDataDevice
    {
        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }

        [JsonProperty("flowData")]
        public List<FlowData> FlowData { get; set; } = new List<FlowData>();
    }
}