// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rezoskour.Content
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = null!;
        [SerializeField] private PlayerStats playerStats = null!;

        public Sprite berserkSprite = null!;
        public Animator animator = null!;
        public Sprite normalSprite = null!;
        private CustomInput input = null!;
        private Vector2 moveVector = Vector2.zero;
        private Rigidbody2D rb = null!;
        private bool facingRight = true;

        private GameManager? GameManager => GameManager.Instance;

        private void Awake()
        {
            input = new CustomInput();
            rb = GetComponent<Rigidbody2D>();
            playerStats = GetComponent<PlayerStats>();
        }

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null !");
                return;
            }

            GameManager.Instance.OnVictory += OnGameEnds;
            GameManager.Instance.OnDefeat += OnGameEnds;
            GameManager.Instance.OnBerserkModeChange += BerserkStatusChangedHandler;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError($"GameManager.Instance is null OnDestroy:{nameof(PlayerMovement)}!");
                return;
            }

            GameManager.Instance.OnVictory -= OnGameEnds;
            GameManager.Instance.OnDefeat -= OnGameEnds;
        }


        private void OnEnable()
        {
            input.Enable();
            input.PauseCtx.Disable();
            input.Player.Movement.performed += OnMovementPerformed;
            input.Player.Movement.canceled += OnMovementCanceled;
            input.Player.Berserk.performed += OnBerzerkPerformed;

            input.Player.Pause.performed += OnStartPause;
            input.PauseCtx.Pause.performed += OnStopPause;
        }


        private void OnDisable()
        {
            input.Disable();
            input.Player.Movement.performed -= OnMovementPerformed;
            input.Player.Movement.canceled -= OnMovementCanceled;
            input.Player.Berserk.performed -= OnBerzerkPerformed;

            input.Player.Pause.performed -= OnStartPause;
            input.PauseCtx.Pause.performed -= OnStopPause;
        }

        private void FixedUpdate()
        {
            rb.velocity = playerStats.CurrentStats.speed * moveVector;
        }

        private void Update()
        {
            animator.SetFloat("ordonne", MathF.Abs(moveVector.y));
            animator.SetFloat("Speed", MathF.Abs(moveVector.x));
        }

        private void OnMovementPerformed(InputAction.CallbackContext _ctx)
        {
            if (GameManager == null)
            {
                Debug.LogError("GAME MANAGER IS NULL");
                return;
            }

            moveVector = _ctx.ReadValue<Vector2>();
            GameManager.PlayerLookDirection = moveVector.normalized;
            if (moveVector.y > 0 && moveVector.x == 0)
            {
                animator.SetBool("isRight", false);
                animator.SetBool("isForward", true);
                animator.SetBool("isDown", false);
            }
            else if (moveVector.y < 0 && moveVector.x == 0)
            {
                animator.SetBool("isRight", false);
                animator.SetBool("isForward", false);
                animator.SetBool("isDown", true);
            }
            else if (moveVector.x > 0)
            {
                animator.SetBool("isRight", true);
                animator.SetBool("isForward", false);
                animator.SetBool("isDown", false);
                
            }
            else if (moveVector.x < 0)
            {
                animator.SetBool("isRight", true);
                animator.SetBool("isForward", false);
                animator.SetBool("isDown", false);
                
            }
            else
            {
                animator.SetBool("isRight", false);
                animator.SetBool("isForward", false);
                animator.SetBool("isDown", false);
            }
            
            if (moveVector.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveVector.x < 0 && facingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            Transform spriteTransform = spriteRenderer.transform;
            Vector3 theScale = spriteTransform.localScale;
            theScale.x *= -1;
            facingRight = !facingRight;
            spriteTransform.localScale = theScale;
        }

        private void OnMovementCanceled(InputAction.CallbackContext _ctx)
        {
            moveVector = Vector2.zero;
        }

        private void OnBerzerkPerformed(InputAction.CallbackContext _obj)
        {
            if (GameManager == null)
            {
                Debug.LogError("GAME MANAGER IS NULL");
                return;
            }

            GameManager.ChangeState(GameManager.CurrentState == GameStateName.Berserk
                ? GameStateName.Main
                : GameStateName.Berserk);
        }

        private void BerserkStatusChangedHandler(bool _isBerserkOn)
        {
            if (_isBerserkOn)
            {
                animator.SetBool("IsBerserk", true);
                spriteRenderer.sprite = berserkSprite;
            }
            else
            {
                animator.SetBool("IsBerserk", false);
                spriteRenderer.sprite = normalSprite;
            }
        }

        private GameStateName stateBeforePause;

        private void OnStartPause(InputAction.CallbackContext _obj)
        {
            if (GameManager == null)
            {
                Debug.LogError("GAME MANAGER IS NULL");
                return;
            }

            input.Player.Disable();
            input.PauseCtx.Enable();
            stateBeforePause = GameManager.CurrentState;
            GameManager.ChangeState(GameStateName.Pause);
        }

        private void OnStopPause(InputAction.CallbackContext _obj)
        {
            if (GameManager == null)
            {
                Debug.LogError("GAME MANAGER IS NULL");
                return;
            }

            input.Player.Enable();
            input.PauseCtx.Disable();
            GameManager.ChangeState(stateBeforePause);
        }

        private void OnGameEnds()
        {
            input.Disable();
        }
    }
}