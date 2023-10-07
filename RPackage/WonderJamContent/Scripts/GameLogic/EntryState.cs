// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using System;

namespace Rezoskour.Content
{
    public class EntryState : GameState
    {
        public EntryState(Action? _onEnterCallback = null, Action? _onExitCallback = null) :
            base(_onEnterCallback, _onExitCallback) { }

        public override void Enter() { }

        public override void Process() { }

        public override void Exit() { }
    }
}