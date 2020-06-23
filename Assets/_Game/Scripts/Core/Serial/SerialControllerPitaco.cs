/**
 * SerialCommUnity (Serial Communication for Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using UnityEngine;

/**
 * This class allows a Unity program to continually check for messages from a
 * serial device.
 *
 * It creates a Thread that communicates with the serial port and continually
 * polls the messages on the wire.
 * That Thread puts all the messages inside a Queue, and this SerialController
 * class polls that queue by means of invoking SerialThreadPitaco.GetSerialMessage().
 *
 * The serial device must send its messages separated by a newline character.
 * Neither the SerialController nor the SerialThreadPitaco perform any validation
 * on the integrity of the message. It's up to the one that makes sense of the
 * data.
 */

namespace Ibit.Core.Serial
{
    public partial class SerialControllerPitaco : MonoBehaviour
    {
        public const string SERIAL_DEVICE_CONNECTED = "__Connected__";
        public const string SERIAL_DEVICE_DISCONNECTED = "__Disconnected__";

        [SerializeField]
        [Tooltip("Timeout to open a connection to arduino in milliseconds.")]
        private int arduinoOpenTime = 3000;

        [SerializeField]
        [Tooltip("Baud rate that the serial device is using to transmit data.")]
        private int baudRate = 115200;

        [SerializeField]
        [Tooltip("Maximum number of unread data messages in the queue. " +
            "New messages will be discarded.")]
        private int maxUnreadMessages = 1;

        [SerializeField]
        [Tooltip("After an error in the serial communication, or an unsuccessful " +
            "connect, how many milliseconds we should wait.")]
        private int reconnectionDelay = 1000;

        private SerialThreadPitaco serialThread;

        private Thread thread;


//#if UNITY_EDITOR
//        [SerializeField]
//        [Tooltip("Valor de teste a ser enviado")]
//        [Range(-1000f, 1000f)]
//        public float testPitacoValue = 0;

//        [SerializeField]
//        [Tooltip("Conectar PITACO Teste?")]
//        public bool testPitacoConnection = true;

//        [SerializeField]
//        [Tooltip("Parar de receber valores do PITACO")]
//        public bool stopReceivePicatoValues;
//#endif
        #region Events

        // ------------------------------------------------------------------------
        // Executes a user-defined function before Unity closes the COM port, so
        // the user can send some tear-down message to the hardware reliably.
        // ------------------------------------------------------------------------
        public delegate void TearDownFunction();
        private TearDownFunction userDefinedTearDownFunction;

        public delegate void SerialConnectedHandler();
        public event SerialConnectedHandler OnSerialConnected;

        public delegate void SerialDisconnectedHandler();
        public event SerialDisconnectedHandler OnSerialDisconnected;

        public delegate void SerialMessageReceivedHandler(string msg);
        public event SerialMessageReceivedHandler OnSerialMessageReceived;

        #endregion Events

        public bool IsConnected { get; private set; }

        public string ReadSerialMessage() => serialThread.ReadSerialMessage();

        public void SendSerialMessage(string message)
        {

//#if UNITY_EDITOR

//            if (message == "f")
//                stopReceivePicatoValues = true;
//            else if (message == "dr")
//                Connect();
//            else if (message == "r") stopReceivePicatoValues = false;

//            return;
//#endif

            if (!IsConnected)
                return;

            serialThread.SendSerialMessage(message);
        }

        public void SetTearDownFunction(TearDownFunction userFunction) => this.userDefinedTearDownFunction = userFunction;

        private static string[] GetPortNames()
        {
            if (Application.platform == RuntimePlatform.LinuxPlayer ||
                Application.platform == RuntimePlatform.LinuxEditor ||
                Application.platform == RuntimePlatform.OSXPlayer ||
                Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.Android)
            {
                return Directory.GetFiles("/dev/").Where(port => port.StartsWith("/dev/ttyACM") || port.StartsWith("/dev/tty.usb") || port.StartsWith("/dev/ttyUSB")).ToArray();
            }

            return SerialPort.GetPortNames(); //windows
        }

