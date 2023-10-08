// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Rezoskour.Content
{
    public class OilPlayerUIUpdate : MonoBehaviour
    {
        [SerializeField] private OilComponent oilComponent = null!;

        [FormerlySerializedAs("HungerBar")] [SerializeField]
        private Slider hungerBar = null!;

        [SerializeField] private TextMeshProUGUI hungerTxt;

        //Play Burn animation when in Berserk mode
        [SerializeField] private GameObject burnAnimator = null!;

        //Detect actual controller
        private bool isUsingGamepad = false;
        private string currentController = null;
        [SerializeField] private Sprite basePotato;
        [SerializeField] private Sprite BurnedPotato;
        [SerializeField] private Sprite KeyboardControls;
        [SerializeField] private Sprite GamepadControls;
        [SerializeField] private Image ControlsToShow;
        [SerializeField] private Image PotatoColdown;

        //Blink Animation parameter
        [SerializeField] private Image hungerImage;
        [SerializeField] private Image hungerBgImage;
        [SerializeField] private Color startColor;
        [SerializeField] private Color endColor;
        [Range(0, 10)] [SerializeField] private float speed = 1f;
        private bool isCritical = false;
        private bool isBurning = false;

        // Start is called before the first frame update
        private void Start()
        {
            oilComponent.OnOilModified += UpdateOilUI;
            oilComponent.OnOilCritical += BlinkUI;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnBerserkModeChange += ActivateBurnAnimation;
            }
        }

        private void UpdateOilUI(int _hungerValue)
        {
            hungerTxt.text = oilComponent.Oil.ToString("F0");
            hungerBar.value = oilComponent.OilPercentage;
        }

        private void BlinkUI(bool _isCritical)
        {
            isCritical = _isCritical;
            if (!_isCritical)
            {
                hungerImage.color = startColor;
                hungerBgImage.color = startColor;
                hungerTxt.color = Color.black;
            }
        }

        private void Update()
        {
            if (isCritical)
            {
                hungerImage.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
                hungerBgImage.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
                hungerTxt.color = Color.Lerp(Color.black, endColor, Mathf.PingPong(Time.time * speed, 1));
            }

            if (isBurning)
            {
                hungerBgImage.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
            }

            if (Gamepad.current != null && Keyboard.current != null)
            {
                if (isUsingGamepad && currentController != "Keyboard")
                {
                    if (Keyboard.current.wasUpdatedThisFrame)
                    {
                        isUsingGamepad = false;
                        currentController = "Keyboard";
                    }
                }
                else if (!isUsingGamepad && currentController != "Gamepad")
                {
                    if (Gamepad.current.wasUpdatedThisFrame)
                    {
                        isUsingGamepad = true;
                        currentController = "Gamepad";
                    }
                }

                switch (currentController)
                {
                    case "Keyboard":
                        ControlsToShow.sprite = KeyboardControls;
                        break;
                    case "Gamepad":
                        ControlsToShow.sprite = GamepadControls;
                        break;
                }
            }
        }

        private void ActivateBurnAnimation(bool _isBerserk)
        {
            burnAnimator.SetActive(_isBerserk);
            isBurning = _isBerserk;
            if (_isBerserk)
            {
                PotatoColdown.sprite = BurnedPotato;
            }
            if (!_isBerserk)
            {
                hungerBgImage.color = startColor;
                PotatoColdown.sprite = basePotato;
            }
        }

        private void OnDestroy()
        {
            oilComponent.OnOilModified -= UpdateOilUI;
            oilComponent.OnOilCritical -= BlinkUI;
        }
    }
}