using Ibit.Core.Data;
using Ibit.Core.Util;
using Ibit.Plataform.Camera;
using UnityEngine;

namespace Ibit.Plataform
{
    
    public partial class Player
    {

        public float TestSensorValue;// p/ a plataforma de testes
        public float TestPositionValue;// p/ a plataforma de testes
        
        public object outputDoPlayer { get; set; }
        private void PositionOnSerial(string msg)
        {
            if (msg.Length < 1)
                return;

/////////////////////////// PITACO ///////////////////////////
            //var sensorValue = Parsers.Float(msg);

            // sensorValue = sensorValue < -Pacient.Loaded.PitacoThreshold || sensorValue > Pacient.Loaded.PitacoThreshold ? sensorValue : 0f;
            // // sensorValue = sensorValue < -Pacient.Loaded.PitacoThreshold || sensorValue > Pacient.Loaded.PitacoThreshold ? sensorValue : (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow+Pacient.Loaded.CapacitiesCinta.InsPeakFlow)/2f;

            // var peak = sensorValue > 0 ? Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow * 0.3f : -Pacient.Loaded.CapacitiesPitaco.InsPeakFlow;

/////////////////////////// CINTA ///////////////////////////
            var sensorValue = Parsers.Float(msg)+(Pacient.Loaded.CapacitiesCinta.ExpPeakFlow);

            if (sensorValue == Pacient.Loaded.CapacitiesCinta.ExpPeakFlow) //teste 01
            {
                sensorValue = Parsers.Float(msg)+(Pacient.Loaded.CapacitiesCinta.ExpPeakFlow);
                if (sensorValue == Pacient.Loaded.CapacitiesCinta.ExpPeakFlow) //teste 02
                {
                    sensorValue = Parsers.Float(msg)+(Pacient.Loaded.CapacitiesCinta.ExpPeakFlow);
                    if (sensorValue == Pacient.Loaded.CapacitiesCinta.ExpPeakFlow) //teste 03
                    {
                        sensorValue = 0f;
                    }
                }
            }

            var peak = sensorValue > 0 ? Pacient.Loaded.CapacitiesCinta.ExpPeakFlow * 0.3f : -Pacient.Loaded.CapacitiesCinta.InsPeakFlow;

//////////////////////////////////////////////////////////////

            var nextPosition = sensorValue * CameraLimits.Boundary / peak; // Ponto crucial do cálculo da posição do blue
            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary);// Ponto crucial do cálculo da posição do blue

            //Debug.Log($"NextPosition: {nextPosition}\nPeak: {peak}\nSensorVal: {sensorValue}");

            TestSensorValue = sensorValue; // p/ a plataforma de testes
            TestPositionValue = nextPosition; // p/ a plataforma de testes

            var from = this.transform.position;
            var to = new Vector3(this.transform.position.x, -nextPosition);

            // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
            this.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Valor original: Vector3.Lerp(from, to, Time.deltaTime * 15f);
            
            //Debug.Log($"lerpSpeed: {Time.deltaTime * 5f}");
            
        }
    }
}