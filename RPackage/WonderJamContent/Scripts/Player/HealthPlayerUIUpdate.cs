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
                StopCoroutine(BlinkHealth(1f));
                StartCoroutine(BlinkHealth(1f));
                healthTxt.text = $"{_newHealth}";
            }
        }

        private IEnumerator BlinkHealth(float _time)
        {
            healthTxt.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * 1.5f, 1));
            healthImage.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * 1.5f, 1));
            yield return new WaitForSeconds(_time);
            healthTxt.color = Color.white;
            healthImage.color = Color.white;
        }
    }
}