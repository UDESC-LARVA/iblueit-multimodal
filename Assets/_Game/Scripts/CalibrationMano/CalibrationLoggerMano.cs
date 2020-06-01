using System;
using Assets._Game.Scripts.Core.Api.Extensions;
using Ibit.Core.Data;
using Ibit.Core.Data.Enums;
using Ibit.Core.Util;

namespace Ibit.Calibration
{
    public class CalibrationLoggerMano
    {
        private FlowDataDevice FlowDataDevice { get; set; }

        public CalibrationLoggerMano()
        {
            FlowDataDevice = new FlowDataDevice {DeviceName = GameDevice.Mano.GetDescription()};
        }

        public void Write(CalibrationExerciseResultMano result, CalibrationExerciseMano exercise, float value)
        {
            switch (exercise)
            {
                case CalibrationExerciseMano.ExpiratoryPeak:
                case CalibrationExerciseMano.InspiratoryPeak:
                    FlowDataDevice.FlowData.Add(new FlowData
                    {
                        Date = DateTime.Now,
                        Value = ManoFlowMath.ToCentimetersofWater(value)
                    });
                    break;
                // case CalibrationExerciseMano.RespiratoryFrequency:
                //     FlowDataDevice.FlowData.Add(new FlowData
                //     {
                //         Date = DateTime.Now,
                //         Value = value * 60f
                //     });
                //     break;
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