// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 06

#nullable enable

using UnityEngine;
using UnityEngine.Serialization;

namespace Rezoskour.Content
{
    public enum AttackName
    {
        Basic = 0,
        Range = 1,
        BerserkBurn = 2,
        Tourniquet = 3
    }

    [CreateAssetMenu(fileName = "AttackData", menuName = "Rezoskour/AttackData", order = 0)]
    public class AttackData : ScriptableObject
    {
        public AttackName attackName;
        public int damage;

        [FormerlySerializedAs("attackCooldown")] [FormerlySerializedAs("attackRate")]
        public float attackSpeed;

        [Header("Melee")] [FormerlySerializedAs("maxRange")]
        public float range;

        [Tooltip("The range of the collider to use for hit detection.")]
        public float attackAreaRange;
    }
}