// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;

// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

namespace Rezoskour.Content
{
    public enum GameStateName
    {
        Entry = 0,
        Main = 1,
        Berserk = 2,
        Pause = 3,
        LevelUp = 4,
        GameEnd = 5,
        Defeat = 6,
        Victory = 7
    }

    public abstract class GameState
    {
        private Action? onEnterCallback;
        private Action? onExitCallback;

        protected GameState(Action? _onEnterCallback = null, Action? _onExitCallback = null)
        {
            onEnterCallback = _onEnterCallback;
            onExitCallback = _onExitCallback;
        }

        protected GameManager? Manager => GameManager.Instance;

        public virtual void Enter()
        {
            onEnterCallback?.Invoke();
        }

        public abstract void Process();

        public virtual void Exit()
        {
            onExitCallback?.Invoke();
        }
    }
}