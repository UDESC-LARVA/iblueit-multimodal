using System;
using Ibit.Core.Game;
using Ibit.Plataform.Data;
using NaughtyAttributes;
using UnityEngine;

namespace Ibit.Plataform.Manager.Score
{
    public enum GameResult
    {
        Failure,
        Success
    }

    public partial class Scorer : MonoBehaviour
    {
        [SerializeField][ReadOnly] private float maxScore;

        [SerializeField][ReadOnly] private float score;

        public float MaxScore => maxScore;
        public GameResult Result { get; private set; }
        public float Score => score;

        public GameResult CalculateResult(bool gameOver = false)
        {
            if (gameOver)
            {
                Result = GameResult.Failure;
            }
            else
            {
                Result = Score >= MaxScore * GameManager.LevelUnlockScoreThreshold ?
                    GameResult.Success :
                    GameResult.Failure;
            }

            return Result;
        }

        private void Awake()
        {
            score = 0;
            maxScore = 0;
            FindObjectOfType<Player>().OnObjectHit += ScoreOnPlayerCollision;

            DistantiateFromCameraCenter();
        }

        private void DistantiateFromCameraCenter()
        {
            var horzExtent = UnityEngine.Camera.main.orthographicSize * Screen.width / Screen.height;
            this.gameObject.transform.position = new Vector3(-horzExtent / 2f, 0f, 0f);
        }

        public void UpdateMaxScore(StageObjectType type, ref GameObject obj, float difficultyFactor)
        {
            switch (type)
            {
                case StageObjectType.Target:
                    maxScore += obj.GetComponent<Target>().Score;
                    break;

                case StageObjectType.Obstacle:
                    maxScore += obj.GetComponent<Obstacle>().Score;
                    break;
            }
        }
    }
}