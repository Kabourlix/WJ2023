// Copyrighted by team Rézoskour
// Created by corentin vernel on 06

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

        // Start is called before the first frame update
        private void Start()
        {
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
    }
}