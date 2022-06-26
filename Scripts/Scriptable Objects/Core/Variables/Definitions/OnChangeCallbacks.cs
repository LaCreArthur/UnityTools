using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Toolbox.ScriptableObjects.Variables
{
    public class OnChangeCallbacks<T>
    {
        [Space, SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, Expanded = true), OnInspectorGUI("RemoveNullElements")] 
        List<ReferencedEvent<UnityEvent<T>>> listeners = new List<ReferencedEvent<UnityEvent<T>>>();

        public List<ReferencedEvent<UnityEvent<T>>> Listeners => listeners;
        void RemoveNullElements() => listeners?.RemoveAll(l => l.reference == null || l.callbacks.GetPersistentEventCount() == 0);

        public void Add(Action<T> callback, Object listener)
        {
            // look in listeners if the listener already exists
            var existingListener = listeners.Find(l => l.reference == listener);
            if (existingListener == null || existingListener.reference == null)
            {
                existingListener = new ReferencedEvent<UnityEvent<T>>(listener, new UnityEvent<T>());
            }
            
            existingListener.callbacks.AddListener(callback as UnityAction<T>);
        }

        public void Add(Action callback, Object listener)
        {
            // look in listeners if the listener already exists
            var existingListener = listeners.Find(l => l.reference == listener);
            if (existingListener == null || existingListener.reference == null)
            {
                existingListener = new ReferencedEvent<UnityEvent<T>>(listener, new UnityEvent<T>());
            }
            
            existingListener.callbacks.AddListener(callback as UnityAction<T>);
        }

        public void Remove(Action<T> callback, Object listener)
        {
            var existingListener = listeners.Find(l => l.reference == listener);
            if (existingListener != null)
            {
                existingListener.callbacks.RemoveListener(callback as UnityAction<T>);
            }
        }

        public void Remove(Action callback, Object listener)
        {
            var existingListener = listeners.Find(l => l.reference == listener);
            if (existingListener != null)
            {
                existingListener.callbacks.RemoveListener(callback as UnityAction<T>);
            }
        }

        public void RemoveAll(Func<ReferencedEvent<UnityEvent<T>>, bool> match)
        {
            if (listeners == null) return;
            for (int i = listeners.Count - 1; i >= 0 ; i--)
            {
                if (match(listeners[i]))
                {
                    listeners.RemoveAt(i);
                }
            }
        }

        public void Add(ReferencedEvent<UnityEvent<T>> refAction)
        {
            if (listeners == null) listeners = new List<ReferencedEvent<UnityEvent<T>>>();
            listeners.Add(refAction);
        }

        public void Remove(ReferencedEvent<UnityEvent<T>> refAction)
        {
            if (listeners == null) return;
            var listener = listeners.Find(l => l.reference == refAction.reference);
            listeners.Remove(listener);
        }
    }
}