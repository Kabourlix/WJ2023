using Rezoskour.Content.Collectable;
using Rezoskour.Content.Misc;

namespace Rezoskour.Content
{
    public class BombEnemy : ChasingEnemy
    {
        // ReSharper disable Unity.PerformanceAnalysis
        protected override void PerformAttack()
        {
            animator.SetBool("isAttacking", true);
            if (player.TryGetComponent(out IHealth health))
            {
                health.Damage(damage);
            }
            Damage(100);
        }
        
    }
}