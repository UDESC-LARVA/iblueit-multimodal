using System.Collections.Generic;
using System.Linq;
using Ibit.Plataform.Data;
using Ibit.Plataform.Manager.Stage;
using NaughtyAttributes;
using UnityEngine;

namespace Ibit.Plataform.Manager.Spawn
{
    public partial class Spawner : MonoBehaviour
    {
        [BoxGroup("Tutorial")][SerializeField] private GameObject arrowUpPrefab;
        [BoxGroup("Tutorial")][SerializeField] private GameObject arrowDownPrefab;

        public LinkedList<Transform> SpawnedObjects
        {
            get
            {
                var array = this.GetComponentsInChildren<Transform>().Where(o => !o.name.Equals("Spawner") && !o.name.Equals("Sprite") && !o.name.StartsWith("Arrow"));
                return new LinkedList<Transform>(array);
            }
        }

        public int ObjectsOnScene => SpawnedObjects.Count;

        private Transform _lastSpawned => SpawnedObjects.Count > 0 ? SpawnedObjects.Last.Value : this.transform;

        private void Awake()
        {
            var stgMgr = FindObjectOfType<StageManager>();
            stgMgr.OnStageStart += Spawn;
            stgMgr.OnStageEnd += Clean;

            FindObjectOfType<Player>().OnObjectHit += PerformanceOnPlayerHit;
        }

        [Button("Spawn")]
        private void Spawn()
        {
            for (int i = 0; i < StageModel.Loaded.Loops; i++)
            {
                foreach (var stageObject in StageModel.Loaded.ObjectModels)
                {
                    switch (stageObject.Type)
                    {
                        case StageObjectType.Target:
                            SpawnTarget(stageObject);
                            break;
                        case StageObjectType.Obstacle:
                            SpawnObstacle(stageObject);
                            break;
                        case StageObjectType.Relax:
                            SpawnRelax(stageObject.PositionXSpacing);
                            break;
                    }
                }
            }
        }

        private void Clean()
        {
            if (SpawnedObjects.Count < 1)
                return;

            foreach (var tform in SpawnedObjects)
            {
                if (tform == this.gameObject.transform)
                    continue;

                Destroy(tform.gameObject);
            }
        }

        private void UpdateSpeed(ref GameObject obj)
        {
            obj.GetComponent<MoveObject>().Speed = StageModel.Loaded.ObjectSpeedFactor;
        }

        private void SpawnTutorialArrowAir(ref GameObject obj)
        {
            var arrow = Instantiate(arrowUpPrefab, obj.transform);
            arrow.transform.Translate(0f, obj.transform.localScale.y / 2f + 1f, 0f);
        }

        private void SpawnTutorialArrowWater(ref GameObject obj)
        {
            var arrow = Instantiate(arrowDownPrefab, obj.transform);
            arrow.transform.Translate(0f, obj.transform.localScale.y / 2f - 2f, 0f);
        }
    }
}