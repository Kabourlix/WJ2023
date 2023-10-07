// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

#nullable enable

using UnityEngine;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

namespace Rezoskour.Content.Collectable
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] private GameObject? oil;
        [Range(0, 1)] [SerializeField] private float oilSpawnProbability;
        [SerializeField] private GameObject? experience;
        [Range(0, 1)] [SerializeField] private float expSpawnProbability;

        [ContextMenu("Destroy")]
        private void Destory()
        {
            Debug.Log("Crate destroyed");
            var range = oilSpawnProbability + expSpawnProbability;
            var rand = Random.Range(0, range);
            if (rand <= oilSpawnProbability)
            {
                var transform1 = transform;
                CollectableManager.Instance.SpawnOil(transform1.position, transform1.rotation);
            }
            else
            {
                var transform1 = transform;
                CollectableManager.Instance.SpawnXp(transform1.position, transform1.rotation);
            }

            Destroy(gameObject);
        }
    }
}