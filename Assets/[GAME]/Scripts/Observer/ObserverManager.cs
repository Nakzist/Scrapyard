using UnityEngine;
using System;
using System.Collections.Generic;

namespace _GAME_.Scripts.Observer
{
    public static class ObserverManager
    {
        #region Private Variables

        private static Dictionary<string, List<Action>> _functionListeners = new Dictionary<string, List<Action>>();

        #endregion

        #region Public Methods

        public static void Register(string eventName, Action handler)
        {
            if (!_functionListeners.ContainsKey(eventName))
            {
                _functionListeners[eventName] = new List<Action>();
            }

            _functionListeners[eventName].Add(handler);
        }

        public static void Unregister(string eventName, Action handler)
        {
            if (_functionListeners.TryGetValue(eventName, out var listener))
            {
                listener.Remove(handler);
            }
        }

        public static void Push(string eventName)
        {
            if (_functionListeners.TryGetValue(eventName, out var handlers))
            {
                foreach (var handler in handlers)
                {
                    handler?.Invoke();
                }
            }
            else
            {
                Debug.LogWarning($"Event '{eventName}' does not exist. Make sure to register it before notifying.");
            }
        }

        #endregion
    }
}