// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 06

#nullable enable

using UnityEngine;
using UnityEngine.Serialization;

namespace Rezoskour.Content
{
    public enum AttackName
    {
        Basic,
        Range,
        LaMortQuiTue
    }

    [CreateAssetMenu(fileName = "AttackData", menuName = "Rezoskour/AttackData", order = 0)]
    public class AttackData : ScriptableObject
    {
        public AttackName attackName;
        public float damage;
        [FormerlySerializedAs("maxRange")] public float range;
        [FormerlySerializedAs("attackRate")] public float attackCooldown;

        [Tooltip("The range of the collider to use for hit detection.")]
        public float attackAreaRange;
    }
}