using System.Linq;
using Ibit.Core.Database;
using Ibit.Core.Game;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Score;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.Plataform.UI
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private Text _stageLevel;
        [SerializeField] private Text _stagePhase;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _helpPanel;
        [SerializeField] private GameObject _resultPanel;

        private void OnEnable()
        {
            _stageLevel.text = StageModel.Loaded.Level.ToString();
            _stagePhase.text = StageModel.Loaded.Phase.ToString();
        }

        public void PauseGame()
        {
            if (_resultPanel.activeSelf)
                return;

            if (GameManager.GameIsPaused)
                return;

            _helpPanel.SetActive(true);
            _pauseMenu.SetActive(true);
            GameManager.PauseGame();
        }

        public void UnPauseGame()
        {
            if (_resultPanel.activeSelf)
                return;

            if (!GameManager.GameIsPaused)
                return;

            _helpPanel.SetActive(false);
            _pauseMenu.SetActive(false);
            GameManager.UnPauseGame();
        }

        public void SetNextStage()
        {
            StageModel.Loaded = StageDb.Instance.GetStage(
                StageModel.Loaded.Id + 1 > StageDb.Instance.StageList.Max(x => x.Id) ? 1 : StageModel.Loaded.Id + 1);
        }
    }
}