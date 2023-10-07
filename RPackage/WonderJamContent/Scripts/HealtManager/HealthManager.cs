// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

#nullable enable

using System;
using UnityEngine;
using UnityEngine.Serialization;

// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

namespace Rezoskour.Content
{
    public class HealthManager : MonoBehaviour
    {
        // public int Health { get; private set; }
        //
        // public int MaxHealth { get; private set; }
        public event Action? OnDeath;
        public event Action? OnIncomingDamage;
        public event Action<int>? OnHealthChanged;

        [FormerlySerializedAs("Health")] public int health = 0;
        [FormerlySerializedAs("MaxHealth")] public int maxHealth = 50;


        public float HealthPercent
        {
            get
            {
                if (maxHealth == 0)
                {
                    Debug.LogError("Max Health has not been set.");
                    return 0;
                }

                return (float)health / maxHealth;
            }
        }

        private void Start()
        {
            // int healthPlayer = GetComponents<PlayerStats>();
            if (maxHealth == 0)
            {
                throw new NullReferenceException("Max Health has not been set.");
            }

            health = maxHealth;
        }

        public void Init(int _maxHealth)
        {
            if (maxHealth != 0)
            {
                throw new Exception("Max health has already been set.");
            }

            if (_maxHealth <= 0)
            {
                throw new Exception("Max health must be greater than 0.");
            }

            maxHealth = _maxHealth;
            health = maxHealth;
        }

        public void Damage(int _damage)
        {
            if (maxHealth == 0)
            {
                throw new NullReferenceException("Max Health has not been set.");
            }

            if (_damage < 0)
            {
                throw new Exception("Damage amount must be greater than 0.");
            }

            if (health == 0)
            {
                return;
            }

            OnIncomingDamage?.Invoke();
            health = Mathf.Clamp(health - _damage, 0, maxHealth);
            OnHealthChanged?.Invoke(health);
            if (health == 0)
            {
                OnDeath?.Invoke();
            }
        }

        public void Heal(int _amount)
        {
            health = Mathf.Clamp(health + _amount, 0, maxHealth);
            OnHealthChanged?.Invoke(health);
        }
    }
}