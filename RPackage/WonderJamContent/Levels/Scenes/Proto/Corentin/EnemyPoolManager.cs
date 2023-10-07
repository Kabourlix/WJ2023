// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Rezoskour.Content
{
    public enum EnemyType
    {
        Fries,
        Ketchup
    }
    public class EnemyPoolManager : MonoBehaviour
    {
        [FormerlySerializedAs("ketchupPrefab")] [SerializeField] private ChasingEnemy friesPrefab = null!;
        [SerializeField] private ChasingEnemy ketchupPrefab = null!;

        private ObjectPool<ChasingEnemy> enemyPool = null!;
        private Dictionary<EnemyType, ObjectPool<ChasingEnemy>> enemyPools = new();
        private void Awake()
        {
            //enemyPools.Add(Fries, instanciation de la pool);
            enemyPool = new ObjectPool<ChasingEnemy>(()=>OnCreateEnemyAbstract(friesPrefab), OnGetEnemy, OnReleaseEnemy);
        }


        private ChasingEnemy OnCreateEnemyAbstract(ChasingEnemy _prefab)
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GAMEMANAGER IS NULL!");
                return default!;
            }

            ChasingEnemy enemy = Instantiate(_prefab, GameManager.Instance.transform).GetComponent<ChasingEnemy>();
            enemy.name = $"Enemy {enemyPool.CountAll + 1}";
            enemy.Init(() => enemyPool.Release(enemy));
            return enemy;
        }

        // private ChasingEnemy OnCreateEnemy()
        // {
        //     if (GameManager.Instance == null)
        //     {
        //         Debug.LogError("GAMEMANAGER IS NULL!");
        //         return default!;
        //     }
        //
        //     ChasingEnemy enemy = Instantiate(friesPrefab, GameManager.Instance.transform).GetComponent<ChasingEnemy>();
        //     enemy.name = $"Enemy {enemyPool.CountAll + 1}";
        //     enemy.Init(() => enemyPool.Release(enemy));
        //     return enemy;
        // }

        private void OnGetEnemy(ChasingEnemy _enemy)
        {
            _enemy.gameObject.SetActive(true);
        }

        private void OnReleaseEnemy(ChasingEnemy _enemy)
        {
            _enemy.gameObject.SetActive(false);
        }


        public ChasingEnemy GetEnemy()
        {
            return enemyPool.Get();
        }
    }
}