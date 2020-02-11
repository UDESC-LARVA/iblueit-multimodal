using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ibit.LeavesGame
{
    public class PoolManager : MonoBehaviour
    {
        
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        public List<GameObject> objPool = new List<GameObject>();
        private int objPoolSize;

        public static PoolManager _instance;

        public static PoolManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PoolManager>();
                }
                return _instance;
            }
        }

        public void CreatePool(ref List<Spawner.Pool> pools)
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Spawner.Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject newObject = Instantiate(pool.prefab,this.transform);
                    newObject.SetActive(false);
                    objectPool.Enqueue(newObject);
                }
                poolDictionary.Add(pool.tag, objectPool);
            }
            
        }

        public GameObject GetObject(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("A Pool com a tag " + tag + " não existe!");
                return null;
            }
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            poolDictionary[tag].Enqueue(objectToSpawn);
            //if (objPool.Count > 0)
            //{
            //    GameObject obj = objPool[0];
            //    objPool.RemoveAt(0);
            //    return obj;
            //}
            //return null;
            return objectToSpawn;
        }

        public void DestroyObjectPool(GameObject obj)
        {
            //objPool.Add(obj);
            obj.SetActive(false);
        }
        public void ClearPool()
        {
            objPool.Clear();
            objPool = null;
        }

    }


}
