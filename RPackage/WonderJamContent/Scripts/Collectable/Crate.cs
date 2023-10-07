// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using UnityEngine;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 07

namespace Rezoskour.Content.Collectable
{
    public class Crate : MonoBehaviour, IHealth
    {
        [Range(0, 1)] [SerializeField] private float oilSpawnProbability;
        [Range(0, 1)] [SerializeField] private float expSpawnProbability;

        [SerializeField] private int hp = 100;

        [ContextMenu("Destroy")]
        private void DestoryCrate()
        {
            if (CollectableManager.Instance == null)
            {
                Debug.LogError("CollectableManager.Instance is null !");
                return;
            }

            Debug.Log("Crate destroyed");
            float range = oilSpawnProbability + expSpawnProbability;
            float rand = Random.Range(0, range);
            if (rand <= oilSpawnProbability)
            {
                Transform transform1 = transform;
                CollectableManager.Instance.SpawnOil(transform1.position, transform1.rotation);
            }
            else
            {
                Transform transform1 = transform;
                CollectableManager.Instance.SpawnXp(transform1.position, transform1.rotation);
            }

            Destroy(gameObject);
        }

        public void Heal(int _amount) { }

        public void Damage(int _amount)
        {
            hp -= _amount;
            if (hp <= 0)
            {
                DestoryCrate();
            }
        }
    }
}