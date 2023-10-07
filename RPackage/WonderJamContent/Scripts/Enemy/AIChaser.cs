// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

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
        public LayerMask targetLayer;
        public int damage = 10;
        public bool isOut = true;

        // Start is called before the first frame update
        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
        }

        // Update is called once per frame
        private void Update()
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            if (isDistanceEnemy)
            {
                if (distance > 5f)
                {
                    Vector2 direction = player.transform.position - transform.position;
                    transform.position =
                        Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                }
            }
            else
            {
                Vector2 direction = player.transform.position - transform.position;
                transform.position =
                    Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Trigger");
            isOut = false;
            if (other.CompareTag("Player"))
            {
                other.GetComponent<HealthManager>().Damage(damage);
                StartCoroutine(StayInRange());
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Exit");
            isOut = true;
            if (other.CompareTag("Player"))
            {
                StopCoroutine(StayInRange());
            }
        }

        private IEnumerator StayInRange()
        {
            yield return new WaitForSeconds(1f);
            if (!isOut)
            {
                OnTriggerEnter2D(player.GetComponent<Collider2D>());
            }
        }
    }
}