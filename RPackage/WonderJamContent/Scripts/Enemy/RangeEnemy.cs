// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 08

using System;
using Rezoskour.Content.Misc;
using UnityEngine;

namespace Rezoskour.Content
{
    public class RangeEnemy : ChasingEnemy
    {
        [SerializeField] private LayerMask layerMask;
        private RAttack attack = null!;

        private const string ATTACK_TIMER_CD = "AttackTimer";
        private string timerName;

        protected sealed override void Start()
        {
            base.Start();
            attack = transform.GetComponentInChildren<RAttack>();
            attack.Initialize(transform, layerMask, () => (targetTransform.position - transform.position).normalized,
                false, null);

            if (CoolDownSystem.Instance == null)
            {
                Debug.LogError("CoolDownSystem.Instance is null !");
                return;
            }

            timerName = ATTACK_TIMER_CD + Guid.NewGuid();
            CoolDownSystem.Instance.TryRegisterCoolDown(timerName, attack.AttackSpeed, false);
        }

        protected override void PerformAttack()
        {
            if (CoolDownSystem.Instance == null)
            {
                Debug.LogError("CoolDownSystem.Instance is null !");
                return;
            }

            if (!CoolDownSystem.Instance.IsCoolDownDone(timerName))
            {
                return;
            }

            animator.SetBool("isAttacking", true);
            attack.PerformOneShotAttack();
            CoolDownSystem.Instance.StartCoolDown(timerName, true);
            //attackManager.ResumeAttacking();
            // projectilePrefab.Init(() => animator.SetBool("isAttacking", false));
        }

        protected override void StopPerform()
        {
            animator.SetBool("isAttacking", false);
        }
    }
}