// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Rezoskour.Content
{
    public class HealthPlayerUIUpdate : MonoBehaviour
    {
        [SerializeField] private Slider hpBar;
        [SerializeField] private HealthManager healthManager;
        [SerializeField] private TextMeshProUGUI healthTxt;
        [SerializeField] private Image healthImage;

        // Start is called before the first frame update
        private void Start()
        {
        }

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
                StartCoroutine(BlinkHealth());
                healthTxt.text = $"{_newHealth}";
            }
        }

        private IEnumerator BlinkHealth()
        {
            var time = 0f;
            while (time <= 1f)
            {
                time += Time.deltaTime * 1.5f;
                healthTxt.color = Color.black;
                healthImage.color = Color.black;
                yield return new WaitForSeconds(0.2f);
                healthTxt.color = Color.white;
                healthImage.color = Color.white;
                yield return new WaitForSeconds(0.2f);
            }

            healthTxt.color = Color.white;
            healthImage.color = Color.white;
        }
    }
}