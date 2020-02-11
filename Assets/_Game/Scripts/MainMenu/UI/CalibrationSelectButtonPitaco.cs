using Ibit.Calibration;
using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class CalibrationSelectButtonPitaco : MonoBehaviour
    {
        [SerializeField] private CalibrationExercisePitaco _calibrationToLoad;
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
                case CalibrationExercisePitaco.RespiratoryFrequency:
                    if (Pacient.Loaded.CapacitiesPitaco.RespiratoryRate != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExercisePitaco.InspiratoryPeak:
                    if (Pacient.Loaded.CapacitiesPitaco.InsPeakFlow != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExercisePitaco.InspiratoryDuration:
                    if (Pacient.Loaded.CapacitiesPitaco.InsFlowDuration != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExercisePitaco.ExpiratoryPeak:
                    if (Pacient.Loaded.CapacitiesPitaco.ExpPeakFlow != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExercisePitaco.ExpiratoryDuration:
                    if (Pacient.Loaded.CapacitiesPitaco.ExpFlowDuration != 0)
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
            CalibrationManagerPitaco.CalibrationToLoad = _calibrationToLoad;
        }
    }
}