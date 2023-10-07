// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

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


        public override IEnumerator PerformCoroutine()
        {
            if (TargetTransform == null)
            {
                Debug.LogError("Player transform is null.");
                yield break;
            }

            while (true)
            {
                Projectile proj = projectilesPool.Get();
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
            if (TargetTransform == null)
            {
                return;
            }

            if (Manager == null)
            {
                Debug.LogError("GAMEMANAGER IS NULL!");
                return;
            }

            _proj.gameObject.SetActive(true);
            _proj.transform.SetPositionAndRotation(TargetTransform.position + data.range
                * Manager.PlayerLookDirection,
                Quaternion.identity);
        }

        private void OnReleaseProjectile(Projectile _proj)
        {
            _proj.gameObject.SetActive(false);
        }
    }
}