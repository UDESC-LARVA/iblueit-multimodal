using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets._Game.Scripts.Core.Api.Dto;
using Assets._Game.Scripts.Core.Api.Extensions;
using Ibit.Core.Audio;
using Ibit.Core.Data;
using Ibit.Core.Data.Enums;
using Ibit.Core.Data.Manager;
using Ibit.Core.Serial;
using Ibit.Core.Util;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.Calibration
{
    public partial class CalibrationManagerCinta : MonoBehaviour
    {
        public static CalibrationExerciseCinta CalibrationToLoad = 0;

        [BoxGroup("Controls")][SerializeField] private CalibrationExerciseCinta _currentExercise;
        [BoxGroup("Controls")][SerializeField] private int FlowTimeThreshold = 2000; //ms
        [BoxGroup("Controls")][SerializeField] private float RespiratoryFrequencyThreshold = 0.05f; //s
        [BoxGroup("Controls")][SerializeField] private int TimerRespFreq = 60; //seg
        [BoxGroup("Controls")][SerializeField] private int TimerPeakExercise = 8; //seg        
        [BoxGroup("Controls")][SerializeField] private int _currentStep = 1; //default: 1 
        [BoxGroup("Screen Objects")][SerializeField] private GameObject _clockObject;
        [BoxGroup("Screen Objects")][SerializeField] private GameObject _dudeObject;
        [BoxGroup("UI")][SerializeField] private Text _dialogText;
        [BoxGroup("UI")][SerializeField] private Text _exerciseCountText;
        [BoxGroup("UI")][SerializeField] private Text _timerText;
        [BoxGroup("UI")][SerializeField] private GameObject _enterButton;

        private bool _acceptingValues;
        private bool _calibrationDone;
        private bool _runStep;
        private int _currentExerciseCount;
        private float _flowMeter;
        private Stopwatch _flowWatch;
        private Stopwatch _timerWatch;
        private Capacities _tmpCapacities;
        private CalibrationOverviewSendDto _calibrationOverviewSendDto;
        private CalibrationLoggerCinta _calibrationLogger;
        private Dictionary<float, float> _capturedSamples;
        private SerialControllerCinta _serialController;

        public static bool RecentlyCalibrated_InsPeak = false;
        public static bool RecentlyCalibrated_ExpPeak = false;

        public static float _maxFlowINS;

        private void Awake()
        {
            _serialController = FindObjectOfType<SerialControllerCinta>();
            _serialController.OnSerialMessageReceived += OnSerialMessageReceived;
            _tmpCapacities = new Capacities();
            _calibrationOverviewSendDto = new CalibrationOverviewSendDto
            {
                GameDevice = GameDevice.Cinta.GetDescription(),
                PacientId = Pacient.Loaded.IdApi
            };
            _flowWatch = new Stopwatch();
            _timerWatch = new Stopwatch();
            _capturedSamples = new Dictionary<float, float>();
            _calibrationLogger = new CalibrationLoggerCinta();

            _dudeObject.transform.Translate(-Camera.main.orthographicSize * Camera.main.aspect + (_dudeObject.transform.localScale.x / 2f), 0f, 0f);

            if (CalibrationToLoad > 0)
                _currentExercise = CalibrationToLoad;
        }

        private void OnDestroy()
        {
            _serialController.OnSerialMessageReceived -= OnSerialMessageReceived;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                SkipStep();

            if (Input.GetKeyDown(KeyCode.F2))
                _serialController.Recalibrate();
        }

        private IEnumerator Start()
        {
            _runStep = true;
            _serialController.Recalibrate();
            UnityEngine.Debug.Log("recalibrando...");

            while (!_calibrationDone)
            {
                if (_runStep)
                {
                    // Clear screen
                    _enterButton.SetActive(false);
                    DudeClearMessage();

                    // Wait to show next step
                    yield return new WaitForSeconds(0.7f);

                    switch (_currentExercise)
                    {

                        case CalibrationExerciseCinta.RespiratoryFrequency:

                            switch (_currentStep)
                            {
                                case 1:
                                    DudeTalk("Manipule a Cinta através da respiração. Pressione (Enter) para continuar.");
                                    SetupNextStep();
                                    break;

                                case 2:
                                    DudeTalk($"Neste exercício, você deve RESPIRAR NORMALMENTE por {TimerRespFreq} segundos. Ao apertar (Enter), o relógio ficará verde para você começar o exercício.");
                                    SetupNextStep();
                                    break;

                                case 3:
                                    if (!_serialController.IsConnected)
                                    {
                                        SerialDisconnectedWarning();
                                        continue;
                                    }

                                    _capturedSamples.Clear();
                                    _serialController.StartSampling();

                                    yield return new WaitForSeconds(1f);
                                    _dialogText.text = "(relaxe e RESPIRE NORMALMENTE)";

                                    AirFlowEnable();

                                    StartCoroutine(DisplayCountdown(TimerRespFreq));
                                    while (_flowWatch.ElapsedMilliseconds < TimerRespFreq * 1000) 
                                        yield return null;

                                    AirFlowDisable();

                                    _flowMeter = CintaFlowMath.RespiratoryRate(_capturedSamples, TimerRespFreq);

                                    if (_flowMeter > RespiratoryFrequencyThreshold)
                                    {
                                        _calibrationLogger.Write(CalibrationExerciseResultCinta.Success, _currentExercise, _flowMeter);
                                        _tmpCapacities.RespiratoryRate = _flowMeter;
                                        SetupNextStep(true);
                                        continue;
                                    }
                                    else
                                    {
                                        _calibrationLogger.Write(CalibrationExerciseResultCinta.Failure, _currentExercise, _flowMeter);
                                        DudeWarnUnknownFlow();
                                        SetupStep(_currentStep);
                                        break;
                                    }

                                case 4:
                                    SoundManager.Instance.PlaySound("Success");
                                    DudeTalk($"Sua média de frequência respiratória é de {(_tmpCapacities.RawRespRate * 60f):F} resp/min." +
                                        " Pressione (Enter) para continuar com os outros exercícios.");
                                    SetupNextStep();
                                    break;

                                case 5:
                                    _calibrationOverviewSendDto.CalibrationValue = _tmpCapacities.RawRespRate;
                                    _calibrationOverviewSendDto.Exercise = RespiratoryExercise.RespiratoryFrequency.GetDescription();
                                    Pacient.Loaded.CapacitiesCinta.RespiratoryRate = _tmpCapacities.RawRespRate;
                                    SaveAndQuit();
                                    break;

                                default:
                                    FindObjectOfType<SceneLoader>().LoadScene(0);
                                    break;
                            }
                            break;

                        case CalibrationExerciseCinta.InspiratoryPeak:
                            switch (_currentStep)
                            {
                                case 1:
                                    DudeTalk("Neste exercício, você deve PUXAR O AR COM FORÇA dilatando peito e abdome. Serão 3 tentativas. Ao apertar (Enter), o relógio ficará verde para você começar o exercício.");
                                    _serialController.Recalibrate();
                                    SetupNextStep();
                                    break;

                                case 2:
                                    if (!_serialController.IsConnected)
                                    {
                                        SerialDisconnectedWarning();
                                        continue;
                                    }

                                    _serialController.StartSampling();

                                    _exerciseCountText.text = $"Exercício: {_currentExerciseCount + 1}/3";
                                    yield return new WaitForSeconds(1f);

                                    AirFlowEnable();
                                    StartCoroutine(DisplayCountdown(TimerPeakExercise));
                                    _dialogText.text = "(PUXE O AR COM FORÇA 1 vez! E aguarde o próximo passo...)";

                                    yield return new WaitForSeconds(TimerPeakExercise);

                                    AirFlowDisable();

                                    var insCheck = _flowMeter;
                                    _maxFlowINS = insCheck;
                                    
                                    ResetFlowMeter();

                                    if (insCheck < -Pacient.Loaded.CintaThreshold)
                                    {
                                        _calibrationLogger.Write(CalibrationExerciseResultCinta.Success, _currentExercise, insCheck);
                                        _currentExerciseCount++;
                                        SetupNextStep(true);
                                        continue;
                                    }
                                    else
                                    {
                                        _calibrationLogger.Write(CalibrationExerciseResultCinta.Failure, _currentExercise, insCheck);
                                        DudeWarnUnknownFlow();
                                        SetupStep(_currentStep);
                                        break;
                                    }

                                case 3:
                                    DudeCongratulate();
                                    SetupStep(_currentExerciseCount == 3 ? _currentStep + 1 : _currentStep - 1);
                                    break;

                                case 4:
                                    SoundManager.Instance.PlaySound("Success");

                                    _tmpCapacities.InsPeakFlow = _tmpCapacities.RawInsPeakFlow/2;

                                    DudeTalk($"Seu pico inspiratório é de {CintaFlowMath.ToLitresPerMinute(_tmpCapacities.RawInsPeakFlow/2):F} L/min." +
                                        " Pressione (Enter) para continuar com os outros exercícios.");
                                        
                                    SetupNextStep();
                                    break;

                                case 5:
                                    _calibrationOverviewSendDto.CalibrationValue = _tmpCapacities.RawInsPeakFlow/2;
                                    _calibrationOverviewSendDto.Exercise = RespiratoryExercise.InspiratoryPeak.GetDescription();
                                    Pacient.Loaded.CapacitiesCinta.InsPeakFlow = _tmpCapacities.RawInsPeakFlow/2;
                                    SaveAndQuit();
                                    RecentlyCalibrated_InsPeak = true;
                                    break;

                                default:
                                    FindObjectOfType<SceneLoader>().LoadScene(0);
                                    break;
                            }

                            break;

                        case CalibrationExerciseCinta.ExpiratoryPeak:

                            switch (_currentStep)
                            {
                                case 1:
                                    DudeTalk("Neste exercício, você deve ASSOPRAR FORTE contraindo peito e abdome. Serão 3 tentativas. Ao apertar (Enter), o relógio ficará verde para você começar o exercício.");
                                    _serialController.Recalibrate();
                                    SetupNextStep();
                                    break;

                                case 2:
                                    if (!_serialController.IsConnected)
                                    {
                                        SerialDisconnectedWarning();
                                        continue;
                                    }

                                    _serialController.StartSampling();

                                    _exerciseCountText.text = $"Exercício: {_currentExerciseCount + 1}/3";
                                    yield return new WaitForSeconds(1);

                                    AirFlowEnable();
                                    StartCoroutine(DisplayCountdown(TimerPeakExercise));
                                    _dialogText.text = "(ASSOPRE FORTE 1 vez! E aguarde o próximo passo...)";

                                    // Wait for player input
                                    yield return new WaitForSeconds(TimerPeakExercise);

                                    AirFlowDisable();

                                    var expCheck = _flowMeter; //_tmpCapacities.RawInsPeakFlow-_flowMeter
                                    
                                    ResetFlowMeter();

                                    if (expCheck > Pacient.Loaded.CintaThreshold)
                                    {
                                        _calibrationLogger.Write(CalibrationExerciseResultCinta.Success, _currentExercise, expCheck);
                                        _currentExerciseCount++;
                                        SetupNextStep(true);
                                        continue;
                                    }
                                    else
                                    {
                                        _calibrationLogger.Write(CalibrationExerciseResultCinta.Failure, _currentExercise, expCheck);
                                        DudeWarnUnknownFlow();
                                        SetupStep(_currentStep);
                                        break;
                                    }

                                case 3:
                                    DudeCongratulate();
                                    SetupStep(_currentExerciseCount == 3 ? _currentStep + 1 : _currentStep - 1);
                                    break;

                                case 4:
                                    SoundManager.Instance.PlaySound("Success");

                                    _tmpCapacities.ExpPeakFlow = _tmpCapacities.RawExpPeakFlow/2;

                                    DudeTalk($"Seu pico expiratório é de {CintaFlowMath.ToLitresPerMinute(_tmpCapacities.RawExpPeakFlow/2):F} L/min." +
                                        " Pressione (Enter) para continuar.");
                                    
                                    SetupNextStep();
                                    break;

                                case 5:
                                    _calibrationOverviewSendDto.CalibrationValue = _tmpCapacities.RawExpPeakFlow/2;
                                    _calibrationOverviewSendDto.Exercise = RespiratoryExercise.ExpiratoryPeak.GetDescription();
                                    Pacient.Loaded.CapacitiesCinta.ExpPeakFlow = _tmpCapacities.RawExpPeakFlow/2;
                                    SaveAndQuit();
                                    RecentlyCalibrated_ExpPeak = true;
                                    break;

                                default:
                                    FindObjectOfType<SceneLoader>().LoadScene(0);
                                    break;
                            }

                            break;

                        case CalibrationExerciseCinta.InspiratoryDuration:

                            switch (_currentStep)
                            {
                                case 1:
                                    DudeTalk("Neste exercício, MANTENHA o ponteiro GIRANDO, PUXANDO O AR! Serão 3 tentativas. Ao apertar (Enter), o relógio ficará verde para você começar o exercício.");
                                    SetupNextStep();
                                    break;

                                case 2:
                                    if (!_serialController.IsConnected)
                                    {
                                        SerialDisconnectedWarning();
                                        continue;
                                    }

                                    _serialController.StartSampling();

                                    _exerciseCountText.text = $"Exercício: {_currentExerciseCount + 1}/3";
                                    yield return new WaitForSeconds(2);

                                    AirFlowEnable(false);
                                    _dialogText.text = "(MANTENHA o ponteiro GIRANDO, PUXANDO O AR!)";

                                    var tmpThreshold = Pacient.Loaded.CintaThreshold;
                                    Pacient.Loaded.CintaThreshold = tmpThreshold * 0.25f;

                                    while (_flowMeter >= -Pacient.Loaded.CintaThreshold)
                                        yield return null;

                                    _flowWatch.Restart();

                                    while (_flowMeter < -Pacient.Loaded.CintaThreshold)
                                        yield return null;

                                    AirFlowDisable();
                                    ResetFlowMeter();
                                    Pacient.Loaded.CintaThreshold = tmpThreshold;

                                    // Validate for player input
                                    if (_flowWatch.ElapsedMilliseconds > FlowTimeThreshold)
                                    {
                                        _calibrationLogger.Write(CalibrationExerciseResultCinta.Success, _currentExercise, _flowWatch.ElapsedMilliseconds);

                                        if (_flowWatch.ElapsedMilliseconds > _tmpCapacities.InsFlowDuration)
                                            _tmpCapacities.InsFlowDuration = _flowWatch.ElapsedMilliseconds;

                                        _currentExerciseCount++;

                                        SetupNextStep(true);
                                        continue;
                                    }
                                    else
                                    {
                                        _calibrationLogger.Write(CalibrationExerciseResultCinta.Failure, _currentExercise, _flowWatch.ElapsedMilliseconds);
                                        DudeWarnUnknownFlow();
                                        SetupStep(_currentStep);
                                        break;
                                    }

                                case 3:
                                    DudeCongratulate();
                                    SetupStep(_currentExerciseCount == 3 ? _currentStep + 1 : _currentStep - 1);
                                    break;

                                case 4:
                                    SoundManager.Instance.PlaySound("Success");
                                    DudeTalk($"Seu tempo de inspiração máximo é de {(_tmpCapacities.RawInsFlowDuration / 1000f):F} segundos." +
                                        " Pressione (Enter) para continuar com os outros exercícios.");
                                    SetupNextStep();
                                    break;

                                case 5:
                                    _calibrationOverviewSendDto.CalibrationValue = _tmpCapacities.RawInsFlowDuration;
                                    _calibrationOverviewSendDto.Exercise = RespiratoryExercise.InspiratoryDuration.GetDescription();
                                    Pacient.Loaded.CapacitiesCinta.InsFlowDuration = _tmpCapacities.RawInsFlowDuration;
                                    SaveAndQuit();
                                    break;

                                default:
                                    FindObjectOfType<SceneLoader>().LoadScene(0);
                                    break;
                            }

                            break;

                        case CalibrationExerciseCinta.ExpiratoryDuration:

                            switch (_currentStep)
                            {
                                case 1:
                                    DudeTalk("Neste exercício, MANTENHA o ponteiro GIRANDO, ASSOPRANDO! Serão 3 tentativas. Ao apertar (Enter), o relógio ficará verde para você começar o exercício.");
                                    SetupNextStep();
                                    break;

                                case 2:
                                    if (!_serialController.IsConnected)
                                    {
                                        SerialDisconnectedWarning();
                                        continue;
                                    }

                                    _serialController.StartSampling();

                                    _exerciseCountText.text = $"Exercício: {_currentExerciseCount + 1}/3";
                                    yield return new WaitForSeconds(2);

                                    AirFlowEnable(false);
                                    _dialogText.text = "(MANTENHA o ponteiro GIRANDO, ASSOPRANDO!)";

                                    var tmpThreshold = Pacient.Loaded.CintaThreshold;
                                    Pacient.Loaded.CintaThreshold = tmpThreshold * 0.25f; //this helps the player expel all his air

                                    // Wait for player input to be greather than threshold
                                    while (_flowMeter <= Pacient.Loaded.CintaThreshold)
                                        yield return null;

                                    _flowWatch.Restart();

                                    while (_flowMeter > Pacient.Loaded.CintaThreshold)
                                        yield return null;

                                    AirFlowDisable();
                                    ResetFlowMeter();

                                    Pacient.Loaded.CintaThreshold = tmpThreshold;

                                    // Validate for player input
                                    if (_flowWatch.ElapsedMilliseconds > FlowTimeThreshold)
                                    {
                                        _calibrationLogger.Write(CalibrationExerciseResultCinta.Success, _currentExercise, _flowWatch.ElapsedMilliseconds);

                                        if (_flowWatch.ElapsedMilliseconds > _tmpCapacities.ExpFlowDuration)
                                            _tmpCapacities.ExpFlowDuration = _flowWatch.ElapsedMilliseconds;

                                        _currentExerciseCount++;
                                        SetupNextStep(true);
                                        continue;
                                    }
                                    else
                                    {
                                        _calibrationLogger.Write(CalibrationExerciseResultCinta.Failure, _currentExercise, _flowWatch.ElapsedMilliseconds);
                                        DudeWarnUnknownFlow();
                                        SetupStep(_currentStep);
                                        break;
                                    }

                                case 3:
                                    DudeCongratulate();
                                    SetupStep(_currentExerciseCount == 3 ? _currentStep + 1 : _currentStep - 1);
                                    break;

                                case 4:
                                    SoundManager.Instance.PlaySound("Success");
                                    DudeTalk($"Seu tempo de fluxo expiratório máximo é de {(_tmpCapacities.RawExpFlowDuration / 1000f):F} segundos." +
                                        " Pressione (Enter) para continuar com os outros exercícios.");
                                    SetupNextStep();
                                    break;

                                case 5:
                                    _calibrationOverviewSendDto.CalibrationValue = _tmpCapacities.RawExpFlowDuration;
                                    _calibrationOverviewSendDto.Exercise = RespiratoryExercise.ExpiratoryDuration.GetDescription();
                                    Pacient.Loaded.CapacitiesCinta.ExpFlowDuration = _tmpCapacities.RawExpFlowDuration;
                                    SaveAndQuit();
                                    break;

                                default:
                                    ReturnToMainMenu();
                                    break;
                            }

                            break;
                    }

                    _enterButton.SetActive(true);
                    _runStep = false;
                }

                yield return null;
            }
        }

        private IEnumerator DisplayCountdown(long timer)
        {
            timer *= 1000;
            _timerWatch.Restart();

            while (_timerWatch.ElapsedMilliseconds < timer)
            {
                yield return null;
                _timerText.text = $"TIMER: {(timer - _timerWatch.ElapsedMilliseconds) / 1000}";
            }

            _timerText.text = "";
        }

        private void ResetFlowMeter()
        {
            _flowMeter = 0f;
        }

        private void AirFlowDisable()
        {
            _flowWatch.Stop();
            _clockObject.GetComponent<SpriteRenderer>().color = Color.white;
            _clockObject.GetComponentInChildren<ClockArrowAnimationCinta>().SpinClock = false;
            _acceptingValues = false;
        }

        private void AirFlowEnable(bool restartWatch = true)
        {
            if (restartWatch)
                _flowWatch.Restart();

            _clockObject.GetComponent<SpriteRenderer>().color = Color.green;
            _clockObject.GetComponentInChildren<ClockArrowAnimationCinta>().SpinClock = true;
            _clockObject.GetComponentInChildren<ClockArrowAnimationCinta>().CurrentExercise = _currentExercise.ToString();
            _acceptingValues = true;
        }

        private async void SaveAndQuit()
        {
            GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(true);

            Pacient.Loaded.CalibrationCintaDone = Pacient.Loaded.IsCalibrationCintaDone;

            var pacientSendDto = Pacient.MapToPacientSendDto();
            var responsePacient = await DataManager.Instance.UpdatePacient(pacientSendDto);

            var responseCalibration = await DataManager.Instance.SaveCalibrationOverview(_calibrationOverviewSendDto);

            GameObject.Find("Canvas").transform.Find("SavingBgPanel").gameObject.SetActive(false);

            if (responsePacient.ApiResponse == null)
                SysMessage.Info("Erro ao atualizar os dados do paciente na nuvem!\n Os dados poderão ser enviados posteriormente..");

            if (responseCalibration.ApiResponse == null)
                SysMessage.Info("Erro salvar os dados de calibração na nuvem!\n Os dados poderão ser enviados posteriormente..");

            FindObjectOfType<CintaLogger>().StopLogging();
            ReturnToMainMenu();
        }

        private void ReturnToMainMenu()
        {
            if (_currentExercise == CalibrationExerciseCinta.InspiratoryPeak) // Se estiver no INSPeak vai direto para EXPPeak
            {
                CalibrationToLoad = CalibrationExerciseCinta.ExpiratoryPeak;
                FindObjectOfType<SceneLoader>().LoadScene(4); // Scene 4 é a Calibração Cinta
            } else {
                FindObjectOfType<SceneLoader>().LoadScene(0);
            }
        }

        private void SerialDisconnectedWarning()
        {
            _enterButton.SetActive(true);
            DudeWarnCintaDisconnected();
            SetupStep(99);
        }
    }
}