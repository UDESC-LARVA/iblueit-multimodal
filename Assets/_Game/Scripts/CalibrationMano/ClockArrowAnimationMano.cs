using Ibit.Core.Data;
using Ibit.Core.Serial;
using Ibit.Core.Util;
using UnityEngine;

namespace Ibit.Calibration
{
    public class ClockArrowAnimationMano : MonoBehaviour
    {
        public bool SpinClock { get; set; }

        private void Awake()
        {
            FindObjectOfType<SerialControllerMano>().OnSerialMessageReceived += OnSerialMessageReceived;
        }

        private void OnSerialMessageReceived(string msg)
        {
            if (!SpinClock)
                return;

            if (msg.Length < 1)
                return;

            var snsrVal = Parsers.Float(msg);

            snsrVal = snsrVal < -Pacient.Loaded.ManoThreshold || snsrVal > Pacient.Loaded.ManoThreshold ? snsrVal : 0f;

            if (((snsrVal < 150)&&(snsrVal > 0))||((snsrVal > -150)&&(snsrVal < 0))) // evita oscilações do ponteiro em medidas próximas a zero
                {
                    snsrVal = 0f;
                }

            this.transform.Rotate(Vector3.back, snsrVal);
        }
    }
}