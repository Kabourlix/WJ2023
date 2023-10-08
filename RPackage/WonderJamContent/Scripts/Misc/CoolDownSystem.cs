// Copyrighted by team Rézoskour
// Created by Kabourlix Cendrée on 07

#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rezoskour.Content.Misc
{
    public class CoolDownSystem : MonoBehaviour
    {
        #region Singleton

        public static CoolDownSystem? Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #endregion

        public event Action<string>? OnCoolDownDone;
        public event Action<string, float>? OnCoolDownUpdates;

        private Dictionary<string, bool> registeredCoolDowns = new();
        private Dictionary<string, float> registeredCoolDownDurations = new();
        
        private Dictionary<string, bool>
            registeredCdWithNotification = new(); //The list of cd that will notify each time it is updated.


        private Dictionary<string, float> registeredCoolDownTimers = new();

        private bool ContainsTimer(string _timerName)
        {
            return registeredCoolDowns.ContainsKey(_timerName) || registeredCoolDownDurations.ContainsKey(_timerName);
        }
        

        public bool TryRegisterCoolDown(string _timerName, float _duration, bool _notifyOnUpdate = false)
        {
            if (ContainsTimer(_timerName))
            {
                Debug.LogWarning("[REZOSKOUR] Cannot add the same timer twice.");
                return false;
            }

            registeredCoolDowns.Add(_timerName, true);
            registeredCoolDownDurations.Add(_timerName, _duration);
            registeredCdWithNotification.Add(_timerName, _notifyOnUpdate);

            return true;
        }

        public bool TryUnRegisterCoolDown(string _timerName)
        {
            if (!ContainsTimer(_timerName))
            {
                return false;
            }

            registeredCoolDowns.Remove(_timerName);
            registeredCoolDownDurations.Remove(_timerName);
            registeredCoolDownTimers.Remove(_timerName);
            registeredCdWithNotification.Remove(_timerName);
            return true;
        }

        public bool IsCoolDownDone(string _timerName)
        {
            if (!ContainsTimer(_timerName))
            {
                Debug.LogError($"[REZOSKOUR] {_timerName} is not registered in the cd system.");
                return true;
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

            float timeTarget = registeredCoolDownDurations[_timerName];

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
            Dictionary<string, float>? iterationDict = new(registeredCoolDownTimers);
            foreach (KeyValuePair<string, float> kv in iterationDict)
            {
                if (registeredCdWithNotification[kv.Key] && Time.timeScale > 0)
                {
                    OnCoolDownUpdates?.Invoke(kv.Key, kv.Value);
                }

                if (kv.Value > 0)
                {
                    registeredCoolDownTimers[kv.Key] -= Time.deltaTime;
                    continue;
                }

                registeredCoolDowns[kv.Key] = true;
                OnCoolDownDone?.Invoke(kv.Key);
                registeredCoolDownTimers.Remove(kv.Key);
            }
        }
    }
}