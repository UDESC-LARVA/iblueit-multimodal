using Ibit.Core.Audio;
using Ibit.Core.Data;
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

        private void OnEnable()
        {
            var scorer = FindObjectOfType<Scorer>();

            if (scorer.Result == GameResult.Success)
            {
                finalResult.text = "GoGoGo!";
                finalResult.color = Color.cyan;
                motivationText.text = "Muito bem! Você passou de nível. Continue assim!";
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