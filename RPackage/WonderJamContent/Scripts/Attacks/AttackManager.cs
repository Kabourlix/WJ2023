// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 06

#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rezoskour.Content
{
    public class AttackManager : MonoBehaviour
    {
        [SerializeField] private AttackName startAttack;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private RAttack[] allAttacks = null!;
        [SerializeField] private bool isPlayer = true;
        private readonly Dictionary<AttackName, RAttack> allPrefabAttacksDict = new();
        private readonly Dictionary<AttackName, RAttack> currentAttacks = new();

        private PlayerStats playerStats;

        private Transform tf = null!;

        private void Awake()
        {
            tf = transform;
            playerStats = GetComponent<PlayerStats>();
            InitializeAllAttacks();
        }

        private void Start()
        {
            TryAddAttack(startAttack, isPlayer);
        }

        public void ResumeAttacking()
        {
            foreach (KeyValuePair<AttackName, RAttack> kv in currentAttacks)
            {
                kv.Value.StartAttacking();
            }
        }

        public void PauseAttacking()
        {
            foreach (KeyValuePair<AttackName, RAttack> kv in currentAttacks)
            {
                kv.Value.StopAttacking();
            }
        }

        public void UpdateAttackStats()
        {
            foreach (KeyValuePair<AttackName, RAttack> kv in currentAttacks)
            {
                kv.Value.UpdateStats(playerStats.CurrentStats);
            }
        }

        public bool TryAddAttack(AttackName _name, bool _startAttackCoroutine = true)
        {
            if (!allPrefabAttacksDict.ContainsKey(_name))
            {
                Debug.LogError($"Attack {_name} not found.");
                return false;
            }

            if (currentAttacks.ContainsKey(_name))
            {
                Debug.LogError($"Attack {_name} already possessed by {name}.");
                return false;
            }

            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null !");
                return false;
            }


            RAttack newAttack = Instantiate(allPrefabAttacksDict[_name], transform);
            currentAttacks.Add(_name, newAttack);

            Func<Vector2> getLookDirectionFunc = isPlayer
                ? () => GameManager.Instance!.PlayerLookDirection.normalized
                : () => (GameManager.Instance!.PlayerTf.position - tf.position).normalized;

            newAttack.Initialize(transform, layerMask, getLookDirectionFunc, _startAttackCoroutine);
            return true;
        }

        public bool TryRemoveAttack(AttackName _name)
        {
            if (!currentAttacks.ContainsKey(_name))
            {
                Debug.LogError($"Attack {_name} is not possessed by {name}.");
                return false;
            }

            currentAttacks.Remove(_name, out RAttack toRemove);
            toRemove.StopAttacking();
            Destroy(toRemove.gameObject);
            return true;
        }

        private void InitializeAllAttacks()
        {
            foreach (RAttack attack in allAttacks)
            {
                AttackName aName = attack.Name;
                allPrefabAttacksDict.Add(aName, attack);
            }

            //Debug.Log($"{allPrefabAttacksDict.Count} attacks has been loaded.");
        }
    }
}