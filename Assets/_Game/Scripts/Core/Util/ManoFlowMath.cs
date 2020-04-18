using System.Collections.Generic;
using System.Linq;
using Ibit.Core.Data;
using UnityEngine;

namespace Ibit.Core.Util
{
    public class ManoFlowMath
    {
        /// <summary>
        /// Calculates a mean of respiratory duration in Seconds per Cycle.
        /// </summary>
        /// <param name="data">Dictionary containing respiratory samples as [time,value] from MANO.</param>
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

                if (actualValue < -Pacient.Loaded.ManoThreshold || actualValue > Pacient.Loaded.ManoThreshold)
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
        private const float CentimetersofWaterConverter = 0.01019716f; 
        // 1 Pascal (Pa) = 0.01019716 Centimeters of Water (cmH2O)
        // 1 Pascal (Pa) = 0,10197162 Milimeters of Water (mmH2O)
        // 1 Pascal (Pa) = 0.00075006 Centimeters of Mercury (cmHg)
        // 1 Pascal (Pa) = 0.00750064 Milimeters of Mercury (mmHg)

        /// <summary>
        /// Returns the pressure in Centimeters of Water
        /// </summary>
        /// <param name="absolutePressure">Pressure in Pascal (Pa)</param>
        /// <returns></returns>
        public static float ToCentimetersofWater(float absolutePressure) =>
            absolutePressure * CentimetersofWaterConverter;
    }
}