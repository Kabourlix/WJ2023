// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;
using UnityEngine;

namespace Rezoskour.Content
{
    public class BerserkState : GameState
    {
        public BerserkState(Action? _onEnterCallback, Action? _onExitCallback) :
            base(_onEnterCallback, _onExitCallback) { }

        public override void Enter()
        {
            if (Manager == null)
            {
                Debug.LogError("CRITICAL !!! GameManager is null.");
                return;
            }

            Debug.Log("Enter BerserkState");
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }

        public override void Exit()
        {
            if (Manager == null)
            {
                Debug.LogError("CRITICAL !!! GameManager is null.");
                return;
            }
        }
    }
}