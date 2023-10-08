// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 08

#nullable enable

using System;
using Rezoskour.Content.Misc;
using UnityEngine;

namespace Rezoskour.Content
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Tourniquet : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float initAngle;
        [SerializeField] private float rotationSpeed;

        private float currentAngle;
        private RAttack attack = null!;
        private Transform playerTf = null!;
        [SerializeField] private float radius;

        private void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
            Rigidbody2D? rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            attack = GetComponentInParent<RAttack>();

            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null !");
                return;
            }

            currentAngle = initAngle;
            playerTf = GameManager.Instance.PlayerTf;
        }

        private void Update()
        {
            if (playerTf == null)
            {
                Debug.LogError("Player transform is null.");
                return;
            }


            currentAngle += rotationSpeed * Time.deltaTime;
            Vector2 pos = (Vector2) playerTf.position + radius * new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad));
            transform.position = pos;
        }

        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (layerMask.HasNot(_other.gameObject.layer))
            {
                return;
            }

            if (!_other.TryGetComponent(out IHealth health))
            {
                return;
            }

            health.Damage(Mathf.FloorToInt(attack.AttackDamage));
        }
    }
}