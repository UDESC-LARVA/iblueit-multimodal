using Ibit.Core.Serial;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class OximetroStatus : MonoBehaviour
    {
        [SerializeField]
        private SerialControllerPitaco serialController;

        [SerializeField]
        private Sprite offline, online;

        private void Awake()
        {
            if (serialController == null)
                serialController = FindObjectOfType<SerialControllerPitaco>();

            if (serialController == null)
                Debug.LogWarning("Serial Controller instance not found!");
        }

        private void FixedUpdate() => GetComponent<Image>().sprite = serialController.IsConnected ? online : offline;

        public void Reboot() => UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}