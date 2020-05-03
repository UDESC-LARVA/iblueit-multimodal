using System;
using Assets._Game.Scripts.Core.Api.Extensions;
using Ibit.Core.Data;
using Ibit.Core.Data.Enums;
using Ibit.Core.Util;

namespace Ibit.Calibration
{
    public class CalibrationLoggerPitaco
    {
        private FlowDataDevice FlowDataDevice { get; set; }

        public CalibrationLoggerPitaco()
        {
            FlowDataDevice = new FlowDataDevice {DeviceName = GameDevice.Pitaco.GetDescription()};
        }

        public void Write(CalibrationExerciseResultPitaco result, CalibrationExercisePitaco exercise, float value)
        {
            switch (exercise)
            {
                case CalibrationExercisePitaco.ExpiratoryPeak:
                case CalibrationExercisePitaco.InspiratoryPeak:
                    FlowDataDevice.FlowData.Add(new FlowData
                    {
                        Date = DateTime.Now,
                        Value = PitacoFlowMath.ToLitresPerMinute(value)
                    });
                    break;
                case CalibrationExercisePitaco.RespiratoryFrequency:
                    FlowDataDevice.FlowData.Add(new FlowData
                    {
                        Date = DateTime.Now,
                        Value = value * 60f
                    });
                    break;
                default:
                    FlowDataDevice.FlowData.Add(new FlowData
                    {
                        Date = DateTime.Now,
                        Value = value / 1000f
                    });
                    break;
            }
        }
    }
}