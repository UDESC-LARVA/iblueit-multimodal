using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.MainMenu.UI
{
    public class HowToPlayUI : MonoBehaviour
    {
        [SerializeField] private Sprite targetTutorial;
        [SerializeField] private Sprite relaxTutorial;
        [SerializeField] private Sprite obstacleTutorial;
        [SerializeField] private Image imageHolder;
        [SerializeField] private Button okButton;

        private void OnEnable()
        {
            if (Pacient.Loaded.HowToPlayDone)
            {
                HidePanel();
            }
            else
            {
                // Unlocked Obstacles Stages
                if (Pacient.Loaded.UnlockedLevels == 6)
                {
                    imageHolder.sprite = obstacleTutorial;
                    okButton.onClick.AddListener(PacientReady);
                }
                else
                {
                    // First Time Playing
                    if (Pacient.Loaded.UnlockedLevels == 1)
                    {
                        ShowPanel();
                    }
                    else
                    {
                        HidePanel();
                    }
                }
            }
        }

        public void SwitchImage()
        {
            imageHolder.sprite = relaxTutorial;
            okButton.onClick.AddListener(ResetAndHidePanel);
        }

        private void PacientReady()
        {
            Pacient.Loaded.HowToPlayDone = true;
            HidePanel();
        }

        private void ShowPanel()
        {
            this.transform.localScale = Vector3.one;
        }

        private void HidePanel()
        {
            this.transform.localScale = Vector3.zero;
        }

        private void ResetAndHidePanel()
        {
            imageHolder.sprite = targetTutorial;
            HidePanel();
            okButton.onClick.RemoveListener(ResetAndHidePanel);
        }
    }
}