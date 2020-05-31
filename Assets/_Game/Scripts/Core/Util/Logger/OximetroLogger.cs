using System;
using Ibit.Core.Data;
using Ibit.Core.Game;
using Ibit.Core.Serial;
using System.Linq;
using Assets._Game.Scripts.Core.Api.Extensions;
using Ibit.Core.Data.Enums;
using UnityEngine;

namespace Ibit.Core.Util
{
    public class OximetroLogger : Logger<OximetroLogger>
    {
        public FlowDataDevice flowDataDevice;

        protected override void Awake()
        {
            flowDataDevice = new FlowDataDevice {DeviceName = GameDevice.Oximetro.GetDescription()};

            sb.AppendLine("time;value");
            FindObjectOfType<SerialControllerOximetro>().OnSerialMessageReceived += OnSerialMessageReceived;
        }

        protected override void Save()
        {
            var textData = sb.ToString();

            if (textData.Count(s => s == '\n') < 2)
                return;

            var path = @"savedata/pacients/" + Pacient.Loaded.Id + @"/" + $"{recordStart:yyyyMMdd-HHmmss}_" + FileName + ".csv";
            FileManager.WriteAllText(path, textData);
        }

        private void OnSerialMessageReceived(string msg)
        {
            if (!isLogging || msg.Length < 1 || GameManager.GameIsPaused)
                return;

            flowDataDevice.FlowData.Add(new FlowData
            {
                Date = DateTime.Now,
                Value = Parsers.Float(msg)
            });

            sb.AppendLine($"{Time.time:F};{Parsers.Float(msg):F}");
        }
    }
}