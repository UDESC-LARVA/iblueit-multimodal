using Ibit.Plataform.Manager.Stage;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.Plataform.UI
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField]
        private Text timerText;
        
        private StageManager stgMgr;

        private void Awake() => stgMgr = FindObjectOfType<StageManager>();

        private void FixedUpdate()
        {
            timerText.text = Mathf.Round(stgMgr.Duration).ToString(CultureInfo.InvariantCulture);
        }
    }
}