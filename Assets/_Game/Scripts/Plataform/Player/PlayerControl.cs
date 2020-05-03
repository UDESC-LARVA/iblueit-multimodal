using Ibit.Core.Data;
using Ibit.Core.Serial;
using Ibit.Core.Util;
using Ibit.Plataform.Camera;
using UnityEngine;

namespace Ibit.Plataform
{
    
    public partial class Player
    {

        public float TestSensorValue;// p/ a plataforma de testes
        public float TestPositionValue;// p/ a plataforma de testes

        public float HRValue;
        public float SPO2Value;
        public bool oxiActive = false;
        
        [SerializeField]
        private SerialControllerPitaco scp;

        [SerializeField]
        private SerialControllerMano scm;

        [SerializeField]
        private SerialControllerCinta scc;

        [SerializeField]
        private SerialControllerOximetro sco;

        public object outputDoPlayer { get; set; }



        private void PositionOnSerialPitaco(string msg)
        {
            if (msg.Length < 1)
                return;

            // scp = FindObjectOfType<SerialControllerPitaco>();

            var sensorValuePitaco = 0f;
            
            var peakPitaco = 0f;

            var nextPosition = 0f;

            sensorValuePitaco = Parsers.Float(msg);
            sensorValuePitaco = sensorValuePitaco < -Pacient.Loaded.PitacoThreshold || sensorValuePitaco > Pacient.Loaded.PitacoThreshold ? sensorValuePitaco : 0f;

            peakPitaco = sensorValuePitaco > 0 ? Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow : -Pacient.Loaded.CapacitiesPitaco.InsPeakFlow;


            nextPosition = sensorValuePitaco * CameraLimits.Boundary / peakPitaco; // Ponto crucial do cálculo da posição do blue
            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary);// Ponto crucial do cálculo da posição do blue

            TestSensorValue = sensorValuePitaco; // p/ a plataforma de testes
            TestPositionValue = nextPosition; // p/ a plataforma de testes


            var from = this.transform.position;
            var to = new Vector3(this.transform.position.x, -nextPosition);

            // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
            this.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Valor original: Vector3.Lerp(from, to, Time.deltaTime * 15f);
            
        }




        private void PositionOnSerialMano(string msg)
        {
            if (msg.Length < 1)
                return;

            // scm = FindObjectOfType<SerialControllerMano>();

            var sensorValueMano = 0f;
            
            var peakMano = 0f;

            var nextPosition = 0f;

            sensorValueMano = Parsers.Float(msg);
            sensorValueMano = sensorValueMano < -Pacient.Loaded.ManoThreshold || sensorValueMano > Pacient.Loaded.ManoThreshold ? sensorValueMano : 0f;

            peakMano = sensorValueMano > 0 ? Pacient.Loaded.CapacitiesMano.ExpPeakFlow : -Pacient.Loaded.CapacitiesMano.InsPeakFlow;

            
            nextPosition = sensorValueMano * CameraLimits.Boundary / peakMano; // Ponto crucial do cálculo da posição do blue
            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary);// Ponto crucial do cálculo da posição do blue

            TestSensorValue = sensorValueMano; // p/ a plataforma de testes
            TestPositionValue = nextPosition; // p/ a plataforma de testes


            var from = this.transform.position;
            var to = new Vector3(this.transform.position.x, -nextPosition);

            // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
            this.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Valor original: Vector3.Lerp(from, to, Time.deltaTime * 15f);
            
        }


        private void PositionOnSerialCinta(string msg)
        {
            if (msg.Length < 1)
                return;

            // scc = FindObjectOfType<SerialControllerCinta>();

            var sensorValueCinta = 0f;
            
            var peakCinta = 0f;

            var nextPosition = 0f;

            sensorValueCinta = Parsers.Float(msg)+Pacient.Loaded.CapacitiesCinta.ExpPeakFlow;
            sensorValueCinta = sensorValueCinta < -Pacient.Loaded.CintaThreshold || sensorValueCinta > Pacient.Loaded.CintaThreshold ? sensorValueCinta : 0f;

            peakCinta = sensorValueCinta > 0 ? Pacient.Loaded.CapacitiesCinta.ExpPeakFlow : -Pacient.Loaded.CapacitiesCinta.InsPeakFlow;

            
            nextPosition = sensorValueCinta * CameraLimits.Boundary / peakCinta; // Ponto crucial do cálculo da posição do blue
            nextPosition = Mathf.Clamp(nextPosition, -CameraLimits.Boundary, CameraLimits.Boundary);// Ponto crucial do cálculo da posição do blue

            TestSensorValue = sensorValueCinta; // p/ a plataforma de testes
            TestPositionValue = nextPosition; // p/ a plataforma de testes


            var from = this.transform.position;
            var to = new Vector3(this.transform.position.x, -nextPosition);

            // Lerp(de onde está, para onde vai, lerpSpeed = velocidade de deslocamento);
            this.transform.position = Vector3.Lerp(from, to, Time.deltaTime * 10f); // Valor original: Vector3.Lerp(from, to, Time.deltaTime * 15f);
            
        }

        private void PositionOnSerialOximetro(string msg)
        {
            if (msg.Length < 1)
                return;

            // Se o Oxímetro estiver conectado
            if (sco.IsConnected)
            {   oxiActive = true;
                string[] sensorValueOxi = msg.Split(',');
                HRValue = Parsers.Float(sensorValueOxi[0]);
                SPO2Value = Parsers.Float(sensorValueOxi[1]);
            } else {
                oxiActive = false;
            }
            
        }
    }
}