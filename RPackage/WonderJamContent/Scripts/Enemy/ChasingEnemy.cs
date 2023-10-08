// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using System.Collections;
using Rezoskour.Content.Collectable;
using Rezoskour.Content.Misc;
using Unity.Collections;
using Unity.Jobs;
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
        [SerializeField] private Transform? targetTransform;
        public float speed = 1f;
        public float attackRange = 1f;
        public GameObject player;
        public bool isDistanceEnemy = false;
        private bool isOut = true;
        public Animator animator;
        public int damage = 1;


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
                Debug.Log(sqrDistance);
                if (sqrDistance > attackRange * attackRange)
                {
                    Vector3 direction = (playerPosition - _transform.position).normalized;
                    _transform.position += direction * speed * deltaTime;
                    triggerAttackArray[0] = false;
                }
                else
                {
                    Debug.Log("atteint");
                    triggerAttackArray[0] = true;
                }

                //Update flip logic here
                // var theScale = _transform.localScale;
                // theScale.x *= enemyToPlayer.x < 0 ? -1 : 1;
                // _transform.localScale = theScale;
            }

            
        }
        private TransformAccessArray transformAccessArray;
        protected NativeArray<bool> triggerAttackArray;
        private JobHandle chasingJobHandle;

        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
            transformAccessArray = new TransformAccessArray(1);
            triggerAttackArray = new NativeArray<bool>(1, Allocator.Persistent);
            transformAccessArray.Add(transform);
            targetTransform = player.transform;
        }
        protected abstract void PerformAttack();
        private void Update()
        {
            chasingJobHandle.Complete();
            if (targetTransform == null)
            {
                return;
            }

            ChasingEnemyJob chasingJob = new()
            {
                playerPosition = targetTransform.position,
                speed = speed,
                attackRange = attackRange,
                deltaTime = Time.deltaTime,
                triggerAttackArray = triggerAttackArray
            };
            if(triggerAttackArray[0])
            {
                Debug.Log("PerformedAttack");
                PerformAttack();
            }
            chasingJobHandle = chasingJob.Schedule(transformAccessArray);
        }
        
        private void OnDestroy()
        {
            chasingJobHandle.Complete();
            transformAccessArray.Dispose();
            triggerAttackArray.Dispose();
        }
        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //
        //     if (other.CompareTag("Player"))
        //     {
        //         animator.SetFloat("Speed", 0);
        //         isOut = false;
        //
        //         StartCoroutine(StayInRange(other));
        //     }
        // }
        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     if (other.CompareTag("Player"))
        //     {
        //         animator.SetBool("isAttacking", false);
        //         isOut = true;
        //         StopCoroutine(StayInRange(other));
        //     }
        // }
        //
        // private IEnumerator StayInRange(Collider2D other)
        // {
        //     yield return new WaitForSeconds(0.5f);
        //     animator.SetBool("isAttacking", true);
        //     if (!isDistanceEnemy)
        //     {
        //         if (other.TryGetComponent(out IHealth health))
        //         {
        //             health.Damage(damage);
        //         }
        //     }
        //
        //     yield return new WaitForSeconds(0.5f);
        //     animator.SetBool("isAttacking", false);
        //     yield return new WaitForSeconds(2f);
        //     if (!isOut)
        //     {
        //         OnTriggerEnter2D(player.GetComponent<Collider2D>());
        //     }
        // }

        public void Init(Action? _action)
        {
            releaseCallback = _action;
        }

        public void Heal(int _amount)
        {
            
        }

        public virtual void Damage(int _amount)
        {
            maxHealth -= _amount;
            if (maxHealth <= 0)
            {
                if (CollectableManager.Instance == null)
                {
                    Debug.LogError("CollectableManager.Instance is null !");
                    return;
                }

                var range = oilSpawnProbability + expSpawnProbability;
                var rand = Random.Range(0, range);
                if (rand <= oilSpawnProbability)
                {
                    Debug.Log("au debut : "+transform.position);
                    var transform1 = transform;
                    CollectableManager.Instance.SpawnOil(transform1.position, transform1.rotation);
                }
                else
                {
                    var transform1 = transform;
                    CollectableManager.Instance.SpawnXp(transform1.position, transform1.rotation);
                }
                releaseCallback?.Invoke();
            }
            
        }
    }
}