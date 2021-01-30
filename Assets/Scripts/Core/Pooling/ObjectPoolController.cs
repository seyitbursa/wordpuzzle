using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Pooling
{
    public enum ObjectType
    {
        GridItem = 0,
        Letter = 1,
        Balloon = 2,
    }

    public class ObjectPoolController : MonoBehaviour
    {
        [Serializable]
        public struct ObjectPool
        {
            public ObjectType objectType;
            public List<GameObject> activeObjects;
            public List<GameObject> objectList;
            public GameObject prefab;
            public Transform objectParent;
        }

        [SerializeField] private List<ObjectPool> objectPools;
        private Dictionary<ObjectType, ObjectPool> objectPoolDictionary;

        public static ObjectPoolController instance;
        private void Awake()
        {
            instance = this;
            Initialize();
            DontDestroyOnLoad(this);
        }

        private void Initialize()
        {
            if (objectPoolDictionary == null)
            {
                objectPoolDictionary = new Dictionary<ObjectType, ObjectPool>();

                var numberOfObjectPools = objectPools.Count;
                for (var i = 0; i < numberOfObjectPools; i++)
                {
                    var objectPool = objectPools[i];
                    objectPoolDictionary.Add(objectPool.objectType, objectPool);
                }
            }
            else
            {
                ResetAllActiveObjects();
            }
        }

        public GameObject GetObject(ObjectType objectType)
        {
            var objectPool = objectPoolDictionary[objectType];

            if (objectPool.objectList.Count == 0)
            {
                CreateObject(objectType);
            }
            var obj = objectPool.objectList[0];

            objectPool.objectList.RemoveAt(0);
            objectPool.activeObjects.Add(obj);

            obj.SetActive(true);
            return obj;
        }

        private void CreateObject(ObjectType objectType)
        {
            var objectPool = objectPoolDictionary[objectType];
            var objectPrefab = objectPool.prefab;
            var obj = Instantiate(objectPrefab, objectPool.objectParent);

            objectPool.objectList.Add(obj);
        }

        public void ResetObject(GameObject obj)
        {
            var objectType = obj.GetComponent<PoolObject>().GetObjectType();
            var objectPool = objectPoolDictionary[objectType];

            objectPool.activeObjects.Remove(obj);
            objectPool.objectList.Add(obj);
            obj.transform.SetParent(objectPool.objectParent);
            obj.transform.localPosition = Vector3.zero;
            obj.SetActive(false);
        }

        private void ResetAllActiveObjects()
        {
            var numberOfObjectPools = objectPools.Count;
            for (var i = 0; i < numberOfObjectPools; i++)
            {
                var objectPool = objectPools[i];
                var numberOfActiveObjects = objectPool.activeObjects.Count;
                for (var j = 0; j < numberOfActiveObjects; j++)
                {
                    ResetObject(objectPool.activeObjects[0]);
                }
            }
        }
    }
}