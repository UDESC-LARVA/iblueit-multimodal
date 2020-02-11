using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Ibit.LeavesGame
{
    public class FinalScorer : MonoBehaviour
    {
        //public Image[] starsFinal_UI = new Image[3];
        //public Sprite[] starsFilled = new Sprite[3];
        public Text[] pikeString = new Text[3];

        private void Start()
        {
            this.transform.parent.gameObject.SetActive(false);
        }

        public void ChangeFinalScore(int[] roundScores)
        {
            pikeString[0].text = "Folhas: " + roundScores[0].ToString();
            pikeString[1].text = "Folhas: " + roundScores[1].ToString();
            pikeString[2].text = "Folhas: " + roundScores[2].ToString();
        }
        
        //public void ChangeFinalRoundScore(int starsQty, int roundNumber)
        //{
        //    //Debug.Log(starsQty + " " + roundNumber);
        //    starsFinal_UI[roundNumber].sprite = starsFilled[starsQty - 1];
        //}

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