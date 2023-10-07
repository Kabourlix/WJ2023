// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Rezoskour.Content
{
    public class HealthPlayerUIUpdate : MonoBehaviour
    {
        [SerializeField] private Slider hpBar;
        [SerializeField] private HealthManager healthManager;
        [SerializeField] private Text healthTxt;

        // Start is called before the first frame update
        private void Start() { }

        private void OnEnable()
        {
            healthManager.OnHealthChanged += UpdateHealth;
        }

        private void OnDisable()
        {
            healthManager.OnHealthChanged -= UpdateHealth;
        }

        private void UpdateHealth(int _newHealth)
        {
            healthTxt.text = _newHealth.ToString();
            if (healthManager.Health <= 0)
            {
                healthTxt.text = "0";
                hpBar.value = 0;
            }
            else
            {
                hpBar.value = healthManager.HealthPercent;
                Debug.Log(hpBar.value);
                healthTxt.text = $"{_newHealth}/{healthManager.MaxHealth}";
            }
        }
    }
}