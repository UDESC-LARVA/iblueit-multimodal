using Ibit.Core.Serial;
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

        [SerializeField]
        private SerialControllerPitaco scp;

        [SerializeField]
        private SerialControllerMano scm;

        [SerializeField]
        private SerialControllerCinta scc;

        public void DisplayFinalScore(int score1, int score2, int score3)
        {
            scp = FindObjectOfType<SerialControllerPitaco>();
            scm = FindObjectOfType<SerialControllerMano>();
            scc = FindObjectOfType<SerialControllerCinta>();

            finalScore[0].FillStarsFinal(score1);
            finalScore[1].FillStarsFinal(score2);
            finalScore[2].FillStarsFinal(score3);

            // Se o Pitaco estiver conectado
            if (scp.IsConnected)
            {
                peakText[0].text = "   Pico: " + PitacoFlowMath.ToLitresPerMinute(Parsers.Float(pikeString[0])) + " L/min";
                peakText[1].text = "   Pico: " + PitacoFlowMath.ToLitresPerMinute(Parsers.Float(pikeString[1])) + " L/min";
                peakText[2].text = "   Pico: " + PitacoFlowMath.ToLitresPerMinute(Parsers.Float(pikeString[2])) + " L/min";

            } else {
            // Se o Mano estiver conectado
            if (scm.IsConnected)
            {
                peakText[0].text = "   Pico: " + ManoFlowMath.ToCentimetersofWater(Parsers.Float(pikeString[0])) + " CmH2O";
                peakText[1].text = "   Pico: " + ManoFlowMath.ToCentimetersofWater(Parsers.Float(pikeString[1])) + " CmH2O";
                peakText[2].text = "   Pico: " + ManoFlowMath.ToCentimetersofWater(Parsers.Float(pikeString[2])) + " CmH2O";

            } else {
            // Se a Cinta Extensora estiver conectada
            if (scc.IsConnected)
            {
                peakText[0].text = "   Pico: " + CintaFlowMath.ToLitresPerMinute(Parsers.Float(pikeString[0])) + " L/min";
                peakText[1].text = "   Pico: " + CintaFlowMath.ToLitresPerMinute(Parsers.Float(pikeString[1])) + " L/min";
                peakText[2].text = "   Pico: " + CintaFlowMath.ToLitresPerMinute(Parsers.Float(pikeString[2])) + " L/min";

            }}} ////////////////////////////////////////

        }

        public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        public void ToggleScoreMenu() => gameObject.SetActive(true);

        private void Start() => gameObject.SetActive(false);
    }
}