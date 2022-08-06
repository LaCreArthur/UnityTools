using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Toolbox.ScriptableObjects.Events
{
    public readonly struct ReferencedAction
    {
        [HideLabel]
        public readonly Object reference;

        [HideReferenceObjectPicker, ListDrawerSettings(Expanded = true)]
        public readonly List<Action> callbacks;

        public ReferencedAction(Object reference, List<Action> callbacks)
        {
            this.reference = reference;
            this.callbacks = callbacks;
        }

        public void LogCallback(ScriptableObject so)
        {
            string header = LogHelper.HeaderStr(so.name, reference.name);
            callbacks.ForEach(c => LogHelper.LogMethodCall(c.Target, header, c.Method.Name));
        }
    }

    public readonly struct ReferencedAction<T>
    {
        [HideLabel]
        public readonly Object reference;

        [HideReferenceObjectPicker, ListDrawerSettings(Expanded = true)]
        public readonly List<Action<T>> callbacks;

        public ReferencedAction(Object reference, List<Action<T>> callbacks)
        {
            this.reference = reference;
            this.callbacks = callbacks;
        }

        public void LogCallback(ScriptableObject so, object t)
        {
            string header = LogHelper.HeaderStr(so.name, reference.name);
            callbacks.ForEach(c => LogHelper.LogMethodCall(c.Target, header, c.Method.Name, t));
        }
    }
}