// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

#nullable enable

using UnityEngine;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

namespace Rezoskour.Content
{
    public class Xp : MonoBehaviour
    {
        private int level;
        private int xp;
        [SerializeField] private AnimationCurve xpCurve;

        public bool AddXp(int _xp)
        {
            xp += xp;
            if (xp >= xpCurve.Evaluate(level))
            {
                level++;
                //event
                return true;
            }

            return false;
        }

        public int GetLevel()
        {
            return level;
        }
    }
}