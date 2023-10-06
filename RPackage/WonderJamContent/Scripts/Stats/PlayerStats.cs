// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

#nullable enable

using System;
using UnityEngine;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

namespace Rezoskour.Content
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private Stats? baseStats;
        private Stats? normalStats;
        private Stats? currentStats;
        private bool isBerserk;

        private void Start()
        {
            if (baseStats == null)
            {
                throw new Exception("Stats not set in PlayerStats");
            }

            //baseStats.SaveStatsToJson();
            currentStats = Instantiate(baseStats);
            //currentStats.LoadStatsFromJson();
        }

        private void SwitchMode(bool _isBerserk)
        {
            if (currentStats == null)
            {
                throw new Exception("Stats not set in PlayerStats");
            }

            if (_isBerserk)
            {
                Destroy(normalStats);
                normalStats = Instantiate(currentStats);
                currentStats.BerserkMode();
            }
            else
            {
                currentStats = normalStats;
            }
        }
    }
}