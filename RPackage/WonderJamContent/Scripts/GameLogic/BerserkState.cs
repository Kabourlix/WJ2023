// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable
using UnityEngine;

namespace Rezoskour.Content
{
    public class BerserkState : GameState
    {
        public override void Enter()
        {
            if (Manager == null)
            {
                Debug.LogError("CRITICAL !!! GameManager is null.");
                return;
            }

            Manager.SetBerserk(true);
        }

        public override void Process()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            if (Manager == null)
            {
                Debug.LogError("CRITICAL !!! GameManager is null.");
                return;
            }

            Manager.SetBerserk(false);
        }
    }
}