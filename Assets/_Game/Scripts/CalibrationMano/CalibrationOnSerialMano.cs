using Ibit.Core.Util;
using UnityEngine;

namespace Ibit.Calibration
{
    public partial class CalibrationManagerMano
    {
        private void OnSerialMessageReceived(string msg)
        {
            if (!_acceptingValues || msg.Length < 1)
                return;

            var tmp = Parsers.Float(msg);

            switch (_currentExercise)
            {
                case CalibrationExerciseMano.ExpiratoryPeak:
                    if (tmp > _flowMeter)
                    {
                        _flowMeter = tmp;

                        if (_flowMeter > _tmpCapacities.RawExpPeakFlow)
                            _tmpCapacities.ExpPeakFlow = _flowMeter;
                    }
                    break;

                case CalibrationExerciseMano.InspiratoryPeak:
                    if (tmp < _flowMeter)
                    {
                        _flowMeter = tmp;

                        if (_flowMeter < _tmpCapacities.RawInsPeakFlow)
                            _tmpCapacities.InsPeakFlow = _flowMeter;
                    }
                    break;

                case CalibrationExerciseMano.ExpiratoryDuration:
                case CalibrationExerciseMano.InspiratoryDuration:
                    _flowMeter = tmp;
                    break;

                // case CalibrationExerciseMano.RespiratoryFrequency:
                //     if (_flowWatch.IsRunning)
                //         _capturedSamples.Add(_flowWatch.ElapsedMilliseconds, tmp);
                //     break;
            }
        }
    }
}