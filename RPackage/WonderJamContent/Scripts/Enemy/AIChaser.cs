// Copyrighted by team Rézoskour
// Created by corentin vernel on 07

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rezoskour.Content
{
    public class AIChaser : MonoBehaviour
    {
        public GameObject player;
        public float speed = 1f;
        public bool isDistanceEnemy = false;
        private float distance = 1f;
        public int maxHealth = 20;
        public int currentHealth;
        public int damage = 1;
        private bool isOut = true;
        public Animator animator;

        private bool facingRight = true;

        // Start is called before the first frame update
        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
        }

        // Update is called once per frame
        private void Update()
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = player.transform.position - transform.position;
            if (isDistanceEnemy)
            {
                if (isOut)
                {
                    transform.position =
                        Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    animator.SetFloat("Speed", MathF.Abs(direction.x));
                }
            }
            else
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                animator.SetFloat("Speed", MathF.Abs(direction.x));
            }

            if (direction.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && facingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            var theScale = transform.localScale;
            theScale.x *= -1;
            facingRight = !facingRight;
            transform.localScale = theScale;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Trigger");

            if (other.CompareTag("Player"))
            {
                animator.SetFloat("Speed", 0);
                isOut = false;

                StartCoroutine(StayInRange(other));
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Exit");

            if (other.CompareTag("Player"))
            {
                animator.SetBool("isAttacking", false);
                isOut = true;
                StopCoroutine(StayInRange(other));
            }
        }

        private IEnumerator StayInRange(Collider2D other)
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("isAttacking", true);
            if (!isDistanceEnemy)
            {
                other.GetComponent<HealthManager>().Damage(damage);
            }

            yield return new WaitForSeconds(0.5f);
            animator.SetBool("isAttacking", false);
            yield return new WaitForSeconds(2f);
            if (!isOut)
            {
                OnTriggerEnter2D(player.GetComponent<Collider2D>());
            }
        }
    }
}