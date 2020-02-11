using Ibit.Calibration;
using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class CalibrationSelectButtonMano : MonoBehaviour
    {
        [SerializeField] private CalibrationExerciseMano _calibrationToLoad;
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
                // case CalibrationExerciseMano.RespiratoryFrequency:
                //     if (Pacient.Loaded.CapacitiesMano.RespiratoryRate != 0)
                //     {
                //         CheckExercise();
                //     }
                //     break;
                case CalibrationExerciseMano.InspiratoryPeak:
                    if (Pacient.Loaded.CapacitiesMano.InsPeakFlow != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExerciseMano.InspiratoryDuration:
                    if (Pacient.Loaded.CapacitiesMano.InsFlowDuration != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExerciseMano.ExpiratoryPeak:
                    if (Pacient.Loaded.CapacitiesMano.ExpPeakFlow != 0)
                    {
                        CheckExercise();
                    }
                    break;
                case CalibrationExerciseMano.ExpiratoryDuration:
                    if (Pacient.Loaded.CapacitiesMano.ExpFlowDuration != 0)
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
            CalibrationManagerMano.CalibrationToLoad = _calibrationToLoad;
        }
    }
}