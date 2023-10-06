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
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private RAttack[] allAttacks = null!;
        private readonly Dictionary<AttackName, RAttack> allPrefabAttacksDict = new();
        private readonly Dictionary<AttackName, RAttack> currentAttacks = new();

        private Transform tf = null!;

        private void Awake()
        {
            tf = transform;
            InitializeAllAttacks();
        }

        private void Start()
        {
            TryAddAttack(AttackName.Basic);
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

            RAttack newAttack = Instantiate(allPrefabAttacksDict[_name], transform);
            currentAttacks.Add(_name, newAttack);
            newAttack.Initialize(transform, layerMask, _startAttackCoroutine);
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
            Destroy(toRemove);
            return true;
        }

        private void InitializeAllAttacks()
        {
            foreach (RAttack attack in allAttacks)
            {
                AttackName aName = attack.Name;
                allPrefabAttacksDict.Add(aName, attack);
            }

            Debug.Log($"{allPrefabAttacksDict.Count} attacks has been loaded.");
        }
    }
}