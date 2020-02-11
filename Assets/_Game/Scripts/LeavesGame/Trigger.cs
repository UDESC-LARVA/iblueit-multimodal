using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ibit.LeavesGame
{
    public class Trigger : MonoBehaviour
    {
        private Spawner _spawner;

        void Start()
        {
            _spawner = FindObjectOfType<Spawner>();
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            PoolManager.Instance.DestroyObjectPool(other.gameObject);
            _spawner.SpawnObject();

        }
    }
}