        private string AutoConnect()
        {
            var ports = GetPortNames();

#if !UNITY_EDITOR
            if (ports.Length < 1)
                Ibit.Core.Util.SysMessage.Warning("PITACO não encontrado!");
#endif

            foreach (var port in ports)
            {
                var sp = new SerialPort(port, baudRate)
                {
                    ReadTimeout = 1000,
                    WriteTimeout = 1000,
                    DtrEnable = true,
                    RtsEnable = true,
                    Handshake = Handshake.None
                };

                try
                {
                    sp.Open();
                    Thread.Sleep(arduinoOpenTime); // this is a fix to wait for arduino to open a connection :)
                    sp.Write("e");

                    if (!sp.ReadLine().Contains("echop"))
                        throw new TimeoutException("No response from device.");
                }
                catch (Exception e)
                {
                    //Debug.LogWarning($"Unable to connect {sp.PortName}:{sp.BaudRate}:PITACO.\n{e.GetType()}: {e.Message}");
                    sp.Close();
                    sp.Dispose();
                    continue;
                }
                
                sp.Close();
                sp.Dispose();

                return sp.PortName;
            }

            return null;
        }

        private void Awake()
        {
            Connect();
        }

        public void Connect()
        {
//#if UNITY_EDITOR
//            if (testPitacoConnection)
//            {
//                Debug.Log("Debug PITACO connected!");
//                IsConnected = true;
//                return;
//            }
//#endif
            // Debug.Log("Looking for PITACO...");

            var portName = AutoConnect();

            if (string.IsNullOrEmpty(portName))
            {
                Debug.LogWarning("Failed to connect PITACO!");
                StartCoroutine(Reconnect());
                return;
            }

            serialThread = new SerialThreadPitaco(portName, baudRate, reconnectionDelay, maxUnreadMessages);

            thread = new Thread(serialThread.RunForever);
            thread.Start();

            IsConnected = true;

            Debug.Log($"Connected to {portName}:{baudRate}:PITACO");
        }

        //Caso desconectado, tenta reconectar a cada 10 segundos.
        private IEnumerator Reconnect()
        {
            yield return new WaitForSeconds(10f);
            //Debug.Log("Tentando reconectar Pitaco...");
            Connect();
        }

        private void Disconnect()
        {
//#if UNITY_EDITOR
//            if (testPitacoConnection)
//            {
//                IsConnected = false;
//                return;
//            }
//#endif
            if (!IsConnected)
                return;

            // If there is a user-defined tear-down function, execute it before
            // closing the underlying COM port.
            userDefinedTearDownFunction?.Invoke();

            StopSampling();

            // The serialThread reference should never be null at this point,
            // unless an Exception happened in the OnEnable(), in which case I've
            // no idea what face Unity will make.
            if (serialThread != null)
            {
                serialThread.RequestStop();
                serialThread = null;
            }

            // This reference shouldn't be null at this point anyway.
            if (thread != null)
            {
                thread.Join();
                thread = null;
            }

            IsConnected = false;

            Debug.Log("Serial disconnected:PITACO");
        }

        private void OnDestroy() => Disconnect();

        private void Update()
        {
//#if UNITY_EDITOR
//            if (testPitacoConnection && stopReceivePicatoValues == false)
//            {
//                OnSerialMessageReceived?.Invoke(testPitacoValue.ToString(CultureInfo.InvariantCulture));
//                return;
//            }
//#endif
            // Read the next message from the queue
            var message = serialThread?.ReadSerialMessage();
            if (message == null)
                return;

            // Check if the message is plain data or a connect/disconnect event.
            if (string.Equals(message, SERIAL_DEVICE_CONNECTED))
            {
                IsConnected = true;
                OnSerialConnected?.Invoke();
            }
            else if (string.Equals(message, SERIAL_DEVICE_DISCONNECTED))
            {
                IsConnected = false;
                OnSerialDisconnected?.Invoke();
            }
            else
            {
                if (!IsConnected)
                    return;

                OnSerialMessageReceived?.Invoke(message);
            }
        }
    }
}