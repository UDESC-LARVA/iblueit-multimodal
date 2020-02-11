using Ibit.Core.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ibit.LeavesGame
{

    public class Scorer : MonoBehaviour
    {
        public int[] totalScores;
        public FinalScorer finalScorer;

        //private Image[] starsUI;
        //public Sprite starFilled;
        //public Sprite starUnfilled;
        //public FinalScorer FinalScore;

        public static int scoreValue = 0;
        public Text score;

        void Start()
        {
            score.text = "Pontos: " + scoreValue;
            //FindObjectOfType<Player>().HaveStarEvent += ReceivedStars;
            FindObjectOfType<RoundManager>().ShowFinalScoreEvent += ShowFinalScore;
            FindObjectOfType<RoundManager>().CleanRoundEvent += CleanScore;
            //starsUI = gameObject.GetComponentsInChildren<Image>();
            totalScores = new int[3];
        }

        //Stores the score of the round and change the stars sprites
        //public void ReceivedStars(int roundScore, int roundNumber, float pikeValue)
        //{
        //    Debug.Log(roundNumber);
        //    totalScores[roundNumber] = roundScore;
        //    FinalScore.pikeString[roundNumber].text += $"{FlowMath.ToLitresPerMinute(pikeValue)} L/min ({pikeValue} pa)";

        //    for (int i = 0; i < roundScore; i++)
        //    {
        //        starsUI[i].sprite = starFilled;
        //    }
        //}

        //Clean the stars of the round

        public void PutScore()
        {
            scoreValue++;
            score.text = "Pontos: " + scoreValue;
        }

        public void CleanScore() => scoreValue = 0;
        public void DisableScoreText() => score.text = "";
        public void PutRoundScore(int roundNumber) => totalScores[roundNumber] = scoreValue;

        //Show the final score using the totalScores array.
        public void ShowFinalScore()
        {
            DisableScoreText();
            finalScorer.ChangeFinalScore(totalScores);
            finalScorer.ToggleFinalScore();

            //for (int i = 0; i < totalScores.Length; i++)
            //{
            //    if (totalScores[i] != 0)
            //        finalScorer.ChangeFinalRoundScore(totalScores[i], i);
            //}
        }
    }

}
