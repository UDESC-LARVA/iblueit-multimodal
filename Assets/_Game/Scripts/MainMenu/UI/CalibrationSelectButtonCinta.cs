using Ibit.Calibration;
using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class CalibrationSelectButtonCinta : MonoBehaviour
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
                case CalibrationExerciseCinta.RespiratoryFrequency:
                    if (Pacient.Loaded.CapacitiesCinta.RespiratoryRate != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExerciseCinta.InspiratoryPeak:
                    if (Pacient.Loaded.CapacitiesCinta.InsPeakFlow != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExerciseCinta.InspiratoryDuration:
                    if (Pacient.Loaded.CapacitiesCinta.InsFlowDuration != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExerciseCinta.ExpiratoryPeak:
                    if (Pacient.Loaded.CapacitiesCinta.ExpPeakFlow != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExerciseCinta.ExpiratoryDuration:
                    if (Pacient.Loaded.CapacitiesCinta.ExpFlowDuration != 0)
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