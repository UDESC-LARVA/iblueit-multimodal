using Ibit.Core.Audio;
using Ibit.Core.Game;
using Ibit.Core.Serial;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Spawn;
using Ibit.Plataform.UI;
using UnityEngine;

namespace Ibit.Plataform
{
    public class OnTheFlyInputs : MonoBehaviour
    {
        [SerializeField] private GameObject _helpPanel;
        [SerializeField] private CanvasManager _canvasManager;

        private void Start()
        {
            if (_canvasManager == null)
                _canvasManager = FindObjectOfType<CanvasManager>();
        }

        private void Update()
        {
            /* RENATO:
             * ToDo - A bug occurs when you press the GUI pause button and then unpause with space/esc.
             * The pause function on the space/esc keeps calling pause() and unpause() at the same time.
             * Maybe it can be solved by switching all GameManager.GameIsPaused references to pause events in GameManager. */

            




            // ESC - SPACE
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
            {
                if (!GameManager.GameIsPaused)
                    _canvasManager.PauseGame();
                else
                    _canvasManager.UnPauseGame();
            }

            // F1
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ShowHelp();
            }

            // F2
            if (Input.GetKeyDown(KeyCode.F2))
            {
                FindObjectOfType<SerialControllerPitaco>().Recalibrate();
                FindObjectOfType<SerialControllerMano>().Recalibrate();
                FindObjectOfType<SerialControllerCinta>().Recalibrate();
            }

            // S
            if (Input.GetKeyDown(KeyCode.S))
            {
                ToggleSound();
            }

            // T
            if (Input.GetKeyDown(KeyCode.T))
            {
                ChangeMusic();
            }

            // +
            if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                IncreaseGamingFactors();
            }

            // -
            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                DecreaseGamingFactors();
            }

            // ←
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                IncreaseSpeedFactor();
            }

            // →
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                DecreaseSpeedFactor();
            }
        }

        private void ShowHelp()
        {
            _helpPanel.SetActive(!_helpPanel.activeSelf);
        }

        private void ToggleSound()
        {
            if (AudioListener.volume > 0f)
                AudioListener.volume = 0f;
            else
                AudioListener.volume = 0.8f;
        }

        private void ChangeMusic()
        {
            SoundManager.Instance.PlayAnotherBgm();
        }

        private void IncreaseGamingFactors()
        {
            var spwn = FindObjectOfType<Spawner>();
            spwn.IncrementExpHeight();
            spwn.IncrementExpSize();
            spwn.IncrementInsHeight();
            spwn.IncrementInsSize();
        }

        private void DecreaseGamingFactors()
        {
            var spwn = FindObjectOfType<Spawner>();
            spwn.DecrementExpHeight();
            spwn.DecrementExpSize();
            spwn.DecrementInsHeight();
            spwn.DecrementInsSize();
        }

        private void IncreaseSpeedFactor()
        {
            StageModel.Loaded.ObjectSpeedFactor *= 1.05f;

            foreach (var obj in FindObjectOfType<Spawner>().SpawnedObjects)
            {
                obj.GetComponent<MoveObject>().Speed *= 1.05f;
            }
        }

        private void DecreaseSpeedFactor()
        {
            StageModel.Loaded.ObjectSpeedFactor *= 0.95f;

            foreach (var obj in FindObjectOfType<Spawner>().SpawnedObjects)
            {
                obj.GetComponent<MoveObject>().Speed *= 0.95f;
            }
        }
    }
}