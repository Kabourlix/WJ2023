// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

using System;
using Rezoskour.Content.Misc;
using Rezoskour.Content.waves;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Rezoskour.Content
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private WaveData waveData;
        public EnemyPoolManager enemyPoolManager;
        public float spawnInterval = 30f;
        private Camera playerCamera;
        private GameObject player;
        private float lastSpawnTime;
        private Random random = new(1);
        private int WaveCounter = 0 ;
        private bool firstwave = true;
        public int width = 22;
        public int height = 6;
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
            
            if (Time.time - lastSpawnTime >= spawnInterval || firstwave)
            {
                WaveCounter++;
                SpawnEnemy();
                lastSpawnTime = Time.time;
                firstwave = false;
            }
            
        }


        private void SpawnEnemy()
        {
            foreach (var index in waveData.chasingEnemies)
            {
                var res = (int) index.curve.Evaluate(WaveCounter);
                Debug.Log(index.enemyType +" " + res);
                if (res > 0)
                {
                    for (int i = 0; i < res; i++)
                    {
                        float cameraHeight = playerCamera.orthographicSize;
                        float cameraWidth = cameraHeight * playerCamera.aspect;
                        float2 spawnPosition;
                        Vector3 cameraPosition = playerCamera.transform.position;
                        do
                        {
                            spawnPosition =
                                random.NextFloat2(
                                    new float2(height, width),
                                    new float2(-height, -width));
                        }while (IsInRange(spawnPosition, cameraPosition, cameraWidth, cameraHeight));
                        Vector3 spawnPoint = new(spawnPosition.x, spawnPosition.y, 0f);
                        ChasingEnemy enemyCurve = enemyPoolManager.GetEnemy(index.enemyType);
                        enemyCurve.transform.position = spawnPoint;
                    }
                }
            }
        }

        private bool IsInRange(float2 spawnPosition, Vector3 cameraPosition, float cameraWidth, float cameraHeight)
        {
            return spawnPosition.x < cameraPosition.x + cameraWidth && spawnPosition.x > cameraPosition.x - cameraWidth &&
                   spawnPosition.y < cameraPosition.y + cameraHeight && spawnPosition.y > cameraPosition.y - cameraHeight;
        }
    }
}