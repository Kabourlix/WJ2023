// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

using UnityEngine;

namespace Rezoskour.Content.Perks
{
    [CreateAssetMenu(fileName = "Perks", menuName = "Rezoskour/PerksData", order = 0)]
    public class PerksData : ScriptableObject
    {
        [SerializeField] private StatName statName;
        public StatName StatName => statName;

        [SerializeField] private float multiplier;
        public float Multiplier => multiplier;

        [SerializeField] private float addValue;
        public float AddValue => addValue;
    }
}