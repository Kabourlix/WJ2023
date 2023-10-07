// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;
using UnityEngine;

namespace Rezoskour.Content
{
    public class BerserkState : GameState
    {
        public BerserkState(Action? _onEnterCallback = null, Action? _onExitCallback = null) :
            base(_onEnterCallback, _onExitCallback) { }

        public override void Enter()
        {
            base.Enter();
            if (Manager == null)
            {
                Debug.LogError("CRITICAL !!! GameManager is null.");
                return;
            }

            Debug.Log("Enter BerserkState");
        }

        public override void Process() { }

        public override void Exit()
        {
            if (Manager == null)
            {
                Debug.LogError("CRITICAL !!! GameManager is null.");
                return;
            }

            base.Exit();
        }
    }
}