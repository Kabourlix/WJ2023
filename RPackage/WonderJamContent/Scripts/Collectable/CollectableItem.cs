// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

#nullable enable

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

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

        protected IObjectPool<CollectableItem> _pool;

        public void Init(IObjectPool<CollectableItem> pool)
        {
            _pool = pool;
        }

        public void ReturnToPool()
        {
            CollectableManager.ReturnObjectToPool(gameObject);
        }
    }
}