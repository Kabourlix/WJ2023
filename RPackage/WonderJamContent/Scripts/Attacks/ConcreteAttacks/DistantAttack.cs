// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

namespace Rezoskour.Content
{
    public class DistantAttack : RAttack
    {
        [SerializeField] private Projectile projectilePrefab = null!;
        private ObjectPool<Projectile> projectilesPool = null!;

        private void Awake()
        {
            projectilesPool = new ObjectPool<Projectile>(OnCreateProjectile, OnGetProjectile, OnReleaseProjectile, null,
                true, 5, 40);
        }


        public override void PerformOneShotAttack()
        {
            if (UserTransform == null)
            {
                Debug.LogError("Player transform is null.");
                return;
            }

            if (Manager == null)
            {
                Debug.LogError("GAMEMANAGER IS NULL!");
                return;
            }

            Projectile proj = projectilesPool.Get();
            float angle = Vector2.SignedAngle(Vector2.right, GetLookDirection());

            proj.transform.SetPositionAndRotation(UserTransform.position + data.range
                * (Vector3) GetLookDirection(),
                Quaternion.Euler(0, 0, angle));
            proj.Fire();
            //OnAttackEnd?.Invoke();
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
                Debug.LogError("GAMEMANAGER IS NULL!");
                yield break;
            }

            while (true)
            {
                Projectile proj = projectilesPool.Get();
                float angle = Vector2.SignedAngle(Vector2.right, GetLookDirection());

                proj.transform.SetPositionAndRotation(UserTransform.position + data.range
                    * (Vector3) GetLookDirection(),
                    Quaternion.Euler(0, 0, angle));
                proj.Fire();

                yield return waitForAttackRefresh;
            }
        }


        private Projectile OnCreateProjectile()
        {
            if (Manager == null)
            {
                Debug.LogError("GAMEMANAGER IS NULL!");
                return default!;
            }

            Projectile proj = Instantiate(projectilePrefab, Manager.transform).GetComponent<Projectile>();
            proj.name = $"Projectile {projectilesPool.CountAll + 1}";
            proj.Init(() => projectilesPool.Release(proj));

            proj.damage = data.damage;

            return proj;
        }

        private void OnGetProjectile(Projectile _proj)
        {
            _proj.gameObject.SetActive(true);
        }

        private void OnReleaseProjectile(Projectile _proj)
        {
            _proj.gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (UserTransform == null || Manager == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(UserTransform.position + data.range * (Vector3) GetLookDirection(), 0.2f);
        }
#endif
    }
}