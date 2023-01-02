using System;
using AS.Toolbox.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    public abstract class LoadedSOEvent<T> : SOEventBase, ISOEvent<UnityEvent<T>>
    {
        [TitleGroup("Debug"), SerializeField, InlineButton("RaiseWithTestValue")]
        T testValue;

        public void RaiseWithTestValue() => Raise(testValue);

        [TitleGroup("Listener"), HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullElements")]
        public ReferencedCallbacks<T> listeners = new ReferencedCallbacks<T>();

        void RemoveNullElements() => listeners.RemoveAll(l => l.reference == null);

        public void Add(ReferencedEvent<UnityEvent<T>> rEvent) => listeners.Add(rEvent);
        public void Add(Action<T> action, Object listener) => listeners.Add(action, listener);

        public void Remove(ReferencedEvent<UnityEvent<T>> rEvent) => listeners.Remove(rEvent);
        public void Remove(Action<T> action, Object listener) => listeners.Remove(action, listener);

        public void Raise(T value)
        {
            if (logRaise)
                Debug.Log($"{this.TypeAndNameToString()} has been raise with <color=yellow>{value}</color>");

            listeners.Invoke(this, value, logListeners);
        }
    }
}