using Ibit.Core.Game;
using Ibit.Plataform.Manager.Score;
using UnityEngine;
using UnityEngine.UI;

public class StarGroupUI : MonoBehaviour
{
	[SerializeField] private Sprite starOn;
	[SerializeField] private Image star1;
	[SerializeField] private Image star2;
	[SerializeField] private Image star3;

	void OnEnable()
	{
		var scorer = FindObjectOfType<Scorer>();
		var scoreRatio = scorer.Score / scorer.MaxScore;
		var diffTo100 = 1 - GameManager.LevelUnlockScoreThreshold;

		if (scoreRatio >= GameManager.LevelUnlockScoreThreshold)
		{
			star1.sprite = starOn;
		}

		if (scoreRatio >= GameManager.LevelUnlockScoreThreshold + (0.33f * diffTo100))
		{
			star2.sprite = starOn;
		}

		if (scoreRatio >= GameManager.LevelUnlockScoreThreshold + (0.66f * diffTo100))
		{
			star3.sprite = starOn;
		}
	}
}