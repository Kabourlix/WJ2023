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
        public Sprite berserkSprite = null;
        public Animator animator;
        public Sprite normalSprite = null;
        private CustomInput input = null;
        private Vector2 moveVector = Vector2.zero;
        private Rigidbody2D rb = null;
        private float moveSpeed = 10f;
        private bool isBerserk = false;
        private bool facingRight = true;

        private void Awake()
        {
            input = new CustomInput();
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            input.Enable();
            input.Player.Movement.performed += OnMovementPerformed;
            input.Player.Movement.canceled += OnMovementCanceled;
            input.Player.Berserk.performed += OnBerzerkPerformed;
        }


        private void OnDisable()
        {
            input.Disable();
            input.Player.Movement.performed -= OnMovementPerformed;
            input.Player.Movement.canceled -= OnMovementCanceled;
            input.Player.Berserk.performed -= OnBerzerkPerformed;
        }

        private void FixedUpdate()
        {
            rb.velocity = moveVector * moveSpeed;
        }

        private void Update()
        {
            animator.SetFloat("Speed", MathF.Abs(moveVector.x));
        }

        private void OnMovementPerformed(InputAction.CallbackContext ctx)
        {
            moveVector = ctx.ReadValue<Vector2>();
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

        private void OnMovementCanceled(InputAction.CallbackContext ctx)
        {
            moveVector = Vector2.zero;
        }

        private void OnBerzerkPerformed(InputAction.CallbackContext _obj)
        {
            if (isBerserk)
            {
                GameManager.Instance.ChangeState(GameStateName.Main);
                animator.SetBool("IsBerserk", false);
                isBerserk = false;
                transform.GetComponent<SpriteRenderer>().sprite = normalSprite;
            }
            else if (!isBerserk)
            {
                GameManager.Instance.ChangeState(GameStateName.Berserk);
                animator.SetBool("IsBerserk", true);
                isBerserk = true;
                transform.GetComponent<SpriteRenderer>().sprite = berserkSprite;
            }
        }
    }
}