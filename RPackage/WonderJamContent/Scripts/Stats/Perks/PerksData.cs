// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rezoskour.Content.Perks
{
    [CreateAssetMenu(fileName = "Perks", menuName = "Rezoskour/PerksData", order = 0)]
    public class PerksData : ScriptableObject
    {
        [SerializeField] private Sprite statImage;
        public Sprite StatImage => statImage;

        [SerializeField] private string statTitle;
        public string StatTitle => statTitle;

        [SerializeField] private string statDescription;
        public string StatDescription => statDescription;

        [SerializeField] private StatName statName;
        public StatName StatName => statName;

        [SerializeField] private float multiplier;
        public float Multiplier => multiplier;

        [SerializeField] private float addValue;
        public float AddValue => addValue;
    }
}