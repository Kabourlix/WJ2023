// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 05

#nullable enable

using System;
using UnityEngine;

namespace Rezoskour.Content
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager? Instance;

        public event Action<bool>? OnBerserkModeChange;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"{nameof(GameManager)} cannot be instanced more than once.");
                Destroy(gameObject);
            }

            Instance = this;
        }

        public void SetBerserk(bool _b)
        {
            OnBerserkModeChange?.Invoke(_b);
        }
    }
}