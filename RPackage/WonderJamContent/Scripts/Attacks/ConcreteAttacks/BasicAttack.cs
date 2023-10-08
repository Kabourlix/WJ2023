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

                Collider2D[] hits = Physics2D.OverlapCircleAll(targetPos, data.attackAreaRange, layerMask);
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

                    health.Damage(data.damage);
                }

                yield return waitForAttackRefresh;
            }
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