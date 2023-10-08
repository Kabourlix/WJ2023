// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 08

#nullable enable

using System;
using System.Collections;
using UnityEngine;

// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 08

namespace Rezoskour.Content
{
    public class TourniquetAttack : RAttack
    {
        [SerializeField] private GameObject[] tourniquetProjectiles = null!;
        [SerializeField] private float duration = 3;

        private WaitForSeconds waitDuration = null!;

        private void Start()
        {
            waitDuration = new WaitForSeconds(duration);
        }

        public override IEnumerator PerformCoroutine()
        {
            if (UserTransform == null)
            {
                Debug.LogError("Player transform is null.");
                yield break;
            }

            if (Manager == null)
            {
                Debug.LogError("GAMEMANAGER IS NULL!");
                yield break;
            }

            while (true)
            {
                foreach (GameObject tourn in tourniquetProjectiles)
                {
                    tourn.SetActive(true);
                }

                yield return waitDuration;

                foreach (GameObject tourn in tourniquetProjectiles)
                {
                    tourn.SetActive(false);
                }

                yield return waitForAttackRefresh;
            }
        }
    }
}