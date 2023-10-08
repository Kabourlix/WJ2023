// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

#nullable enable

using System;
using UnityEngine;

namespace Rezoskour.Content
{
    public class OilComponent : MonoBehaviour
    {
        public event Action? OnOilExhausted;
        public event Action<int>? OnOilModified; //new value of oilFloat as parameter
        public event Action<bool>? OnOilCritical;

        private PlayerStats stats = null!;

        private float oilFloat;
        private bool isCritical;

        public int Oil
        {
            get => stats.CurrentStats.oil;
            set => stats.CurrentStats.oil = value;
        }

        public int MaxOil => stats.CurrentStats.maxOil;

        public float OilPercentage => (float)Oil / MaxOil;

        private void Start()
        {
            stats = GetComponent<PlayerStats>();

            if (MaxOil == 0)
            {
                Debug.LogError("Max Oil has not been set.");
                return;
            }

            if (MaxOil <= 0)
            {
                Debug.LogError("Max Oil cannot be negative.");
                return;
            }

            Oil = MaxOil;
            oilFloat = Oil;
        }

        public void LooseOil(float _amount)
        {
            if (MaxOil == 0)
            {
                Debug.LogError("Max Oil has not been set.");
                return;
            }

            if (_amount < 0)
            {
                Debug.LogError("Damage amount must be greater than 0.");
                return;
            }

            if (Oil == 0)
            {
                return;
            }

            if (!isCritical && Oil <= MaxOil / 4)
            {
                isCritical = true;
                OnOilCritical?.Invoke(true);
            }

            OnOilModified?.Invoke(Oil);
            oilFloat = Mathf.Clamp(oilFloat - _amount, 0, MaxOil);
            Oil = Mathf.CeilToInt(oilFloat);
            OnOilModified?.Invoke(Oil);

            if (Oil != 0)
            {
                return;
            }

            OnOilExhausted?.Invoke();
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null !");
                return;
            }

            GameManager.Instance.ChangeState(GameStateName.Defeat);
        }

        public void RefillOil(int _amount)
        {
            if (MaxOil == 0)
            {
                Debug.LogError("Max Health has not been set.");
                return;
            }

            if (_amount < 0)
            {
                Debug.LogError("Heal amount must be greater than 0.");
                return;
            }

            if (isCritical && Oil >= MaxOil / 4)
            {
                isCritical = false;
                OnOilCritical?.Invoke(false);
            }

            oilFloat = Mathf.Clamp(oilFloat + _amount, 0, MaxOil);
            Oil = Mathf.CeilToInt(oilFloat);
            OnOilModified?.Invoke(Oil);
        }
    }
}