// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;
using System.Collections;
using UnityEngine;

namespace Rezoskour.Content
{
    public class BasicAttack : RAttack
    {
        public override IEnumerator PerformCoroutine()
        {
            if (PlayerTransform == null)
            {
                Debug.LogError("Player transform is null.");
                yield break;
            }

            while (true)
            {
                Vector3 targetPos = PlayerTransform.position + data.range * PlayerTransform.forward.normalized;

                Collider2D[] hits = Physics2D.OverlapCircleAll(targetPos, data.attackAreaRange, layerMask);
                if (hits.Length == 0) //No hit
                {
                    yield return waitForAttackRefresh;
                    continue;
                }

                foreach (Collider2D col in hits)
                {
                    if (!col.gameObject.TryGetComponent(out HealthManager health))
                    {
                        yield return waitForAttackRefresh;
                        continue;
                    }

                    health.Damage(data.damage);
                }

                yield return waitForAttackRefresh;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (PlayerTransform == null)
            {
                return;
            }

            Vector3 targetPos = PlayerTransform.position + data.range * PlayerTransform.forward.normalized;
            Gizmos.DrawWireSphere(targetPos, data.attackAreaRange);
        }
#endif
    }
}