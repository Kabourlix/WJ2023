// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

using System;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Rezoskour.Content
{
    public class EnemySpawner : MonoBehaviour
    {
        public EnemyPoolManager enemyPoolManager;
        public float spawnInterval = 2.0f;
        private Camera playerCamera;
        private GameObject player;
        private float lastSpawnTime;
        private Random random = new(1);

        private void Awake()
        {
            random = new Random((uint) Environment.TickCount);
        }

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null !");
                return;
            }

            player = GameManager.Instance.PlayerTf.gameObject;
            lastSpawnTime = Time.time;
            playerCamera = player.GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            if (Time.time - lastSpawnTime >= spawnInterval)
            {
                SpawnEnemy();
                lastSpawnTime = Time.time;
            }
        }


        private void SpawnEnemy()
        {
            float cameraHeight = playerCamera.orthographicSize;
            float cameraWidth = cameraHeight * playerCamera.aspect;
            float2 spawnPosition;
            Vector3 cameraPosition = playerCamera.transform.position;
            if (random.NextBool())
            {
                // Moitié supérieure
                spawnPosition =
                    random.NextFloat2(new float2(cameraPosition.x - cameraWidth, cameraPosition.y + cameraWidth),
                        new float2(cameraPosition.y, cameraPosition.y + cameraHeight));
            }
            else
            {
                // Moitié inférieure
                spawnPosition =
                    random.NextFloat2(new float2(cameraPosition.x - cameraWidth, cameraPosition.x + cameraWidth),
                        new float2(cameraPosition.y - cameraHeight, cameraPosition.y));
            }

            Vector3 spawnPoint = new(spawnPosition.x, spawnPosition.y, 0f);
            ChasingEnemy enemy = enemyPoolManager.GetEnemy();

            if (enemy != null)
            {
                enemy.transform.position = spawnPoint;
            }
        }
    }
}