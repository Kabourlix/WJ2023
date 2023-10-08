// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using UnityEngine;
using UnityEngine.Serialization;

// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

namespace Rezoskour.Content
{
    public class HealthManager : MonoBehaviour, IHealth
    {
        public event Action? OnDeath;
        public event Action? OnIncomingDamage;
        public event Action<int>? OnHealthChanged;

        private PlayerStats stats = null!;

        public int Health
        {
            get => stats.CurrentStats.health;
            set => stats.CurrentStats.health = value;
        }

        public int MaxHealth => stats.CurrentStats.maxHealth;


        public float HealthPercent
        {
            get
            {
                if (MaxHealth == 0)
                {
                    Debug.LogError("Max Health has not been set.");
                    return 0;
                }

                return (float) Health / MaxHealth;
            }
        }

        private void Start()
        {
            stats = GetComponent<PlayerStats>();

            if (MaxHealth == 0)
            {
                Debug.LogError("Max Health has not been set.");
                return;
            }

            if (MaxHealth <= 0)
            {
                Debug.LogError("Max Health cannot be negative.");
                return;
            }

            Heal(MaxHealth);

            GetComponent<OilComponent>().OnOilExhausted += OnOilExhaustedCallback;
        }

        private void OnOilExhaustedCallback()
        {
            Health = 0;
            OnHealthChanged?.Invoke(Health);
        }

        public void Heal(int _amount)
        {
            if (MaxHealth == 0)
            {
                Debug.LogError("Max Health has not been set.");
                return;
            }

            if (_amount < 0)
            {
                Debug.LogError("Heal amount must be greater than 0.");
                return;
            }

            Health = Mathf.Clamp(Health + _amount, 0, MaxHealth);
            OnHealthChanged?.Invoke(Health);
        }

        public void Damage(int _damage)
        {
            if (MaxHealth == 0)
            {
                Debug.LogError("Max Health has not been set.");
                return;
            }

            if (_damage < 0)
            {
                Debug.LogError("Damage amount must be greater than 0.");
                return;
            }

            if (Health == 0)
            {
                return;
            }

            int realDamage = Mathf.FloorToInt(_damage * stats.CurrentStats.globalDamageReductor);

            OnIncomingDamage?.Invoke();
            Health = Mathf.Clamp(Health - realDamage, 0, MaxHealth);
            OnHealthChanged?.Invoke(Health);
            if (Health != 0)
            {
                return;
            }

            OnDeath?.Invoke();
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null !");
                return;
            }

            GameManager.Instance.ChangeState(GameStateName.Defeat);
        }
    }
}