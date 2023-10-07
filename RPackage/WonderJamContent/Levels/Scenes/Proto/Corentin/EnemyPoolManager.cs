using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Rezoskour.Content
{
    public class EnemyPoolManager : MonoBehaviour
    {
        [SerializeField] private ChasingEnemy enemyPrefab = null!;
        private ObjectPool<ChasingEnemy> enemyPool = null!;

        private void Awake()
        {
            enemyPool = new ObjectPool<ChasingEnemy>(OnCreateEnemy, OnGetEnemy, OnReleaseEnemy);
        }
        

        private ChasingEnemy OnCreateEnemy()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GAMEMANAGER IS NULL!");
                return default!;
            }
            ChasingEnemy enemy = Instantiate(enemyPrefab, GameManager.Instance.transform).GetComponent<ChasingEnemy>();
            enemy.name = $"Enemy {enemyPool.CountAll + 1}";
            enemy.Init(() => enemyPool.Release(enemy));
            return enemy;
        }

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
