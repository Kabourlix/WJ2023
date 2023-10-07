// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

namespace Rezoskour.Content
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = null!;

        public Sprite berserkSprite = null;
        public Animator animator;
        public Sprite normalSprite = null;
        private CustomInput input = null;
        private Vector2 moveVector = Vector2.zero;
        private Rigidbody2D rb = null;
        private float moveSpeed = 10f;
        private bool isBerserk = false;
        private bool facingRight = true;

        private GameManager? gameManager => GameManager.Instance;

        private void Awake()
        {
            input = new CustomInput();
            rb = GetComponent<Rigidbody2D>();
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
            rb.velocity = moveVector * moveSpeed;
        }

        private void Update()
        {
            animator.SetFloat("Speed", MathF.Abs(moveVector.x));
        }

        private void OnMovementPerformed(InputAction.CallbackContext _ctx)
        {
            if (gameManager == null)
            {
                Debug.LogError("GAME MANAGER IS NULL");
                return;
            }

            moveVector = _ctx.ReadValue<Vector2>();
            gameManager.PlayerLookDirection = moveVector.normalized;
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
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            facingRight = !facingRight;
            transform.localScale = theScale;
        }

        private void OnMovementCanceled(InputAction.CallbackContext _ctx)
        {
            moveVector = Vector2.zero;
        }

        private void OnBerzerkPerformed(InputAction.CallbackContext _obj)
        {
            if (gameManager == null)
            {
                Debug.LogError("GAME MANAGER IS NULL");
                return;
            }

            if (isBerserk)
            {
                gameManager.ChangeState(GameStateName.Main);
                animator.SetBool("IsBerserk", false);
                isBerserk = false;
                spriteRenderer.sprite = normalSprite;
            }
            else if (!isBerserk)
            {
                gameManager.ChangeState(GameStateName.Berserk);
                animator.SetBool("IsBerserk", true);
                isBerserk = true;
                spriteRenderer.sprite = berserkSprite;
            }
        }

        private GameStateName stateBeforePause;

        private void OnStartPause(InputAction.CallbackContext _obj)
        {
            if (gameManager == null)
            {
                Debug.LogError("GAME MANAGER IS NULL");
                return;
            }

            input.Player.Disable();
            input.PauseCtx.Enable();
            stateBeforePause = gameManager.CurrentState;
            gameManager.ChangeState(GameStateName.Pause);
        }

        private void OnStopPause(InputAction.CallbackContext _obj)
        {
            if (gameManager == null)
            {
                Debug.LogError("GAME MANAGER IS NULL");
                return;
            }

            input.Player.Enable();
            input.PauseCtx.Disable();
            gameManager.ChangeState(stateBeforePause);
        }
    }
}