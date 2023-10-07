// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

#nullable enable

using System;
using UnityEngine;

// Copyrighted by team Rézoskour
// Created by alexandre buzon on 06

namespace Rezoskour.Content
{
    public class Xp : MonoBehaviour
    {
        private int level;
        private int xp;
        private bool isBerserk;
        [SerializeField] private AnimationCurve xpCurve;

        private void Start()
        {
            level = 1;
            xp = 0;
            isBerserk = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnBerserkModeChange += SwitchMode;
            }
        }

        private void SwitchMode(bool _obj)
        {
            isBerserk = _obj;
        }

        public bool AddXp(int _xp)
        {
            xp += xp;
            if (xp >= xpCurve.Evaluate(level))
            {
                if (isBerserk)
                {
                    return false;
                }

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