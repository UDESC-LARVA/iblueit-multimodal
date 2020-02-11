using UnityEngine;
using UnityEngine.UI;

namespace Ibit.Core.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        public string Key;

        private void OnEnable()
        {
            if (LocalizationManager.Instance != null)
                this.GetComponent<Text>().text = LocalizationManager.Instance.GetLocalizedValue(Key);
        }
    }
}