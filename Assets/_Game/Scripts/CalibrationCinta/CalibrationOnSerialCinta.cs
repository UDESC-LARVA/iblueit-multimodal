using Ibit.Core.Util;
using UnityEngine;

namespace Ibit.Calibration
{
    public partial class CalibrationManagerCinta
    {
        private void OnSerialMessageReceived(string msg)
        {
            if (!_acceptingValues || msg.Length < 1)
                return;

            var tmp = Parsers.Float(msg);

            switch (_currentExercise)
            {
                case CalibrationExerciseCinta.ExpiratoryPeak:
                    if (tmp > _flowMeter)
                    {
                        _flowMeter = tmp;

                        if (_flowMeter > _tmpCapacities.RawExpPeakFlow)
                            _tmpCapacities.ExpPeakFlow = _flowMeter;
                    }
                    break;

                case CalibrationExerciseCinta.InspiratoryPeak:
                    if (tmp < _flowMeter)
                    {
                        _flowMeter = tmp;

                        if (_flowMeter < _tmpCapacities.RawInsPeakFlow)
                            _tmpCapacities.InsPeakFlow = _flowMeter;
                    }
                    break;

                case CalibrationExerciseCinta.ExpiratoryDuration:
                case CalibrationExerciseCinta.InspiratoryDuration:
                    _flowMeter = tmp;
                    break;

                case CalibrationExerciseCinta.RespiratoryFrequency:
                    if (_flowWatch.IsRunning)
                        _capturedSamples.Add(_flowWatch.ElapsedMilliseconds, tmp);
                    break;
            }
        }
    }
}