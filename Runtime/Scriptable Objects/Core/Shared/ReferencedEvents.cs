using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    public abstract class ReferencedEventBase<T>
    {

        [HideReferenceObjectPicker] [ListDrawerSettings(DefaultExpandedState = true)] public readonly T callbacks;
        [HideLabel] public readonly Object reference;

        protected ReferencedEventBase(T callbacks, Object reference)
        {
            this.reference = reference;
            this.callbacks = callbacks;
        }
    }

    public class ReferencedAction : ReferencedEventBase<List<Action>>
    {
        public ReferencedAction(List<Action> callbacks, Object reference) : base(callbacks, reference) {}

        public void LogCallback(ScriptableObject so)
        {
#if UNITY_EDITOR
            string header = LogHelper.HeaderStr(so.name, reference.name);
            callbacks.ForEach(c => LogHelper.LogMethodCall(c.Target, header, c.Method.Name));
#endif
        }
    }

    public class ReferencedAction<T> : ReferencedEventBase<List<Action<T>>>
    {
        public ReferencedAction(List<Action<T>> callbacks, Object reference) : base(callbacks, reference) {}

        public void LogCallback(ScriptableObject so, T t = default(T))
        {
#if UNITY_EDITOR
            string header = LogHelper.HeaderStr(so.name, reference.name);
            callbacks.ForEach(c => LogHelper.LogMethodCall(c.Target, header, c.Method.Name, t));
#endif
        }
    }

    public class ReferencedEvent<T> : ReferencedEventBase<T> where T : UnityEventBase
    {
        public ReferencedEvent(T callbacks, Object reference) : base(callbacks, reference) {}

        public void LogCallback(ScriptableObject so, object t)
        {
#if UNITY_EDITOR
            string header = LogHelper.HeaderStr(so.name, reference.name);
            UnityEventHelper.GetPersistentCalls("events", reference).ForEach(c => LogHelper.LogMethodCall(c, header, t));
#endif
        }
    }
}
