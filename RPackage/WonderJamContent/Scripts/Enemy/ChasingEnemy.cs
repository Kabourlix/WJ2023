// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using System.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

namespace Rezoskour.Content
{
    public class ChasingEnemy : MonoBehaviour, IHealth
    {
        [SerializeField] private float maxHealth = 1f;
        [SerializeField] private Transform? targetTransform;
        public float speed = 1f;
        public float attackRange = 1f;
        public GameObject player;
        public bool isDistanceEnemy = false;
        private bool isOut = true;
        public Animator animator;
        public int damage = 1;
        
        private struct ChasingEnemyJob : IJobParallelForTransform
        {
            //Use only value types here
            public Vector3 playerPosition;
            public float speed;
            public float attackRange;
            public float deltaTime;

            public void Execute(int _index, TransformAccess _transform)
            {
                Vector3 enemyToPlayer = -playerPosition + _transform.position;
                float sqrDistance = Vector3.SqrMagnitude(enemyToPlayer);

                if (sqrDistance > attackRange * attackRange)
                {
                    Vector3 direction = (playerPosition - _transform.position).normalized;
                    _transform.position += direction * speed * deltaTime;
                }
                else
                {
                    // Perform attack logic here
                }

                //Update flip logic here
                // var theScale = _transform.localScale;
                // theScale.x *= enemyToPlayer.x < 0 ? -1 : 1;
                // _transform.localScale = theScale;
            }
        }

        private TransformAccessArray transformAccessArray;
        private JobHandle chasingJobHandle;

        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
            transformAccessArray = new TransformAccessArray(1);
            transformAccessArray.Add(transform);
            targetTransform = player.transform;
        }

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
                deltaTime = Time.deltaTime
            };

            chasingJobHandle = chasingJob.Schedule(transformAccessArray);
        }

        private void OnDestroy()
        {
            chasingJobHandle.Complete();
            transformAccessArray.Dispose();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Trigger");

            if (other.CompareTag("Player"))
            {
                animator.SetFloat("Speed", 0);
                isOut = false;

                StartCoroutine(StayInRange(other));
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Exit");

            if (other.CompareTag("Player"))
            {
                animator.SetBool("isAttacking", false);
                isOut = true;
                StopCoroutine(StayInRange(other));
            }
        }

        private IEnumerator StayInRange(Collider2D other)
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("isAttacking", true);
            if (!isDistanceEnemy)
            {
                if (other.TryGetComponent(out IHealth health))
                {
                    health.Damage(damage);
                }
            }

            yield return new WaitForSeconds(0.5f);
            animator.SetBool("isAttacking", false);
            yield return new WaitForSeconds(2f);
            if (!isOut)
            {
                OnTriggerEnter2D(player.GetComponent<Collider2D>());
            }
        }

        public void Init(Action action)
        {
            
        }

        public void Heal(int _amount)
        {
            
        }

        public void Damage(int _amount)
        {
            maxHealth -= _amount;
            if (maxHealth <= 0)
            {
                
            }
            
        }
    }
}