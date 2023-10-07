// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

#nullable enable

using System;
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
        [SerializeField] private AnimationCurve xpCurve;
        [SerializeField] private Slider xpBar;

        private void Start()
        {
            currentlevel = 1;
            totalExperience = 0;
            isBerserk = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnBerserkModeChange += SwitchMode;
            }
        }

        private void SwitchMode(bool _obj)
        {
            isBerserk = _obj;
            if (!isBerserk)
            {
                CheckForLevelUp();
            }
        }

        public void AddXp(int _xp)
        {
            totalExperience += _xp;
            CheckForLevelUp();
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
            }
        }

        private void UpdateLevel()
        {
            previousLevelExperience = (int)xpCurve.Evaluate(currentlevel);
            nextLevelExperience = (int)xpCurve.Evaluate(currentlevel + 1);
            UpdateUI();
        }

        private void UpdateUI()
        {
            var start = previousLevelExperience;
            var end = nextLevelExperience;

            xpBar.minValue = start;
            xpBar.maxValue = end;

            xpBar.value = totalExperience;
        }

        public int GetLevel()
        {
            return currentlevel;
        }
    }
}