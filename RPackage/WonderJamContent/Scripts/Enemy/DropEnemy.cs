  using Rezoskour.Content;
  using Rezoskour.Content.Collectable;
  using UnityEngine;

  namespace Rezoskour.Content
    {
        public class DropEnemy : ChasingEnemy
        {
            protected override void PerformAttack()
            {
            
                if(triggerAttackArray[0])
                {
                    animator.SetBool("isAttacking", true);
                    if (player.TryGetComponent(out IHealth health))
                    {
                        health.Damage(damage);
                    }
                }
                else
                {
                    animator.SetBool("isAttacking", false);
                }
            }

            public override void Damage(int _amount)
            {
                maxHealth -= _amount;
                if (maxHealth <= 0)
                {
                    chasingJobHandle.Complete();
                    triggerAttackArray[0] = false;
                    isDying = true;
                    if (CollectableManager.Instance == null)
                    {
                        Debug.LogError("CollectableManager.Instance is null !");
                        return;
                    }
                    Transform? transform1 = transform;
                    CollectableManager.Instance.SpawnWeapon(transform1.position, transform1.rotation);
                    
                    releaseCallback?.Invoke();
                }
            }
        }
    }
    