// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

#nullable enable

using System;
using UnityEngine;

// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

namespace Rezoskour.Content
{
    public class HealthManager : MonoBehaviour
    {
        // public int Health { get; private set; }
        //
        // public int MaxHealth { get; private set; }
        public int Health = 0;
        public int MaxHealth = 50;

        public float HealthPercent
        {
            get
            {
                if (MaxHealth == 0)
                {
                    throw new NullReferenceException("Max Health has not been set.");
                }

                return (float) Health / MaxHealth;
            }
        }

        private void Start()
        {
            // int healthPlayer = GetComponents<PlayerStats>();
            if (MaxHealth == 0)
            {
                throw new NullReferenceException("Max Health has not been set.");
            }

            Health = MaxHealth;
        }

        public void Init(int maxHealth)
        {
            if (MaxHealth != 0)
            {
                throw new Exception("Max health has already been set.");
            }

            if (maxHealth <= 0)
            {
                throw new Exception("Max health must be greater than 0.");
            }

            MaxHealth = maxHealth;
            Health = MaxHealth;
        }

        public event Action OnDeath;
        public event Action OnIncomingDamage;
        public event Action<int> OnHealthChanged;

        public void Damage(int damage)
        {
            if (MaxHealth == 0)
            {
                throw new NullReferenceException("Max Health has not been set.");
            }

            if (damage < 0)
            {
                throw new Exception("Damage amount must be greater than 0.");
            }

            if (Health == 0)
            {
                return;
            }

            OnIncomingDamage?.Invoke();
            Health = Mathf.Clamp(Health - damage, 0, MaxHealth);
            OnHealthChanged?.Invoke(Health);
            if (Health == 0)
            {
                OnDeath?.Invoke();
            }
        }
    }
}