using Ibit.Core.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.WaterGame
{
    public class Scorer : MonoBehaviour
    {
        public delegate void ChangeWaterLevelDelegate(int level);
        public event ChangeWaterLevelDelegate ChangeWaterLevelEvent;

        public int[] totalScores;

        private Image[] starsUI;
        public Sprite starFilled;
        public Sprite starUnfilled;
        public FinalScorer FinalScore;

        private void Start()
        {
            FindObjectOfType<Player>().HaveStarEvent += ReceivedStars;
            FindObjectOfType<RoundManager>().ShowFinalScoreEvent += ShowFinalScore;
            FindObjectOfType<RoundManager>().CleanRoundEvent += CleanScores;
            starsUI = gameObject.GetComponentsInChildren<Image>();
            totalScores = new int[3];
        }

        protected virtual void ChangeWaterLevel(int level)
        {
            ChangeWaterLevelEvent?.Invoke(level);
        }

        //Stores the score of the round and change the stars sprites
        public void ReceivedStars(int roundScore, int roundNumber, float pikeValue)
        {
            Debug.Log(roundNumber);
            totalScores[roundNumber] = roundScore;
            FinalScore.pikeString[roundNumber].text += $"{FlowMath.ToLitresPerMinute(pikeValue)} L/min ({pikeValue} pa)";
            WaterBehaviour(roundScore);

            for (int i = 0; i < roundScore; i++)
            {
                starsUI[i].sprite = starFilled;
            }
        }

        public void WaterBehaviour(int roundScore)
        {
            if (roundScore == 3)
                ChangeWaterLevel(0);//3 STARS

            if (roundScore == 2)
                ChangeWaterLevel(1);//2 STARS

            if (roundScore == 1)
                ChangeWaterLevel(2);//1 STAR

            if (roundScore == 0)
                ChangeWaterLevel(3);//0 STARS
        }

        //Clean the stars of the round
        public void CleanStars()
        {
            foreach (Image starsUi in starsUI)
            {
                starsUi.sprite = starUnfilled;
            }
        }

        public void CleanScores()
        {
            CleanStars();
            ChangeWaterLevel(3);

        }

        //Show the final score using the totalScores array.
        public void ShowFinalScore()
        {
            FinalScore.ToggleFinalScore();

            for (int i = 0; i < totalScores.Length; i++)
            {
                if (totalScores[i] != 0)
                    FinalScore.ChangeFinalRoundScore(totalScores[i], i);
            }
        }
    }
}