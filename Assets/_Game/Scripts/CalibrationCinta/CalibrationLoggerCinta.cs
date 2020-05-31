using System;
using Assets._Game.Scripts.Core.Api.Extensions;
using Ibit.Core.Data;
using Ibit.Core.Data.Enums;
using Ibit.Core.Util;

namespace Ibit.Calibration
{
    public class CalibrationLoggerCinta
    {
        private FlowDataDevice FlowDataDevice { get; set; }

        public CalibrationLoggerCinta()
        {
            FlowDataDevice = new FlowDataDevice {DeviceName = GameDevice.Cinta.GetDescription()};
        }

        public void Write(CalibrationExerciseResultCinta result, CalibrationExerciseCinta exercise, float value)
        {
            switch (exercise)
            {
                case CalibrationExerciseCinta.ExpiratoryPeak:
                case CalibrationExerciseCinta.InspiratoryPeak:
                    FlowDataDevice.FlowData.Add(new FlowData
                    {
                        Date = DateTime.Now,
                        Value = CintaFlowMath.ToLitresPerMinute(value)
                    });
                    break;
                case CalibrationExerciseCinta.RespiratoryFrequency:
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