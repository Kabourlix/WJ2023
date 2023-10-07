// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using TMPro;
using UnityEngine;

namespace Rezoskour.Content.Misc
{
    public class GameTimerUIHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText = null!;

        private void Start()
        {
            if (CoolDownSystem.Instance == null)
            {
                Debug.LogError("CoolDownSystem.Instance is null !");
                return;
            }

            CoolDownSystem.Instance.OnCoolDownUpdates += UpdateTimer;
        }

        private void UpdateTimer(string _name, float _remainingTime)
        {
            if (_name != GameManager.GAME_TIMER)
            {
                return;
            }

            timerText.text = FormatTime(_remainingTime);
        }

        private string FormatTime(float _time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_time);
            return timeSpan.ToString(@"mm\:ss");
        }
    }
}