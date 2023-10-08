// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;
using UnityEngine;

namespace Rezoskour.Content
{
    public class LevelUpState : GameState
    {
        public LevelUpState(Action? _onEnterCallback = null, Action? _onExitCallback = null) :
            base(_onEnterCallback, _onExitCallback) { }

        public override void Enter()
        {
            base.Enter();
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