using System.Collections.Generic;
using System.Linq;
using Ibit.Core.Data;
using UnityEngine;

namespace Ibit.Core.Util
{
    public class PitacoFlowMath
    {
        /// <summary>
        /// Calculates a mean of respiratory duration in Seconds per Cycle.
        /// </summary>
        /// <param name="data">Dictionary containing respiratory samples as [time,value] from PITACO.</param>
        /// <param name="duration">Duration of the sample capture.</param>
        public static float RespiratoryRate(Dictionary<float, float> data, int duration)
        {
            var samples = data.ToList();

            float startTime = 0, firstCurveTime = 0, secondCurveTime = 0;
            float quantCycles = 0;

            for (var i = 1; i < samples.Count; i++)
            {
                var actualTime = samples[i].Key;
                var actualValue = samples[i].Value;

                var lastTime = samples[i - 1].Key;

                if (actualValue < -Pacient.Loaded.PitacoThreshold || actualValue > Pacient.Loaded.PitacoThreshold)
                {
                    if (startTime == 0)
                    {
                        startTime = lastTime;
                    }
                }
                else
                {
                    if (startTime == 0)
                        continue;

                    if (firstCurveTime == 0)
                    {
                        firstCurveTime = actualTime - startTime;
                    }
                    else if (secondCurveTime == 0)
                    {
                        secondCurveTime = actualTime - startTime;
                    }

                    startTime = 0;
                }

                if (firstCurveTime == 0 || secondCurveTime == 0)
                    continue;

                quantCycles++;
                firstCurveTime = 0;
                secondCurveTime = 0;
            }

            //UnityEngine.Debug.Log($"{quantCycles}/{duration} = {quantCycles/duration} ({quantCycles/duration*60} bpm)");

            return quantCycles / duration;
        }

        /// <summary>
        /// Converts m³/s to L/min
        /// </summary>
        private const float LitresPerMinuteConverter = 60000; //60 seconds

        /// <summary>
        /// Returns the volumetric flow of air in Cubic Meter / Second
        /// </summary>
        /// <param name="differentialPressure">Pressure difference in Pascal (Pa)</param>
        /// <returns></returns>
        private static float Poiseulle(float differentialPressure) =>
            differentialPressure * Mathf.PI * Mathf.Pow(Pitaco.Radius, 4) / (8 * Pitaco.AirViscosity * Pitaco.Lenght);

        /// <summary>
        /// Returns the volumetric flow of air in Litres/Minute
        /// </summary>
        /// <param name="differentialPressure">Pressure difference in Pascal (Pa)</param>
        /// <returns></returns>
        public static float ToLitresPerMinute(float differentialPressure) =>
            Poiseulle(differentialPressure / 1000f) * LitresPerMinuteConverter; // Poiseulle(absolutePressure / 1000f) = Litre per second
    }
}