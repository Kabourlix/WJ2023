// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace Rezoskour.Content
{
    public class BasicAttack : RAttack
    {
        [SerializeField] private ParticleSystem basicEffect = null!;
        private Vector3 positionAttack;
        private Transform basicCachedTransform = null!;

        private void Start()
        {
            basicCachedTransform = basicEffect.transform;
        }

        public override IEnumerator PerformCoroutine()
        {
            if (UserTransform == null)
            {
                Debug.LogError("Player transform is null.");
                yield break;
            }

            if (Manager == null)
            {
                Debug.LogError("GAME MANAGER IS NULL");
                yield break;
            }

            while (true)
            {
                Vector3 targetPos = UserTransform.position + data.range * (Vector3) GetLookDirection();

                float rangeAttack = data.attackAreaRange;
                if (playerStats != null)
                {
                    //TODO
                    //rangeAttack *= playerStats.;
                }

                positionAttack = targetPos;
                Collider2D[] hits = Physics2D.OverlapCircleAll(targetPos, rangeAttack, layerMask);
                basicEffect.Play();
                if (hits.Length == 0) //No hit
                {
                    yield return waitForAttackRefresh;
                    continue;
                }

                Debug.Log("Deal damage");
                foreach (Collider2D col in hits)
                {
                    if (!col.gameObject.TryGetComponent(out IHealth health))
                    {
                        continue;
                    }


                    float damage = data.damage;
                    if (playerStats != null)
                    {
                        damage *= playerStats.globalDamageMultiplier;
                    }

                    health.Damage(Mathf.FloorToInt(damage));
                }

                yield return waitForAttackRefresh;
            }
        }

        private void Update()
        {
            basicCachedTransform.position = positionAttack;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (UserTransform == null || Manager == null)
            {
                return;
            }

            Gizmos.color = Color.green;
            Vector3 targetPos = UserTransform.position + data.range * Manager.PlayerLookDirection;
            Gizmos.DrawWireSphere(targetPos, data.attackAreaRange);
            Handles.color = Color.green;
            Handles.Label(targetPos, "Attack Basic");
            Handles.color = Color.white;
            Gizmos.color = Color.white;
        }
#endif
    }
}