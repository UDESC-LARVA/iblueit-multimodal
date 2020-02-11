using Ibit.Core.Util;
using UnityEngine;

namespace Ibit.Calibration
{
    public partial class CalibrationManagerPitaco
    {
        private void OnSerialMessageReceived(string msg)
        {
            if (!_acceptingValues || msg.Length < 1)
                return;

            var tmp = Parsers.Float(msg);

            switch (_currentExercise)
            {
                case CalibrationExercisePitaco.ExpiratoryPeak:
                    if (tmp > _flowMeter)
                    {
                        _flowMeter = tmp;

                        if (_flowMeter > _tmpCapacities.RawExpPeakFlow)
                            _tmpCapacities.ExpPeakFlow = _flowMeter;
                    }
                    break;

                case CalibrationExercisePitaco.InspiratoryPeak:
                    if (tmp < _flowMeter)
                    {
                        _flowMeter = tmp;

                        if (_flowMeter < _tmpCapacities.RawInsPeakFlow)
                            _tmpCapacities.InsPeakFlow = _flowMeter;
                    }
                    break;

                case CalibrationExercisePitaco.ExpiratoryDuration:
                case CalibrationExercisePitaco.InspiratoryDuration:
                    _flowMeter = tmp;
                    break;

                case CalibrationExercisePitaco.RespiratoryFrequency:
                    if (_flowWatch.IsRunning)
                        _capturedSamples.Add(_flowWatch.ElapsedMilliseconds, tmp);
                    break;
            }
        }
    }
}