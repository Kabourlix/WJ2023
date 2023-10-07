// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 05

#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rezoskour.Content
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager? Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"{nameof(GameManager)} cannot be instanced more than once.");
                Destroy(gameObject);
            }

            Instance = this;
        }

        #endregion

        //ça marche pas chez Corentin c'est de LA MERDE !
        public event Action<bool>? OnBerserkModeChange;

        [SerializeField] private Transform playerTransform = null!;
        public Transform PlayerTf => playerTransform;

        private GameStateName currentState = GameStateName.Entry;

        private readonly Dictionary<GameStateName, GameState> allGameStates = new()
        {
            {GameStateName.Entry, new EntryState()},
            {GameStateName.Main, new MainState()},
            {GameStateName.Berserk, new BerserkState()},
            {GameStateName.Pause, new PauseState()},
            {GameStateName.LevelUp, new LevelUpState()},
            {GameStateName.GameEnd, new GameEndState()},
            {GameStateName.Defeat, new DefeatState()},
            {GameStateName.Victory, new VictoryState()}
        };

        private bool[,] isStateSwitchPossible =
        {
            {false, true, false, false, false, false, false, false}, //Entry
            {false, false, true, true, true, true, false, false}, //Main
            {false, true, false, true, false, true, false, false}, //Berserk
            {false, true, true, false, false, false, false, false}, //Pause
            {false, true, false, false, false, false, false, false}, //LevelUp
            {false, false, false, false, false, false, true, true}, //GameEnd
            {true, false, false, false, false, false, false, false}, //Defeat
            {true, false, false, false, false, false, false, false} //Victory
        };

        public void ChangeState(GameStateName _stateName)
        {
            if (!isStateSwitchPossible[(int) currentState, (int) _stateName])
            {
                Debug.Log($"Switch from {currentState} to {_stateName} is impossible.");
                return;
            }

            allGameStates[currentState].Exit();

            switch (_stateName)
            {
                case GameStateName.Entry:
                    break;
                case GameStateName.Main:
                    break;
                case GameStateName.Berserk:
                    break;
                case GameStateName.Pause:
                    break;
                case GameStateName.LevelUp:
                    break;
                case GameStateName.GameEnd:
                    break;
                case GameStateName.Defeat:
                    break;
                case GameStateName.Victory:
                    break;
                default:
                    Debug.LogError($"Unknown state {_stateName}.");
                    return;
            }

            currentState = _stateName;
            allGameStates[currentState].Enter();
        }

        public void SetBerserk(bool _b)
        {
            OnBerserkModeChange?.Invoke(_b);
        }
    }
}