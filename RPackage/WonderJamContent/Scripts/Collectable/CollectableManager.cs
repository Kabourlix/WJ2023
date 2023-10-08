// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

namespace Rezoskour.Content.Collectable
{
    public class CollectableManager : MonoBehaviour
    {
        #region Singleton

        public static CollectableManager? Instance;
        [SerializeField] private GameObject? oilPrefab;
        [SerializeField] private GameObject? experiencePrefab;
        [SerializeField] private GameObject? healPrefab;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"{nameof(CollectableManager)} cannot be instanced more than once.");
                Destroy(gameObject);
            }

            Instance = this;
        }

        #endregion

        public static List<PooledObjectInfo> objectPools = new();

        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            var pool = objectPools.Find(x => x.lookupString == objectToSpawn.name);
            if (pool == null)
            {
                pool = new PooledObjectInfo() { lookupString = objectToSpawn.name };
                objectPools.Add(pool);
            }

            //Check if they are any inactive objects in the pool
            var spawnableObject = pool.InactiveObjects.FirstOrDefault();

            if (spawnableObject == null)
            {
                //If not, create a new one
                spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
                spawnableObject.name = objectToSpawn.name;
            }
            else
            {
                //If yes, reuse it
                spawnableObject.transform.position = spawnPosition;
                spawnableObject.transform.rotation = spawnRotation;
                pool.InactiveObjects.Remove(spawnableObject);
                spawnableObject.SetActive(true);
            }
            Debug.Log("A la fin : "+spawnPosition);

            return spawnableObject;
        }

        public static void ReturnObjectToPool(GameObject obj)
        {
            //var goName = obj.name.Substring(0, obj.name.Length - 7); //Remove "(Clone)" from the name

            var pool = objectPools.Find(p => p.lookupString == obj.name);
            if (pool == null)
            {
                Debug.LogWarning($"No pool found for {obj.name}");
            }
            else
            {
                obj.SetActive(false);
                pool.InactiveObjects.Add(obj);
            }
        }

        public void SpawnXp(Vector3 _spawnPosition, Quaternion _spawnRotation)
        {
            SpawnObject(experiencePrefab, _spawnPosition, _spawnRotation);
        }

        public void SpawnOil(Vector3 _spawnPosition, Quaternion _spawnRotation)
        {
            Debug.Log("au milieu : "+_spawnPosition);
            SpawnObject(oilPrefab, _spawnPosition, _spawnRotation);
        }

        public void SpawnHeal(Vector3 _spawnPosition, Quaternion _spawnRotation)
        {
            SpawnObject(healPrefab, _spawnPosition, _spawnRotation);
        }
    }

    public class PooledObjectInfo
    {
        public string lookupString;
        public List<GameObject> InactiveObjects = new();
    }
}