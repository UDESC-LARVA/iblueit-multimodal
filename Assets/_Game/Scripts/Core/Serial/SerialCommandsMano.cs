using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Ibit.Core.Serial
{
    public partial class SerialControllerMano
    {
        /// <summary>
        /// Sends an "echo" message to device.
        /// </summary>
        [Button("Send Echo")]
        private void SendEcho() => SendSerialMessage("e");

        /// <summary>
        /// Send a message to device to start sending samples.
        /// </summary>
        [Button("Start Sampling")]
        public void StartSampling() => SendSerialMessage("r");

        /// <summary>
        /// Send a message to device to start sending samples after 1.5 seconds.
        /// </summary>
        public void StartSamplingDelayed() => StartCoroutine(StartSampling_Coroutine());

        private IEnumerator StartSampling_Coroutine()
        {
            yield return new WaitForSeconds(1.5f);
            StartSampling();
        }

        /// <summary>
        /// Send a message to device to stop sampling.
        /// </summary>
        [Button("Stop Sampling")]
        public void StopSampling() => SendSerialMessage("f");

        /// <summary>
        /// Send a message to recalibrate device.
        /// </summary>
        [Button("Recalibrate")]
        public void Recalibrate() => SendSerialMessage("c");
    }
}