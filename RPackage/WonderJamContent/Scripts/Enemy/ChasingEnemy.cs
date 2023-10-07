// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

namespace Rezoskour.Content
{
    public class ChasingEnemy : MonoBehaviour
    {
        [SerializeField] private Transform? targetTransform;
        public float speed = 1f;
        public float attackRange = 1f;

        private struct ChasingEnemyJob : IJobParallelForTransform
        {
            //Use only value types here
            public Vector3 playerPosition;
            public float speed;
            public float attackRange;
            public float deltaTime;

            public void Execute(int _index, TransformAccess _transform)
            {
                float sqrDistance = Vector3.SqrMagnitude(playerPosition - _transform.position);

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
            }
        }

        private TransformAccessArray transformAccessArray;
        private JobHandle chasingJobHandle;

        private void Start()
        {
            transformAccessArray = new TransformAccessArray(1);
            transformAccessArray.Add(transform);
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
    }
}