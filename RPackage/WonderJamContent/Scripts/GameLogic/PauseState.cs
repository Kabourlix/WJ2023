// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;
using UnityEngine;

namespace Rezoskour.Content
{
    public class PauseState : GameState
    {
        public PauseState(Action? _onEnterCallback = null, Action? _onExitCallback = null) :
            base(_onEnterCallback, _onExitCallback) { }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
        }

        public override void Process() { }

        public override void Exit()
        {
            base.Exit();
            Time.timeScale = 1f;
        }
    }
}