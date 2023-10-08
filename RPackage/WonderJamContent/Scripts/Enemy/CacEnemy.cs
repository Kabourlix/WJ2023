using Rezoskour.Content.Misc;

namespace Rezoskour.Content
{
    public class CacEnemy : ChasingEnemy
    {
        

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
                if (player.TryGetComponent(out IHealth health))
                {
                    health.Damage(damage);
                }
                if (CoolDownSystem.Instance != null)
                {
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