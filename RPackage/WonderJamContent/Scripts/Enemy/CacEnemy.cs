using Rezoskour.Content.Misc;

namespace Rezoskour.Content
{
    public class CacEnemy : ChasingEnemy
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
    }
}