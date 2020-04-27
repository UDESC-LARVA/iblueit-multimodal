using Assets._Game.Scripts.Core.Configuration;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        public void OnVolumeChanged(float value)
        {
            AudioListener.volume = value;
        }

        public void OnGameApiTokenChanged(InputField inputField)
        {
            ConfigurationManager.GameApiToken = inputField.text;
        }

        public void OnNeedToSendRemoteDataChanged(Toggle toggle)
        {
            ConfigurationManager.SendRemoteData = toggle.isOn;
        }
    }
}