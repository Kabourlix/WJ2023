// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;
using Rezoskour.Content.Misc;
using UnityEngine;

namespace Rezoskour.Content
{
    public class MainState : GameState
    {
        public MainState(Action? _onEnterCallback = null, Action? _onExitCallback = null) :
            base(_onEnterCallback, _onExitCallback) { }

        public override void Enter()
        {
            base.Enter();
            Time.timeScale = 1;
            Debug.Log("Enter MainState");
        }

        public override void Process() { }

        public override void Exit()
        {
            Debug.Log("Exit MainState");
        }
    }
}