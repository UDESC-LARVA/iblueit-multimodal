using Ibit.Core.Data;
using Ibit.Core.Util;
using Ibit.Plataform.Camera;
using UnityEngine;

namespace Ibit.Plataform
{
    public partial class Player
    {
        private void PositionOnSerial(string msg)
        {
            if (msg.Length < 1)
                return;

            var sensorValue = Parsers.Float(msg);

            sensorValue = sensorValue < -Pacient.Loaded.PitacoThreshold || sensorValue > Pacient.Loaded.PitacoThreshold ? sensorValue : 0f;

            var peak = sensorValue > 0 ? Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * 0.3f : -Pacient.Loaded.CapacitiesPitaco.InsPeakFlow;

            var nextPosition = sensorValue * CameraLimits.Boundary / peak;
            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary);

            //Debug.Log($"NextPosition: {nextPosition}\nPeak: {peak}\nSensorVal: {sensorValue}");

            var from = this.transform.position;
            var to = new Vector3(this.transform.position.x, -nextPosition);

            this.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 15f);
        }
    }
}