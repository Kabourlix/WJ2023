// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

namespace Rezoskour.Content
{
    public class PlayerMovement : MonoBehaviour
    {
        public Sprite berzerkSprite = null;
        public Sprite normalSprite = null;
        private CustomInput input = null;
        private Vector2 moveVector = Vector2.zero;
        private Rigidbody2D rb = null;
        private float moveSpeed = 10f;
        private bool isBerzerk = false;
        public GameManager gameManager;

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
            input.Player.Berzerk.performed += OnBerzerkPerformed;
        }


        private void OnDisable()
        {
            input.Disable();
            input.Player.Movement.performed -= OnMovementPerformed;
            input.Player.Movement.canceled -= OnMovementCanceled;
            input.Player.Berzerk.performed -= OnBerzerkPerformed;
        }

        private void FixedUpdate()
        {
            rb.velocity = moveVector * moveSpeed;
        }

        private void OnMovementPerformed(InputAction.CallbackContext ctx)
        {
            moveVector = ctx.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext ctx)
        {
            moveVector = Vector2.zero;
        }

        private void OnBerzerkPerformed(InputAction.CallbackContext _obj)
        {
            if (isBerzerk)
            {
                gameManager.SetBerserk(false);
                isBerzerk = false;
                transform.GetComponent<SpriteRenderer>().sprite = normalSprite;
            }
            else if (!isBerzerk)
            {
                gameManager.SetBerserk(true);
                isBerzerk = true;
                transform.GetComponent<SpriteRenderer>().sprite = berzerkSprite;
            }
        }
    }
}