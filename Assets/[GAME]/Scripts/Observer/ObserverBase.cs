using UnityEngine;
using System;

namespace _GAME_.Scripts.Observer
{
    public class ObserverBase : MonoBehaviour
    {
        #region Protected Methods

        protected void Register(string eventName, Action handler)
        {
            ObserverManager.Register(eventName, handler);
        }

        protected void Unregister(string eventName, Action handler)
        {
            ObserverManager.Unregister(eventName, handler);
        }

        protected void Push(string eventName)
        {
            ObserverManager.Push(eventName);
        }

        #endregion
    }
}