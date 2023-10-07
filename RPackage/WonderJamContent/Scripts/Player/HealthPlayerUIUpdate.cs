// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Rezoskour.Content
{
    public class HealthPlayerUIUpdate : MonoBehaviour
    {
        public Slider hpBar;
        public HealthManager healthManager;
        public float maxHealth = 50;
        public Text healthTxt;
        public GameManager gameManager;

        private bool isBerzerk = false;

        // Start is called before the first frame update
        private void Start()
        {
            gameManager.OnBerserkModeChange += Switch;
        }

        private void OnEnable()
        {
            healthManager.OnHealthChanged += UpdateHealth;
        }

        private void OnDisable()
        {
            healthManager.OnHealthChanged -= UpdateHealth;
        }

        private void UpdateHealth(int _obj)
        {
            healthTxt.text = _obj.ToString();
            if (healthManager.Health <= 0)
            {
                healthTxt.text = "0";
            }
            else
            {
                hpBar.value = healthManager.Health / maxHealth;
                Debug.Log(hpBar.value);
                healthTxt.text = $"{healthManager.Health}/{maxHealth}";
            }
        }


        private void Switch(bool value)
        {
            isBerzerk = value;
        }
    }
}