// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using System.Collections;
using UnityEditor;
using UnityEngine;


namespace Rezoskour.Content
{
    public class BerserkBurnAttack : RAttack
    {
        [SerializeField] private ParticleSystem berserkEffect = null!;

        public override IEnumerator PerformCoroutine()
        {
            if (UserTransform == null)
            {
                Debug.LogError("Player transform is null.");
                yield break;
            }

            while (true)
            {
                Collider2D[] hits =
                    Physics2D.OverlapCircleAll(UserTransform.position, data.attackAreaRange, layerMask);
                berserkEffect.Play(true);
                if (hits.Length == 0)
                {
                    yield return waitForAttackRefresh;
                    continue;
                }

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
            if (UserTransform == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Vector3 position = UserTransform.position;

            Gizmos.DrawWireSphere(position, data.attackAreaRange);
            Handles.color = Color.red;
            Handles.Label(position + data.attackAreaRange * Vector3.up, "berserk");
            Handles.color = Color.white;
            Gizmos.color = Color.white;
        }
#endif
    }
}