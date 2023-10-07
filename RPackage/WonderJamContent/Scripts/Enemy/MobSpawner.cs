//
// using System.Collections;
//
// using UnityEngine;
// using UnityEngine.Pool;
//
// namespace Rezoskour.Content
// {
//     public class MobSpawner : MonoBehaviour
//     {
//         [SerializeField] private ChasingEnemy enemyPrefab = null!;
//         private ObjectPool<ChasingEnemy> enemyPool = null!;
//
//         // private void Awake()
//         // {
//         //     enemyPool = new ObjectPool<ChasingEnemy>(OnCreateEnemy, OnGetEnemy, OnReleaseEnemy, null,
//         //         true, 5, 40);
//         // }
//
//         public IEnumerator PerformCoroutine()
//         {
//             
//
//             while (true)
//             {
//                 ChasingEnemy enemy = enemyPool.Get();
//                 enemy.Spawn();
//                 yield return new WaitForSeconds(0.5f);
//             }
//             
//         }
//
//
//         private ChasingEnemy OnCreateEnemy()
//         {
//             ChasingEnemy enemy = Instantiate(enemyPrefab, transform).GetComponent<ChasingEnemy>();
//             enemy.name = $"Enemy {enemyPool.CountAll + 1}";
//             enemy.Init(() => enemyPool.Release(enemy));
//
//             enemy.health = enemyPrefab.health;
//
//             return enemy;
//         }
//
//         // private void OnGetEnemy(ChasingEnemy _enemy)
//         // {
//         //     if (PlayerTransform == null)
//         //     {
//         //         return;
//         //     }
//         //
//         //     _enemy.gameObject.SetActive(true);
//         //     _enemy.transform.SetPositionAndRotation(PlayerTransform.position + 0.2f * PlayerTransform.forward,
//         //         Quaternion.identity);
//         // }
//         //
//         // private void OnReleaseEnemy(ChasingEnemy _enemy)
//         // {
//         //     _enemy.gameObject.SetActive(false);
//         // }
//         
//         
//        
//     }
//     
// }