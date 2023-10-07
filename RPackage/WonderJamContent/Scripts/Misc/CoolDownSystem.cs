// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Rezoskour.Content.Misc
{
    public class TimerSystem : MonoBehaviour
    {
        private Dictionary<string, bool> registeredCoolDowns = new();
        private Dictionary<string, float> registeredCoolDownDurations = new();

        private Dictionary<string, float> registeredCoolDownTimers = new();

        private bool ContainsTimer(string _timerName)
        {
            return registeredCoolDowns.ContainsKey(_timerName) || registeredCoolDownDurations.ContainsKey(_timerName);
        }

        public bool TryAddCoolDown(string _timerName, float _duration)
        {
            if (ContainsTimer(_timerName))
            {
                Debug.LogWarning("[REZOSKOUR] Cannot add the same timer twice.");
                return false;
            }

            registeredCoolDowns.Add(_timerName, true);
            registeredCoolDownDurations.Add(_timerName, _duration);
            return true;
        }

        public bool IsCoolDownDone(string _timerName)
        {
            if (!ContainsTimer(_timerName))
            {
                Debug.LogError($"[REZOSKOUR] {_timerName} is not registered in the cd system.");
                return false;
            }

            return registeredCoolDowns[_timerName];
        }

        public void StartTimer(string _timerName, bool _overrideCurrent = true)
        {
            if (!ContainsTimer(_timerName))
            {
                Debug.LogError($"[REZOSKOUR] {_timerName} is not registered in the cd system.");
                return;
            }

            if (!_overrideCurrent && !registeredCoolDowns[_timerName])
            {
                Debug.LogWarning(
                    $"[REZOSKOUR] {_timerName} is already running. Use StartTimer({_timerName},true) if you want to override the current one.");
                return;
            }

            //We start timer anyway here

            float timeTarget = Time.realtimeSinceStartup + registeredCoolDownDurations[_timerName];

            if (registeredCoolDownTimers.ContainsKey(_timerName))
            {
                registeredCoolDownTimers[_timerName] = timeTarget;
            }
            else
            {
                registeredCoolDownTimers.Add(_timerName, timeTarget);
            }

            registeredCoolDowns[_timerName] = false;
        }

        public void StopTimer(string _timerName)
        {
            if (!ContainsTimer(_timerName))
            {
                return;
            }

            registeredCoolDowns[_timerName] = true;
            registeredCoolDownTimers.Remove(_timerName);
        }

        private void Update()
        {
            float currentTime = Time.realtimeSinceStartup;
            foreach (KeyValuePair<string, float> kv in registeredCoolDownTimers)
            {
                if (!(currentTime > kv.Value))
                {
                    continue;
                }

                registeredCoolDowns[kv.Key] = true;
                registeredCoolDownTimers.Remove(kv.Key);
            }
        }
    }
}