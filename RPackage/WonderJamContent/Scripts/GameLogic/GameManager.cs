// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

#nullable enable

using System;
using System.Collections.Generic;
using Rezoskour.Content.Misc;
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

            playerAttack = PlayerTf.GetComponent<AttackManager>();
        }

        #endregion

        public const string GAME_TIMER = "GameTimer";
        public const string BERSERK_TIMER = "BerserkTimer";

        public event Action<bool>? OnBerserkModeChange;
        public event Action? OnDefeat;
        public event Action? OnVictory;

        public event Action? OnLevelUp;


        [SerializeField] private int durationToSurviveInMinutes = 5;
        [SerializeField] private int berserkCdInSeconds = 3;

        public int DurationToSurviveInSeconds => durationToSurviveInMinutes * 60;
        [SerializeField] private Transform playerTransform = null!;
        public Transform PlayerTf => playerTransform;
        private AttackManager playerAttack = null!;
        public Vector3 PlayerLookDirection { get; set; } = Vector3.right;


        private GameStateName currentState = GameStateName.Entry;
        public GameStateName CurrentState => currentState;

        private readonly Dictionary<GameStateName, GameState> allGameStates = new()
        {
            { GameStateName.Entry, new EntryState() },
            { GameStateName.Main, new MainState() },
            {
                GameStateName.Berserk,
                new BerserkState(() => Instance!.SetBerserk(true), () => Instance!.SetBerserk(false))
            },
            { GameStateName.Pause, new PauseState() },
            { GameStateName.LevelUp, new LevelUpState(() => Instance!.OnLevelUp?.Invoke()) },
            { GameStateName.Defeat, new DefeatState(() => Instance!.OnDefeat?.Invoke()) },
            { GameStateName.Victory, new VictoryState(() => Instance!.OnVictory?.Invoke()) }
        };

        private bool[,] isStateSwitchPossible =
        {
            { false, true, false, false, false, false, false }, //Entry
            { false, false, true, true, true, true, true }, //Main
            { false, true, false, true, false, true, true }, //Berserk
            { false, true, true, false, false, false, false }, //Pause
            { false, true, false, false, false, false, false }, //LevelUp
            { true, false, false, false, false, false, false }, //Defeat
            { true, false, false, false, false, false, false } //Victory
        };

        private void Start()
        {
            //TODO : REMOVE THIS
            if (CoolDownSystem.Instance == null)
            {
                Debug.LogError("CoolDownSystem.Instance is null !");
                return;
            }

            CoolDownSystem.Instance.OnCoolDownDone += CoolDownDoneHandler;
            //Init game timer
            CoolDownSystem.Instance.TryRegisterCoolDown(GAME_TIMER, DurationToSurviveInSeconds, true);
            CoolDownSystem.Instance.TryRegisterCoolDown(BERSERK_TIMER, berserkCdInSeconds);
            CoolDownSystem.Instance.StartCoolDown(GAME_TIMER, false);

            ChangeState(GameStateName.Main);
        }

        private void CoolDownDoneHandler(string _cdName)
        {
            if (!_cdName.Equals(GAME_TIMER))
            {
                return;
            }

            ChangeState(GameStateName.Victory);
        }

        private void Update()
        {
            allGameStates[currentState].Process();
        }


        public bool ChangeState(GameStateName _stateName)
        {
            if (!isStateSwitchPossible[(int)currentState, (int)_stateName])
            {
                Debug.Log($"Switch from {currentState} to {_stateName} is impossible.");
                return false;
            }

            if (CoolDownSystem.Instance == null)
            {
                Debug.LogError("CoolDownSystem.Instance is null !");
                return false;
            }

            if (_stateName == GameStateName.Berserk && !CoolDownSystem.Instance.IsCoolDownDone(BERSERK_TIMER))
            {
                //TODO : Add feedback
                return false; //Berserk in cooldown
            }

            allGameStates[currentState].Exit();
            currentState = _stateName;
            allGameStates[currentState].Enter();
            return true;
        }

        #region ChangeStates Callbacks

        private void SetBerserk(bool _b)
        {
            if (CoolDownSystem.Instance == null)
            {
                Debug.LogError("CoolDownSystem.Instance. is null !");
                return;
            }

            OnBerserkModeChange?.Invoke(_b);
            if (_b)
            {
                playerAttack.TryAddAttack(AttackName.BerserkBurn);
            }
            else
            {
                playerAttack.TryRemoveAttack(AttackName.BerserkBurn);
                CoolDownSystem.Instance.StartCoolDown(BERSERK_TIMER);
            }
        }

        #endregion
    }
}