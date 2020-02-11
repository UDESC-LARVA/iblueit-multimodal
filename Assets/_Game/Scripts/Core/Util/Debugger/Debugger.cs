using NaughtyAttributes;
using UnityEngine;

namespace Ibit.Core.Util
{
    public partial class Debugger : MonoBehaviour
    {
        [Button("Freeze Scene")]
        private void Freeze() => Time.timeScale = 0f;

        [Button("Unfreeze Scene")]
        private void Unfreeze() => Time.timeScale = 1f;

        private void Awake()
        {
#if !UNITY_EDITOR
            Destroy(this.gameObject);
            return;
#endif

            Application.logMessageReceived += (message, stacktrace, type) =>
            {
                _messageOutput = message;
            };

            AlignFpsDisplayBox();
        }

        private void OnGUI()
        {
            DisplayFramesPerSecond();
            DisplayLogMessages();
        }

        private void Update()
        {
            UpdateFpsDisplayTimer();
        }
    }
}