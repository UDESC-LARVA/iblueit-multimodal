using Ibit.Core.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ibit.CakeGame
{
    public class ScoreMenu : MonoBehaviour
    {
        public Stars[] finalScore = new Stars[3];
        public Text[] peakText = new Text[3];
        public string[] pikeString = new string[3];

        public void DisplayFinalScore(int score1, int score2, int score3)
        {
            finalScore[0].FillStarsFinal(score1);
            finalScore[1].FillStarsFinal(score2);
            finalScore[2].FillStarsFinal(score3);
            peakText[0].text = "   Pico: " + FlowMath.ToLitresPerMinute(Parsers.Float(pikeString[0])) + " L/min";
            peakText[1].text = "   Pico: " + FlowMath.ToLitresPerMinute(Parsers.Float(pikeString[1])) + " L/min";
            peakText[2].text = "   Pico: " + FlowMath.ToLitresPerMinute(Parsers.Float(pikeString[2])) + " L/min";
        }

        public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        public void ToggleScoreMenu() => gameObject.SetActive(true);

        private void Start() => gameObject.SetActive(false);
    }
}