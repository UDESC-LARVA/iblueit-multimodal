using Ibit.Core.Audio;
using Ibit.Core.Data;
using Ibit.Core.Database;
using Ibit.Core.Util;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Score;
using UnityEngine;
using UnityEngine.UI;

using Color = UnityEngine.Color;

namespace Ibit.Plataform.UI
{
    public class ResultScreenUI : MonoBehaviour
    {
        [SerializeField]
        private Text finalResult, motivationText, resultInfo;

        [SerializeField]
        private Button nextStageButton;

        [SerializeField]
        private Button pauseButton;

        [SerializeField]
        private Button repeatStageButton;

        [SerializeField]
        private Text scoreValue, scoreRatio;

        [SerializeField]
        public static int numberFailures; // Número de vezes que o paciente perdeu sequencialmente.

        private void OnEnable()
        {
            var scorer = FindObjectOfType<Scorer>();

            if (scorer.Result == GameResult.Success)
            {
                finalResult.text = "GoGoGo!";
                finalResult.color = Color.cyan;
                motivationText.text = "Muito bem! Você passou de nível. Continue assim!";

                numberFailures = 0; // Caso ganhe uma partida, o número de falhas é zerado.
                Debug.Log($"numberFailures {numberFailures}");

                SoundManager.Instance.PlaySound("StageClear");
            }
            else
            {
                if(scorer.Score < scorer.MaxScore * 0.3f)
                {
                    repeatStageButton.interactable = false;
                }

                nextStageButton.interactable = false;
                finalResult.text = "YOU BLEW IT";
                finalResult.color = Color.red;
                motivationText.text = "Você não conseguiu pontos suficientes. Não desista!";

                numberFailures += 1; // Caso perca, o número de falhas é incrementado.

                if (numberFailures >= ParametersDb.parameters.lostXtimes) // Se perdeu X vezes, então solicita recalibração...
                {
                    SysMessage.Warning($"Você perdeu {numberFailures} vezes, pode haver algum problema com sua calibração, sugiro que recalibre os dispositivos");
                }

                Debug.Log($"numberFailures {numberFailures}");

                SoundManager.Instance.PlaySound("PlayerDamage");
            }

            var score = scorer.Score;
            var maxScore = scorer.MaxScore;
            score = Mathf.Clamp(score, 0f, maxScore);

            pauseButton.interactable = false;
            scoreValue.text = scoreRatio.text = "";

            GameObject.Find("Canvas").transform.Find("HUD").transform.localScale = Vector3.zero;

            resultInfo.text =
                $"• Score: {score:####} / {maxScore:####} ({((score / maxScore) * 100f):####}%)\n" +
                $"• Fase: {StageModel.Loaded.Phase}\n" +
                $"• Nível: {StageModel.Loaded.Level}\n" +
                $"• Jogador: {Pacient.Loaded.Name}";
        }
    }
}