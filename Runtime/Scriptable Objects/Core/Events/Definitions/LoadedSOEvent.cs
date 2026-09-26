using System;
using AS.Toolbox.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    public abstract class LoadedSOEvent<T> : SOEventBase, ISOEvent<UnityEvent<T>>
    {
        [TitleGroup("Debug")] [SerializeField] [InlineButton("RaiseWithTestValue")] T testValue;

        [TitleGroup("Listener")]
        [NonSerialized] [ShowInInspector]
        [HideLabel]
        [InlineProperty]
        [HideReferenceObjectPicker]
        [OnInspectorGUI("RemoveNullElements")]
        public ReferencedCallbacks<T> listeners = new ReferencedCallbacks<T>();

        public void Add(ReferencedEvent<UnityEvent<T>> rEvent) => listeners.Add(rEvent);

        public void Remove(ReferencedEvent<UnityEvent<T>> rEvent) => listeners.Remove(rEvent);

        public void RaiseWithTestValue() => Raise(testValue);

        void RemoveNullElements() => listeners.RemoveAll(l => l.reference == null);
        public void Add(Action<T> action, bool dontAddDuplicate = false) => listeners.Add(action, dontAddDuplicate);
        public void Add(Action<T> action, UnityEngine.Object owner, bool dontAddDuplicate = false) => listeners.Add(action, owner, dontAddDuplicate);
        public void Remove(Action<T> action) => listeners.Remove(action);

        public void Raise(T value)
        {
#if UNITY_EDITOR || UNITY_INCLUDE_INSTRUMENTATION
            if (logRaise)
                Debug.Log($"{this.TypeAndNameToString()} has been raise with <color=yellow>{value}</color>");
#endif

            listeners.Invoke(this, value, logListeners);
        }
    }
}
