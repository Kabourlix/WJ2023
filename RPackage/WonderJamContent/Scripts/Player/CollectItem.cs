// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

#nullable enable

using System;
using Rezoskour.Content.Collectable;
using UnityEngine;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

namespace Rezoskour.Content
{
    public class CollectItem : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D collectRange;
        [SerializeField] private LayerMask collectLayer;
        [SerializeField] private float collectRangeRadius;
        private PlayerStats _stats;
        private HealthManager _healthManager;
        private Xp _xp;

        private void Start()
        {
            _stats = GetComponentInParent<PlayerStats>();
            _healthManager = GetComponentInParent<HealthManager>();
            _xp = GetComponentInParent<Xp>();

            collectRangeRadius = _stats.CurrentStats.collectRange;
            collectRange.radius = collectRangeRadius;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Collision");
            if (other.gameObject.layer == collectLayer)
            {
                Debug.Log("Collecting item");
                var obj = other.gameObject.GetComponent<CollectableItem>();
                switch (obj.Type)
                {
                    case CollectableType.Experience:
                        _xp.AddXp(obj.Value);
                        break;
                }

                Destroy(other.gameObject);
            }
        }
    }
}