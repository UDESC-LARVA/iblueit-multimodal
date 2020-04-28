using System;
using System.IO;
using System.Text;
using Ibit.Core.Data;
using Ibit.Core.Game;
using Ibit.Core.Util;

namespace Ibit.Calibration
{
    public class CalibrationLoggerPitaco
    {
        private StringBuilder _sb;
        private string _pathToSave;

        public CalibrationLoggerPitaco()
        {
            _sb = new StringBuilder();

            _pathToSave = @"savedata/pacients/" + Pacient.Loaded.Id + @"/Calibration-History.csv";

            if (!File.Exists(_pathToSave))
                _sb.AppendLine("dateTime;result;exercise;value");
        }

        public void Write(CalibrationExerciseResultPitaco result, CalibrationExercisePitaco exercise, float value)
        {
            if (exercise == CalibrationExercisePitaco.ExpiratoryPeak || exercise == CalibrationExercisePitaco.InspiratoryPeak)
            {
                _sb.AppendLine($"{DateTime.Now:s};{result};{exercise};{PitacoFlowMath.ToLitresPerMinute(value)};");
            }
            else if (exercise == CalibrationExercisePitaco.RespiratoryFrequency)
            {
                _sb.AppendLine($"{DateTime.Now:s};{result};{exercise};{value * 60f};");
            }
            else
            {
                _sb.AppendLine($"{DateTime.Now:s};{result};{exercise};{value / 1000f};");
            }
        }

        public void Save()
        {
            if (_sb.Length < 0)
                return;

            if (!File.Exists(_pathToSave))
                FileManager.WriteAllText(_pathToSave, _sb.ToString());
            else
                FileManager.AppendAllText(_pathToSave, _sb.ToString());
        }
    }
}