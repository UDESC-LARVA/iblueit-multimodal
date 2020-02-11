using Ibit.Core.Data;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Score;
using NaughtyAttributes;
using UnityEngine;

namespace Ibit.Plataform.Manager.Spawn
{
    public partial class Spawner
    {
        [BoxGroup("Obstacles")][SerializeField] private GameObject[] obstaclesAir;
        [BoxGroup("Obstacles")][SerializeField] private GameObject[] obstaclesWater;

        private void SpawnObstacle(ObjectModel model)
        {
            GameObject instance;

            //air
            if (model.PositionYFactor > 0)
            {
                instance = Instantiate(obstaclesAir[Random.Range(0, obstaclesAir.Length)],
                    new Vector3(_lastSpawned.position.x + _lastSpawned.localScale.x / 2 + model.PositionXSpacing, 0f),
                    this.transform.rotation,
                    this.transform);

                if (StageModel.Loaded.Level == 1)
                {
                    SpawnTutorialArrowWater(ref instance);
                }
            }

            //underwater
            else
            {
                instance = Instantiate(obstaclesWater[Random.Range(0, obstaclesWater.Length)],
                    new Vector3(_lastSpawned.position.x + _lastSpawned.localScale.x / 2 + model.PositionXSpacing, 0f),
                    this.transform.rotation,
                    this.transform);

                if (StageModel.Loaded.Level == 1)
                {
                    SpawnTutorialArrowAir(ref instance);
                }
            }

            instance.AddComponent<Obstacle>().Build(model);

            UpdateSpeed(ref instance);

            FindObjectOfType<Scorer>().UpdateMaxScore(model.Type, ref instance, model.DifficultyFactor);
        }
    }
}