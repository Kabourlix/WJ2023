// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 08

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;

namespace Rezoskour.Content
{
    public enum EnemyType
    {
        Fries,
        Ketchup,
        Nuggets
    }

    public class EnemyPoolManager : MonoBehaviour
    {
        [SerializeField] private ChasingEnemy friesPrefab = null!;
        [SerializeField] private ChasingEnemy ketchupPrefab = null!;
        [SerializeField] private ChasingEnemy nuggetsPrefab = null!;

        private Dictionary<EnemyType, ObjectPool<ChasingEnemy>> enemyPools = new();

        private void Awake()
        {
            enemyPools.Add(EnemyType.Fries,
                new ObjectPool<ChasingEnemy>(() => OnCreateEnemyAbstract(friesPrefab, EnemyType.Fries), OnGetEnemy,
                    OnReleaseEnemy));
            enemyPools.Add(EnemyType.Ketchup,
                new ObjectPool<ChasingEnemy>(() => OnCreateEnemyAbstract(ketchupPrefab, EnemyType.Ketchup), OnGetEnemy,
                    OnReleaseEnemy));
            enemyPools.Add(EnemyType.Nuggets,
                new ObjectPool<ChasingEnemy>(() => OnCreateEnemyAbstract(nuggetsPrefab, EnemyType.Nuggets), OnGetEnemy,
                    OnReleaseEnemy));
        }

        private ChasingEnemy OnCreateEnemyAbstract(ChasingEnemy _prefab, EnemyType _type)
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GAMEMANAGER IS NULL!");
                return default!;
            }

            ChasingEnemy enemy = Instantiate(_prefab, GameManager.Instance.transform).GetComponent<ChasingEnemy>();
            enemy.name = $"Enemy {_prefab.name + 1}";
            enemy.Init(() => enemyPools[_type].Release(enemy));
            //Jouer mon animation
            //LeanTween.value(enemy.gameObject,0,1,time).setOnComplete(() => enemyPools[_type].Release(enemy))
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


        public ChasingEnemy GetEnemy(EnemyType _type)
        {
            return enemyPools[_type].Get();
        }
    }
}