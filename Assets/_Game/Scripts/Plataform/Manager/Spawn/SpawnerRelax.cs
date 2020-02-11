using Ibit.Core.Data;
using Ibit.Plataform.Camera;
using NaughtyAttributes;
using UnityEngine;

namespace Ibit.Plataform.Manager.Spawn
{
    public partial class Spawner
    {
        [BoxGroup("Relax Time")][SerializeField] private GameObject relaxInsPrefab;
        [BoxGroup("Relax Time")][SerializeField] private GameObject relaxExpPrefab;
        [BoxGroup("Relax Time")][SerializeField] private GameObject relaxZeroPrefab;

        public bool RelaxTimeSpawned { get; private set; }

        private const float distanceFactor = 2f; //higher = closer

        private Vector3 _spawnPosition;

        private void SpawnRelax(float distanceFromLast)
        {
            var disfunction = (int)Pacient.Loaded.Condition;
            var objects = new GameObject[11 + 4 * disfunction];
            int i;

            _spawnPosition = new Vector3(_lastSpawned.position.x + (_lastSpawned.localScale.x / 2f) + distanceFromLast, 0f, 0f);

            for (i = 0; i < 4; i++)
                objects[i] = InstantiateRelaxObject(ref relaxInsPrefab, i);

            for (; i < 11; i++)
                objects[i] = InstantiateRelaxObject(ref relaxZeroPrefab, i);

            for (; i < objects.Length; i++)
                objects[i] = InstantiateRelaxObject(ref relaxExpPrefab, i);

            for (i = 0; i < objects.Length; i++)
            {
                UpdateSpeed(ref objects[i]);
                objects[i].transform.Translate(i / distanceFactor, 0f, 0f);
            }

            for (i = 0; i < objects.Length; i++)
            {
                if (i < 4)
                    objects[i].transform.Translate(0f, 0.2f * CameraLimits.Boundary, 0f);
                else if (i > 10)
                    objects[i].transform.Translate(0f, 0.15f * -CameraLimits.Boundary, 0f);
            }

            RelaxTimeSpawned = true;
        }

        private GameObject InstantiateRelaxObject(ref GameObject prefab, int id)
        {
            var tmp = Instantiate(prefab, _spawnPosition, this.transform.rotation, this.transform);
            tmp.AddComponent<Target>();
            return tmp;
        }
    }
}