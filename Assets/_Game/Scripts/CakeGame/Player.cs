using Ibit.Core.Serial;
using Ibit.Core.Util;
using UnityEngine;

namespace Ibit.CakeGame
{

    public class Player : MonoBehaviour
    {

        public Candles candle;
        public Stat flow;
        public float picoExpiratorio;
        public Stars score;
        public ScoreMenu scoreMenu;
        public float sensorValue;
        public bool stopedFlow;


       


        private void OnMessageReceived(string msg)
        {
            if (msg.Length < 1)
                return;

            sensorValue = Parsers.Float(msg);

            if (sensorValue > 0 && picoExpiratorio < sensorValue)
                picoExpiratorio = sensorValue;
            
        }

        private void Start() 
        {

            var scp = FindObjectOfType<SerialControllerPitaco>();
            var scm = FindObjectOfType<SerialControllerMano>();
            var scc = FindObjectOfType<SerialControllerCinta>();


            if (scp.IsConnected) // Se Pitaco conectado
            {
                FindObjectOfType<SerialControllerPitaco>().OnSerialMessageReceived += OnMessageReceived;

            } else {
            if (scm.IsConnected) // Se Mano conectado
            {
                FindObjectOfType<SerialControllerMano>().OnSerialMessageReceived += OnMessageReceived;

            } else {
            if (scc.IsConnected) // Se Cinta conectada
            {
                FindObjectOfType<SerialControllerCinta>().OnSerialMessageReceived += OnMessageReceived;

            }}}
            

        }
    }
}