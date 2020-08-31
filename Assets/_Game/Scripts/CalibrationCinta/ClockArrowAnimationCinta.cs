using Ibit.Core.Data;
using Ibit.Core.Serial;
using Ibit.Core.Util;
using UnityEngine;

namespace Ibit.Calibration
{
    public class ClockArrowAnimationCinta : MonoBehaviour
    {
        public bool SpinClock { get; set; }

        public string CurrentExercise { get; set; }

        private void Awake()
        {
            FindObjectOfType<SerialControllerCinta>().OnSerialMessageReceived += OnSerialMessageReceived;
        }

        private void OnSerialMessageReceived(string msg)
        {
            if (!SpinClock)
                return;

            if (msg.Length < 1)
                return;

            var snsrVal = Parsers.Float(msg);
            
            if(CurrentExercise == "InspiratoryPeak" || CurrentExercise == "InspiratoryDuration")
            {
                // snsrVal = snsrVal < -Pacient.Loaded.CintaThreshold ? snsrVal : 0f;
                if (snsrVal <= 0)
                {
                    Debug.Log($"snsrVal: {snsrVal}");
                    this.transform.Rotate(new Vector3(0, 0, -snsrVal) / 3); // Dividir o valor da serial por 3 faz o ponteiro girar mais devagar
                }
            }

            if(CurrentExercise == "ExpiratoryPeak" || CurrentExercise == "ExpiratoryDuration")
            {
                snsrVal = snsrVal - CalibrationManagerCinta._maxFlowINS; // Cálculo ajuste p/ cinta
                if (snsrVal >= 0)
                {
                    Debug.Log($"snsrVal: {snsrVal}");
                    this.transform.Rotate(new Vector3(0, 0, -snsrVal) / 4); // Dividir o valor da serial por 4 faz o ponteiro girar mais devagar
                }
            }

            if(CurrentExercise == "RespiratoryFrequency")
            {
                snsrVal = snsrVal < -Pacient.Loaded.CintaThreshold || snsrVal > Pacient.Loaded.CintaThreshold ? snsrVal : 0f;
                this.transform.Rotate(Vector3.back, snsrVal);
            }
        }
    }
}