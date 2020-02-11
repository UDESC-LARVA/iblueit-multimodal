using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Ibit.WaterGame
{
    public class FinalScorer : MonoBehaviour
    {
        public Image[] starsFinal_UI = new Image[3];
        public Sprite[] starsFilled = new Sprite[3];
        public Text[] pikeString = new Text[3];

        private void Start()
        {
            this.transform.parent.gameObject.SetActive(false);
        }

        public void ChangeFinalRoundScore(int starsQty, int roundNumber)
        {
            //Debug.Log(starsQty + " " + roundNumber);
            starsFinal_UI[roundNumber].sprite = starsFilled[starsQty - 1];
        }

        public void ToggleFinalScore()
        {
            this.transform.parent.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}