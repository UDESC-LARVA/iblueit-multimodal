using System.ComponentModel;

namespace Ibit.Core.Data.Enums
{
    public enum RespiratoryExercise
    {
        [Description("RespiratoryFrequency")] RespiratoryFrequency = 1,
        [Description("InspiratoryPeak")] InspiratoryPeak = 2,
        [Description("InspiratoryDuration")] InspiratoryDuration = 3,
        [Description("ExpiratoryPeak")] ExpiratoryPeak = 4,
        [Description("ExpiratoryDuration")] ExpiratoryDuration = 5
    }
}