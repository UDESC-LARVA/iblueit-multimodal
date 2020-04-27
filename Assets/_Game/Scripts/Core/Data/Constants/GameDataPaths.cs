using System.IO;
using UnityEngine;

namespace Ibit.Core.Data.Constants
{
    public static class GameDataPaths
    {
        public static string localDataPath = $"{Directory.GetParent(Application.dataPath)}/Data/LocalData";
        public static string remoteDataPath = $"{Directory.GetParent(Application.dataPath)}/Data/RemoteData";
        public static string configurationPath = $"{Directory.GetParent(Application.dataPath)}/Config";
    }
}