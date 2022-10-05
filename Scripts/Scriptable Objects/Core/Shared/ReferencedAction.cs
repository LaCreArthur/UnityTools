using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Toolbox.ScriptableObjects.Events
{
    public abstract class ReferencedActionBase<T>
    {
        [HideLabel]
        public readonly Object reference;

        [HideReferenceObjectPicker, ListDrawerSettings(Expanded = true)]
        public readonly T callbacks;

        protected ReferencedActionBase(T callbacks, Object reference)
        {
            this.reference = reference;
            this.callbacks = callbacks;
        }
    }

    public class ReferencedAction : ReferencedActionBase<List<Action>>
    {
        public ReferencedAction(List<Action> callbacks, Object reference) : base(callbacks, reference) { }

        public void LogCallback(ScriptableObject so)
        {
            string header = LogHelper.HeaderStr(so.name, reference.name);
            callbacks.ForEach(c => LogHelper.LogMethodCall(c.Target, header, c.Method.Name));
        }
    }

    public class ReferencedAction<T> : ReferencedActionBase<List<Action<T>>>
    {
        public ReferencedAction(List<Action<T>> callbacks, Object reference) : base(callbacks, reference) { }

        public void LogCallback(ScriptableObject so, object t)
        {
            string header = LogHelper.HeaderStr(so.name, reference.name);
            callbacks.ForEach(c => LogHelper.LogMethodCall(c.Target, header, c.Method.Name, t));
        }
    }
}