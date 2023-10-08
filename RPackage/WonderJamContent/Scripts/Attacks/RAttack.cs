// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 06

#nullable enable

using System;
using System.Collections;
using UnityEngine;

namespace Rezoskour.Content
{
    [Serializable]
    public abstract class RAttack : MonoBehaviour
    {
        [SerializeField] protected AttackData data = null!;
        protected Stats? playerStats;
        public AttackName Name => data.attackName;
        public float AttackSpeed => data.attackSpeed;

        public float AttackDamage
        {
            get
            {
                float dmg = data.damage;
                if (playerStats != null)
                {
                    dmg *= playerStats.globalDamageMultiplier;
                }

                return dmg;
            }
        }

        public Transform? UserTransform { get; set; }
        protected GameManager? Manager => GameManager.Instance;

        protected LayerMask layerMask;

        private Coroutine? runningCoroutine;
        protected WaitForSeconds waitForAttackRefresh = null!;

        protected Func<Vector2> GetLookDirection = null!;
        protected Action? OnAttackEnd = null!;

        public virtual void Initialize(Transform _userTf,
            LayerMask _layerMask,
            Func<Vector2> _lookDirectionMethod,
            bool _startCoroutine = true,
            Action? _attackEndCallback = null,
            Stats _playerStats = null)
        {
            if (UserTransform != null)
            {
                Debug.LogError($"Cannot initialize twice Player transform on {name}.");
                return;
            }

            layerMask = _layerMask;
            waitForAttackRefresh = new WaitForSeconds(1 / data.attackSpeed);
            UserTransform = _userTf;
            GetLookDirection = _lookDirectionMethod;
            OnAttackEnd = _attackEndCallback;
            playerStats = _playerStats;

            if (_startCoroutine)
            {
                StartAttacking();
            }
        }

        public void UpdateStats(Stats? _stats)
        {
            StopAttacking();
            playerStats = _stats;
            float rate = data.attackSpeed;
            if (_stats != null)
            {
                rate *= _stats.globalAttackRateMultiplier;
            }

            //data = _data;
            waitForAttackRefresh = new WaitForSeconds(1 / rate);
            StartAttacking();
        }

        public void StartAttacking()
        {
            if (runningCoroutine != null)
            {
                Debug.LogError($"Attack Coroutine already on.");
                return;
            }

            runningCoroutine = StartCoroutine(PerformCoroutine());
        }

        public void StopAttacking()
        {
            if (runningCoroutine == null)
            {
                return;
            }

            StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }


        public abstract IEnumerator PerformCoroutine();
        public virtual void PerformOneShotAttack() { }
    }
}