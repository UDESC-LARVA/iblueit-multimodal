namespace Ibit.Calibration
{
    public partial class CalibrationManagerMano
    {

        public void SkipStep()
        {
            _runStep = true;
        }

        private void SetupStep(int step, bool skipStep = false)
        {
            _currentStep = step;
            _runStep = skipStep;
        }

        private void SetupNextStep(bool skipStep = false)
        {
            _currentStep++;
            _runStep = skipStep;
        }
    }
}