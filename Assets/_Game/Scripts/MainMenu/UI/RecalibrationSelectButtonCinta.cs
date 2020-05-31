using Ibit.Calibration;
using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class RecalibrationSelectButtonCinta : MonoBehaviour
    {
        [SerializeField] private CalibrationExerciseCinta _calibrationToLoad;
        private string _originalLabel;
        private const string _checkmark = "  ✓";

        private void Awake()
        {
            _originalLabel = GetComponentInChildren<Text>().text;
            this.GetComponent<Button>().onClick.AddListener(SetExercise);
        }

        private void OnEnable()
        {
            switch (_calibrationToLoad)
            {
                case CalibrationExerciseCinta.InspiratoryPeak:
                    if (Pacient.Loaded.CapacitiesCinta.RawInsPeakFlow != 0 && CalibrationManagerCinta.RecentlyCalibrated_InsPeak)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExerciseCinta.ExpiratoryPeak:
                    if (Pacient.Loaded.CapacitiesCinta.RawExpPeakFlow != 0 && CalibrationManagerCinta.RecentlyCalibrated_ExpPeak)
                    {
                        CheckExercise();
                    }
                    break;
            }
        }

        private void OnDisable()
        {
            GetComponentInChildren<Text>().text = _originalLabel;
        }

        private void CheckExercise()
        {
            GetComponentInChildren<Text>().text += _checkmark;
        }

        private void SetExercise()
        {
            CalibrationManagerCinta.CalibrationToLoad = _calibrationToLoad;
        }
    }
}