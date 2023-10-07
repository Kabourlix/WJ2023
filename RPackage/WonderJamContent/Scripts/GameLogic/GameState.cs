// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

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
        protected GameManager? Manager => GameManager.Instance;

        public abstract void Enter();

        public abstract void Process();

        public abstract void Exit();
    }
}