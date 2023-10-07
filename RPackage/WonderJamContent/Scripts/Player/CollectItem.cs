// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

#nullable enable

using System;
using Rezoskour.Content.Collectable;
using Rezoskour.Content.Misc;
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
        private OilComponent _oil;

        private void Start()
        {
            _stats = GetComponentInParent<PlayerStats>();
            _healthManager = GetComponentInParent<HealthManager>();
            _xp = GetComponentInParent<Xp>();
            _oil = GetComponentInParent<OilComponent>();

            collectRangeRadius = _stats.CurrentStats.collectRange;
            collectRange.radius = collectRangeRadius;
        }

        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (collectLayer.Has(_other.gameObject.layer))
            {
                Debug.Log("Collecting item");
                var obj = _other.gameObject.GetComponent<CollectableItem>();
                switch (obj.Type)
                {
                    case CollectableType.Experience:
                        _xp.AddXp(obj.Value);
                        break;
                    case CollectableType.Health:
                        _healthManager.Heal(obj.Value);
                        break;
                    case CollectableType.Oil:
                        _oil.RefillOil(obj.Value);
                        break;
                }

                obj.ReturnToPool();
            }
        }
    }
}