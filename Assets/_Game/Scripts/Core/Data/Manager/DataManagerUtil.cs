using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ibit.Core.Data.Manager
{
    public static class DataManagerUtil
    {
        public static async Task<T> LoadJsonFileAsync<T>(string filePath)
        {
            if (!File.Exists(filePath)) return (T)Activator.CreateInstance(typeof(T));
            using (var reader = File.OpenText(filePath))
            {
                var json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }

        }

        public static T LoadJsonFile<T>(string filePath)
        {
            if (!File.Exists(filePath)) return (T)Activator.CreateInstance(typeof(T));
            using (var reader = File.OpenText(filePath))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public static string LoadJsonFile(string filePath)
        {
            return !File.Exists(filePath) ? string.Empty : File.ReadAllText(filePath);
        }
    }
}