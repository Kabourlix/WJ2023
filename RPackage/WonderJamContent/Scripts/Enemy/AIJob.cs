#nullable enable

using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;

// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

namespace Rezoskour.Content
{


    public struct Enemy : IComponentData
    {
        public float speed;
        public float attackRange;
    }
}