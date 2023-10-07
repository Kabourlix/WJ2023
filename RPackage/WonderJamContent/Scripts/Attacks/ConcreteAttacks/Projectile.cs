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
        [SerializeField] private LayerMask layerMask;
        public float speed;
        [HideInInspector] public int damage;
        public float duration;

        private bool isMoving;
        private float releaseTime;

        private Action? releaseCallback;

        public void Init(Action _releaseMethod)
        {
            releaseCallback = _releaseMethod;
        }

        public void Fire()
        {
            isMoving = true;
            releaseTime = Time.realtimeSinceStartup + duration;
        }

        private void Release()
        {
            isMoving = false;
            releaseTime = 0;
            releaseCallback?.Invoke();
        }

        private void Update()
        {
            if (!isMoving)
            {
                return;
            }

            transform.Translate(speed * Time.deltaTime * transform.forward);
        }

        private void OnCollisionEnter2D(Collision2D _other)
        {
            //Check if other belongs to layerMask
            if (layerMask.HasNot(_other.gameObject.layer))
            {
                return;
            }

            if (!_other.gameObject.TryGetComponent(out HealthManager health))
            {
                return;
            }

            health.Damage(damage);
            Release();
        }
    }
}