using System;
using System.IO;
using Assets._Game.Scripts.Core.Configuration;
using Ibit.Core.Data.Constants;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        var gameTokenInput = GameObject
            .Find("Settings Menu").gameObject.transform
            .Find("InputFieldName").GetComponent<InputField>();
        gameTokenInput.text = ConfigurationManager.GameApiToken;

        GameObject.
            Find("Settings Menu").gameObject.transform
            .Find("SelectRemoteData").gameObject.transform
            .Find("Toggle")
            .GetComponent<Toggle>().isOn = ConfigurationManager.SendRemoteData;
    }

    void OnDisable()
    {
        if (!Directory.Exists(GameDataPaths.configurationPath))
            Directory.CreateDirectory(GameDataPaths.configurationPath);

        try
        {
            using (StreamWriter file = File.CreateText($"{GameDataPaths.configurationPath}/config.json"))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, ConfigurationManager.Instance);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return;
        }
    }
}
