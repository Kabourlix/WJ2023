// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;
using UnityEngine;

namespace Rezoskour.Content
{
    public class VictoryState : GameState
    {
        public VictoryState(Action? _onEnterCallback = null, Action? _onExitCallback = null) :
            base(_onEnterCallback, _onExitCallback) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter VictoryState");
            Time.timeScale = 0;
        }

        public override void Process() { }

        public override void Exit() { }
    }
}