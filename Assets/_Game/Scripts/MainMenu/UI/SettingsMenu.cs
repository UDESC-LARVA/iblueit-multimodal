using UnityEngine;

namespace Ibit.MainMenu.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        public void SetVolume(float value)
        {
            AudioListener.volume = value;
        }
    }
}