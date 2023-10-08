// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

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
        private AttackManager _weapon;
        public event Action<Boolean>? OnGetMeleeWeapon;
        private void Start()
        {
            OnGetMeleeWeapon?.Invoke(false);
            _stats = GetComponentInParent<PlayerStats>();
            _healthManager = GetComponentInParent<HealthManager>();
            _xp = GetComponentInParent<Xp>();
            _oil = GetComponentInParent<OilComponent>();
            _weapon = GetComponentInParent<AttackManager>();
            collectRangeRadius = _stats.CurrentStats.collectRange;
            collectRange.radius = collectRangeRadius;
        }

        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (collectLayer.Has(_other.gameObject.layer))
            {
                Debug.Log("Collecting item");
                CollectableItem? obj = _other.gameObject.GetComponent<CollectableItem>();
                switch (obj.Type)
                {
                    case CollectableType.Experience:
                        obj.gainCallback = () => _xp.AddXp(obj.Value);
                        break;
                    case CollectableType.Health:
                        obj.gainCallback = () => _healthManager.Heal(obj.Value);
                        break;
                    case CollectableType.Oil:
                        obj.gainCallback = () => _oil.RefillOil(obj.Value);
                        break;
                    case CollectableType.Weapon:
                        OnGetMeleeWeapon?.Invoke(true);
                        obj.gainCallback = () => _weapon.TryAddAttack((AttackName)obj.Value,true);
                        break;
                }

                obj.GoToPlayer = true;
                //obj.ReturnToPool();
            }
        }
    }
}