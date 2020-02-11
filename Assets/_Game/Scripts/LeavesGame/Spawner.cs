using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ibit.LeavesGame
{
    public class Spawner : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;

        }



        public bool passed = false;
        public int initialSpawnQuantity;
        public GameObject lastObj;
        public List<Pool> pools;
        public System.Random rnd = new System.Random();
        
        
        void Awake()
        {
            
            PoolManager.Instance.CreatePool(ref pools);
        }

        void Start()
        {
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            float last_xPos = 0;
            Debug.Log(PoolManager.Instance.objPool.Count);
            for (int i = 0; i < initialSpawnQuantity; i++)
            {
                
                GameObject obj = PoolManager.Instance.GetObject(pools[rnd.Next(pools.Count)].tag);
                if (!passed)
                {
                    obj.transform.position = new Vector3(last_xPos, 2, -0.1f);
                    last_xPos += 2;
                    passed = true;
                }
                else
                {
                    obj.transform.position = new Vector3(last_xPos, -2, -0.1f);
                    last_xPos += 2;
                    passed = false;
                }
                obj.SetActive(true);
                lastObj = obj;
            }
        }

        public void SpawnObject()
        {
            Vector3 lastPos = lastObj.transform.position;
            lastPos.x += 2;
            lastPos.y *= -1;
            GameObject obj = PoolManager.Instance.GetObject(pools[rnd.Next(pools.Count)].tag);
            obj.transform.position = lastPos;
            obj.SetActive(true);
            lastObj = obj;
        }
    }
}
