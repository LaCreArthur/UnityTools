using System;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Variables
{
    public class RegistrableScriptableObject : ScriptableObject
    {
        private Action _onChange;

        protected virtual void OnEnable() {}

        protected void TriggerChange()
        {
            _onChange?.Invoke();
        }

        public void AddOnChangeCallback(Action callback)
        {
            _onChange += callback;
        }

        public void RemoveOnChangeCallback(Action callback)
        {
            _onChange -= callback;
        }
    }
}