// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using System.Collections;
using Rezoskour.Content.Collectable;
using Rezoskour.Content.Misc;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;

// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

namespace Rezoskour.Content
{
    public abstract class ChasingEnemy : MonoBehaviour, IHealth
    {
        [Range(0, 1)] [SerializeField] private float oilSpawnProbability;
        [Range(0, 1)] [SerializeField] private float expSpawnProbability;
        [SerializeField] protected float maxHealth = 1f;
        [SerializeField] protected Transform targetTransform = null!;
        public float speed = 1f;
        public float attackRange = 1f;
        public GameObject player;
        public bool isDistanceEnemy = false;
        public Animator animator;
        public int damage = 1;
        public AudioSource audioSource;
        private float currentHealth;
        protected Action? releaseCallback;

        protected struct ChasingEnemyJob : IJobParallelForTransform
        {
            //Use only value types here
            public Vector3 playerPosition;
            public float speed;
            public float attackRange;
            public float deltaTime;
            public NativeArray<bool> triggerAttackArray;


            public void Execute(int _index, TransformAccess _transform)
            {
                Vector3 enemyToPlayer = -playerPosition + _transform.position;
                float sqrDistance = Vector3.SqrMagnitude(enemyToPlayer);
                if (sqrDistance > attackRange * attackRange)
                {
                    Vector3 direction = (playerPosition - _transform.position).normalized;
                    _transform.position += direction * speed * deltaTime;
                    triggerAttackArray[0] = false;
                }
                else
                {
                    triggerAttackArray[0] = true;
                }
            }
        }

        private TransformAccessArray transformAccessArray;
        protected NativeArray<bool> triggerAttackArray;
        protected JobHandle chasingJobHandle;
        protected bool isDying = false;

        protected virtual void Start()
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
            transformAccessArray = new TransformAccessArray(1);
            triggerAttackArray = new NativeArray<bool>(1, Allocator.Persistent);
            transformAccessArray.Add(transform);
            targetTransform = player.transform;
        }


        private void Update()
        {
            if (targetTransform == null)
            {
                return;
            }

            if (!isDying)
            {
                ChasingEnemyJob chasingJob = new()
                {
                    playerPosition = targetTransform.position,
                    speed = speed,
                    attackRange = attackRange,
                    deltaTime = Time.deltaTime,
                    triggerAttackArray = triggerAttackArray
                };

                chasingJobHandle = chasingJob.Schedule(transformAccessArray);
            }
        }

        protected void LateUpdate()
        {
            chasingJobHandle.Complete();
            if (triggerAttackArray[0])
            {
                PerformAttack();
            }
            else
            {
                StopPerform();
            }
        }

        private void OnDestroy()
        {
            chasingJobHandle.Complete();
            transformAccessArray.Dispose();
            triggerAttackArray.Dispose();
        }

        protected abstract void PerformAttack();

        protected virtual void StopPerform() { }

        public void Init(Action? _action)
        {
            currentHealth = maxHealth;
            GetComponent<Animator>().SetBool("isDead", false);
            isDying = false;
            releaseCallback = _action;
        }

        public void Heal(int _amount) { }

        public virtual void Damage(int _amount)
        {
            currentHealth -= _amount;
            if (currentHealth <= 0)
            {
                chasingJobHandle.Complete();
                triggerAttackArray[0] = false;
                isDying = true;
                if (CollectableManager.Instance == null)
                {
                    Debug.LogError("CollectableManager.Instance is null !");
                    return;
                }

                float range = oilSpawnProbability + expSpawnProbability;
                float rand = Random.Range(0, range);
                if (rand <= oilSpawnProbability)
                {
                    Debug.Log("au debut : " + transform.position);
                    Transform? transform1 = transform;
                    CollectableManager.Instance.SpawnOil(transform1.position, transform1.rotation);
                }
                else
                {
                    Transform? transform1 = transform;
                    CollectableManager.Instance.SpawnXp(transform1.position, transform1.rotation);
                }

                releaseCallback?.Invoke();
            }
        }
    }
}