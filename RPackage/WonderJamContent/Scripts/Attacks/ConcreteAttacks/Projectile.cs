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

            CdSystem.StartTimer(TimerName);
        }

        private void Release()
        {
            isMoving = false;
            if (CdSystem == null)
            {
                Debug.LogError("CRITICAL !!! CoolDownSystem is null.");
                return;
            }

            CdSystem.StopTimer(TimerName);
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

        private void OnCollisionEnter2D(Collision2D _other)
        {
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