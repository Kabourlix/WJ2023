// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

#nullable enable

using System;
using UnityEditor;
using UnityEngine;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

namespace Rezoskour.Content.Collectable
{
    public enum CollectableType
    {
        Health,
        Oil,
        Experience
    }

    public class CollectableItem : MonoBehaviour
    {
        [SerializeField] private CollectableType type;
        public CollectableType Type => type;
        [SerializeField] private int value;
        public int Value => value;
    }
}