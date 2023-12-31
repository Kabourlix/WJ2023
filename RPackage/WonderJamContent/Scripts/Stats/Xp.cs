﻿// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

namespace Rezoskour.Content
{
    public class Xp : MonoBehaviour
    {
        private int currentlevel, totalExperience;
        private int previousLevelExperience, nextLevelExperience;

        private bool isBerserk;

        //[SerializeField] private AnimationCurve xpCurve = null!;
        [SerializeField] private Slider? xpBar;
        [SerializeField] private TextMeshProUGUI levelTxt = null!;

        private int XpRequiredForLevel(int _level)
        {
            return Mathf.FloorToInt(30 * 5 * Mathf.Pow(2, _level));
        }

        private void Start()
        {
            currentlevel = 1;
            totalExperience = 0;
            isBerserk = false;
            UpdateLevel();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnBerserkModeChange += SwitchMode;
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnBerserkModeChange -= SwitchMode;
            }
        }

        private void SwitchMode(bool _obj)
        {
            isBerserk = _obj;
            if (!isBerserk && GameManager.Instance.CurrentState == GameStateName.Main)
            {
                CheckForLevelUp();
            }
        }

        public void AddXp(int _xp)
        {
            totalExperience += _xp;
            LeanTween.value(xpBar.value, totalExperience, 0.5f).setOnUpdate((float _value) => { xpBar.value = _value; })
                .setOnComplete(
                    () => { CheckForLevelUp(); });
        }

        private void CheckForLevelUp()
        {
            if (totalExperience >= nextLevelExperience)
            {
                if (isBerserk)
                {
                    return;
                }

                currentlevel++;
                UpdateLevel();
                //sound

                //Loot
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ChangeState(GameStateName.LevelUp);
                }
            }
        }

        private void UpdateLevel()
        {
            previousLevelExperience = XpRequiredForLevel(currentlevel);
            nextLevelExperience = XpRequiredForLevel(currentlevel + 1);
            UpdateUI();
        }

        private void UpdateUI()
        {
            int start = previousLevelExperience;
            int end = nextLevelExperience;

            if (xpBar == null)
            {
                return;
            }

            xpBar.minValue = start;
            xpBar.maxValue = end;

            xpBar.value = previousLevelExperience;
            levelTxt.text = $"Lvl {currentlevel}";
        }
    }
}