using Rezoskour.Content.Misc;
using UnityEngine;

namespace Rezoskour.Content
{
    public class RangeEnemy : ChasingEnemy
    {
        [SerializeField] private Projectile projectilePrefab = null!;

        protected override void PerformAttack()
        {
            if (CoolDownSystem.Instance != null)
            {
                CoolDownSystem.Instance.TryRegisterCoolDown("Attack", 0.5f, true);
                CoolDownSystem.Instance.StartTimer("Attack");
            }

            if(triggerAttackArray[0])
            {
                animator.SetBool("isAttacking", true);

                if (CoolDownSystem.Instance != null)
                {
                    projectilePrefab.Init(() => animator.SetBool("isAttacking", false));
                    CoolDownSystem.Instance.TryRegisterCoolDown("Attack", 2f, true);
                    CoolDownSystem.Instance.StartTimer("Attack");
                }
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }
}