// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;
using UnityEngine;

namespace Rezoskour.Content
{
    public class DefeatState : GameState
    {
        public DefeatState(Action? _onEnterCallback = null, Action? _onExitCallback = null) :
            base(_onEnterCallback, _onExitCallback) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Game is lost");
            Time.timeScale = 0;
        }

        public override void Process() { }

        public override void Exit()
        {
            Time.timeScale = 1;
            base.Exit();
        }
    }
}