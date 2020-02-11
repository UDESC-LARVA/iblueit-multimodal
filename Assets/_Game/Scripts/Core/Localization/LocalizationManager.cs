// Source: https://www.youtube.com/watch?v=5Kt9jbnqzKA&list=PLX2vGYjWbI0TWkV9aEYq93bOX2kwseqUT

using Ibit.Core.Util;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ibit.Core.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance;

        private Dictionary<string, string> _localizedText;

        public bool IsReady { get; private set; }
        public bool IsLoaded { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            _localizedText = new Dictionary<string, string>();

#if UNITY_EDITOR
            LoadLocalizationData("brazilian.json");
#endif
        }

        public void LoadLocalizationData(string filename)
        {
            _localizedText.Clear();

            var filepath = Application.streamingAssetsPath + @"/Localization/" + filename;

            if (!File.Exists(filepath))
            {
                Debug.LogErrorFormat("Failed to load localized text on {0}", filepath);
                return;
            }

#if UNITY_ANDROID
        var www = new WWW(filepath);

        if (!www.isDone)
        {
            Debug.Log("Waiting www...");
        }

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogErrorFormat("WWW Failed to load localized text on {0}", filepath);
            return;
        }

        var jsonData = www.text;
#else
            var jsonData = FileManager.ReadAllText(filepath);
#endif

            var loadedData = JsonUtility.FromJson<LocalizationData>(jsonData);

            foreach (var localizationItem in loadedData.Items)
                _localizedText.Add(localizationItem.Key, localizationItem.Value);

            Debug.Log($"LocalizationManager loaded {_localizedText.Count} items.");

            IsLoaded = true;
            IsReady = true;
        }

        public string GetLocalizedValue(string key) =>
            !_localizedText.ContainsKey(key) ? key : _localizedText[key];
    }
}