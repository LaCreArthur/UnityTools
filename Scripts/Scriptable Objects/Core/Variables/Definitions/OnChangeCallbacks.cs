using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects.Events;
using UltEvents;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Toolbox.ScriptableObjects.Variables
{
    public class OnChangeCallbacks<T>
    {
        [Space, SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, Expanded = true), OnInspectorGUI("RemoveNullElements")] 
        List<ReferencedUltEvent<UltEvent<T>>> listeners = new List<ReferencedUltEvent<UltEvent<T>>>();

        public List<ReferencedUltEvent<UltEvent<T>>> Listeners => listeners;
        void RemoveNullElements() => listeners?.RemoveAll(l => l.listener == null || !l.callbacks.HasCalls);

        public void Add(Action<T> callback, Object listener)
        {
            // look in listeners if the listener already exists
            var existingListener = listeners.Find(l => l.listener == listener);
            if (existingListener?.listener != null)
            {
                existingListener.callbacks.AddPersistentCall(callback);
            }
            else
            {
                var ultEv = new UltEvent<T>();
                ultEv.AddPersistentCall(callback);
                listeners.Add(new ReferencedUltEvent<UltEvent<T>>(listener, ultEv));
            }
        }

        public void Add(Action callback, Object listener)
        {
            // look in listeners if the listener already exists
            var existingListener = listeners.Find(l => l.listener == listener);
            if (existingListener?.listener != null)
            {
                existingListener.callbacks.AddPersistentCall(callback);
            }
            else
            {
                var ultEv = new UltEvent<T>();
                ultEv.AddPersistentCall(callback);
                listeners.Add(new ReferencedUltEvent<UltEvent<T>>(listener, ultEv));
            }
        }

        public void Add(UltEvent<T> ultEvent, Object listener)
        {
            // look in listeners if the listener already exists
            var existingListener = listeners.Find(l => l.listener == listener);
            if (existingListener?.listener != null)
            {
                existingListener.callbacks.AddRange(ultEvent);
            }
            else
            {
                listeners.Add(new ReferencedUltEvent<UltEvent<T>>(listener, ultEvent));
            }
        }

        public void Remove(Action<T> callback, Object listener)
        {
            var existingListener = listeners.Find(l => l.listener == listener);
            if (existingListener != null)
            {
                existingListener.callbacks.DynamicCalls -= callback;
            }
        }

        public void Remove(Action callback, Object listener)
        {
            var existingListener = listeners.Find(l => l.listener == listener);
            if (existingListener != null)
            {
                existingListener.callbacks.RemovePersistentCall(callback);
            }
        }

        public void RemoveAll(Func<ReferencedUltEvent<UltEvent<T>>, bool> match)
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

        public void Remove(UltEvent<T> ultEvent, Object listener)
        {
            var existingListener = listeners.Find(l => l.listener == listener);
            if (existingListener != null && ultEvent.HasCalls && ultEvent.PersistentCallsList != null)
            {
                foreach (PersistentCall call in ultEvent.PersistentCallsList)
                {
                    var pc = new PersistentCall();
                    pc.CopyFrom(call);
                    existingListener.callbacks.PersistentCallsList.Remove(pc);
                }
            }
        }

        public void Add(ReferencedUltEvent<UltEvent<T>> ultEvent)
        {
            if (listeners == null) listeners = new List<ReferencedUltEvent<UltEvent<T>>>();
            listeners.Add(ultEvent);
        }

        public void Remove(ReferencedUltEvent<UltEvent<T>> ultEvent)
        {
            if (listeners == null) return;
            var listener = listeners.Find(l => l.listener == ultEvent.listener && Equals(l.callbacks, ultEvent.callbacks));
            listeners.Remove(listener);
        }
    }
}