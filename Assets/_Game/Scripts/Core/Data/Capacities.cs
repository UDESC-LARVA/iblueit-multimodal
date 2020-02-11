using Ibit.Core.Game;

namespace Ibit.Core.Data
{
    public class Capacities
    {
        public float RawInsPeakFlow { get; private set; }
        public float RawExpPeakFlow { get; private set; }
        public float RawInsFlowDuration { get; private set; }
        public float RawExpFlowDuration { get; private set; }
        public float RawRespRate { get; private set; }

        public float InsPeakFlow
        {
            get { return RawInsPeakFlow * GameManager.CapacityMultiplier; }
            set { RawInsPeakFlow = value; }
        }

        public float ExpPeakFlow
        {
            get { return RawExpPeakFlow * GameManager.CapacityMultiplier; }
            set { RawExpPeakFlow = value; }
        }

        /// <summary>
        /// Inspiratory Flow Time (in milliseconds)
        /// </summary>
        public float InsFlowDuration
        {
            get { return RawInsFlowDuration * GameManager.CapacityMultiplier; }
            set { RawInsFlowDuration = value; }
        }

        /// <summary>
        /// Expiratory Flow Time (in milliseconds)
        /// </summary>
        public float ExpFlowDuration
        {
            get { return RawExpFlowDuration * GameManager.CapacityMultiplier; }
            set { RawExpFlowDuration = value; }
        }

        /// <summary>
        /// Respiration Rate (breaths per second)
        /// </summary>
        public float RespiratoryRate
        {
            get { return RawRespRate / GameManager.CapacityMultiplier; }
            set { RawRespRate = value; }
        }

        public void Reset()
        {
            RawInsPeakFlow = 0f;
            RawExpPeakFlow = 0f;
            RawInsFlowDuration = 0f;
            RawExpFlowDuration = 0f;
            RawRespRate = 0f;
        }
    }
}