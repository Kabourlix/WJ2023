// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;
using Rezoskour.Content.Misc;
using UnityEngine;

namespace Rezoskour.Content
{
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        private const string TIMER_PREFIX = "Projectile_";
        private int currentAmount;
        private static int amount;
        private CoolDownSystem? CdSystem => CoolDownSystem.Instance;

        private string TimerName => TIMER_PREFIX + currentAmount;

        [SerializeField] private LayerMask layerMask;
        public float speed;
        [HideInInspector] public int damage;
        public float duration;
        public Animator animator;
        private bool isMoving;

        private Action? releaseCallback;

        private void OnDestroy()
        {
            amount--;
            if (CdSystem == null)
            {
                Debug.LogError("CRITICAL !!! CoolDownSystem is null.");
                return;
            }

            CdSystem.TryUnRegisterCoolDown(TimerName);
        }

        public void Init(Action _releaseMethod)
        {
            amount++;
            currentAmount = amount;
            releaseCallback = _releaseMethod;
            if (CdSystem == null)
            {
                Debug.LogError("CRITICAL !!! CoolDownSystem is null.");
                return;
            }

            CdSystem.TryRegisterCoolDown(TimerName, duration);
        }

        public void Fire()
        {
            isMoving = true;
            if (CdSystem == null)
            {
                Debug.LogError("CRITICAL !!! CoolDownSystem is null.");
                return;
            }

            CdSystem.StartCoolDown(TimerName);
        }

        private void Release()
        {
            isMoving = false;
            if (CdSystem == null)
            {
                Debug.LogError("CRITICAL !!! CoolDownSystem is null.");
                return;
            }

            CdSystem.StopCoolDown(TimerName);
            releaseCallback?.Invoke();
        }

        private void FixedUpdate()
        {
            if (!isMoving)
            {
                return;
            }

            if (CdSystem == null)
            {
                Debug.LogError("CRITICAL !!! CoolDownSystem is null.");
                return;
            }

            if (CdSystem.IsCoolDownDone(TimerName))
            {
                Release();
            }

            transform.Translate(speed * Time.fixedDeltaTime * Vector2.right);
        }

        private void OnTriggerEnter2D(Collider2D _other)
        {
            //Debug.Log($"Projectile hit something with layer {_other.gameObject.layer}");
            //Check if other belongs to layerMask
            if (layerMask.HasNot(_other.gameObject.layer))
            {
                return;
            }

            if (!_other.gameObject.TryGetComponent(out IHealth health))
            {
                return;
            }

            health.Damage(damage);
            Release();
        }
    }
}