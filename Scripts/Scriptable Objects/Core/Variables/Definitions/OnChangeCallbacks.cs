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
        List<ReferencedUnityEvent<UnityEvent<T>>> listeners = new List<ReferencedUnityEvent<UnityEvent<T>>>();

        public List<ReferencedUnityEvent<UnityEvent<T>>> Listeners => listeners;
        void RemoveNullElements() => listeners?.RemoveAll(l => l.listener == null);

        public void Add(Action<T> callback, Object listener)
        {
        }

        public void Add(Action callback, Object listener)
        {
        }

        public void Add(UnityEvent<T> ultEvent, Object listener)
        {
        }

        public void Remove(Action<T> callback, Object listener)
        {
        }

        public void Remove(Action callback, Object listener)
        {
        }

        public void RemoveAll(Func<ReferencedUnityEvent<UnityEvent<T>>, bool> match)
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

        public void Remove(UnityEvent<T> ultEvent, Object listener)
        {
        }

        public void Add(ReferencedUnityEvent<UnityEvent<T>> unityEvent)
        {
            if (listeners == null) listeners = new List<ReferencedUnityEvent<UnityEvent<T>>>();
            listeners.Add(unityEvent);
        }

        public void Remove(ReferencedUnityEvent<UnityEvent<T>> unityEvent)
        {
            if (listeners == null) return;
            var listener = listeners.Find(l => l.listener == unityEvent.listener && Equals(l.callbacks, unityEvent.callbacks));
            listeners.Remove(listener);
        }
    }
}