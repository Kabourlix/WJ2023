// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

#nullable enable

using System;
using UnityEngine;
using TMPro;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

namespace Rezoskour.Content
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private Stats? baseStats;
        private Stats? normalStats;
        private Stats? currentStats;
        public Stats? CurrentStats => currentStats;

        [SerializeField] private TextMeshProUGUI statsText;

        private void Start()
        {
            if (baseStats == null)
            {
                throw new Exception("Stats not set in PlayerStats");
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnBerserkModeChange += SwitchMode;
            }

            currentStats = Instantiate(baseStats);
            ShowStats();
        }

        private void SwitchMode(bool _isBerserk)
        {
            Debug.Log("Switching mode");
            if (currentStats == null)
            {
                throw new Exception("Stats not set in PlayerStats");
            }

            if (_isBerserk)
            {
                normalStats = Instantiate(currentStats);
                currentStats.BerserkMode();
                Debug.Log("<color=red>Is Berserk</color>");
            }
            else
            {
                currentStats = normalStats;
            }

            ShowStats();
        }

        private void ShowStats()
        {
            statsText.text = "Attack : " + currentStats.attack + "\n Speed : " + currentStats.speed + "\n Range : " +
                             currentStats.range + "\n CollectRange : " + currentStats.collectRange;
        }
    }
}