// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

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
        private const int PLAYER_LAYER = 6;
        [SerializeField] private float smoothTime = 0.1f;
        [SerializeField] private CollectableType type;
        public CollectableType Type => type;
        [SerializeField] private int value;
        public int Value => value;
        public bool GoToPlayer { get; set; } = false;
        private Transform playerTf;

        protected IObjectPool<CollectableItem> Pool;
        private Vector2 velocity;
        public Action? gainCallback;

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null !");
                return;
            }

            playerTf = GameManager.Instance.PlayerTf;
        }

        public void Init(IObjectPool<CollectableItem> _pool)
        {
            Pool = _pool;
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null !");
                return;
            }
        }

        private void Update()
        {
            if (!GoToPlayer)
            {
                return;
            }

            Vector2 target = Vector2.SmoothDamp(transform.position, playerTf.position, ref velocity, smoothTime);
            transform.position = target;
        }

        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (_other.gameObject.layer == PLAYER_LAYER)
            {
                ReturnToPool();
            }
        }


        public void ReturnToPool()
        {
            gainCallback?.Invoke();
            velocity = Vector2.zero;
            GoToPlayer = false;
            CollectableManager.ReturnObjectToPool(gameObject);
        }
    }
}