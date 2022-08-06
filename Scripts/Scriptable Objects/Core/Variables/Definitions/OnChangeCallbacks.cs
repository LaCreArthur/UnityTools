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
        [Space, SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, Expanded = true),
         OnInspectorGUI("RemoveNullPersistent")]
        List<ReferencedEvent<UnityEvent<T>>> persistentListeners = new List<ReferencedEvent<UnityEvent<T>>>();

        [Space, SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, Expanded = true),
         OnInspectorGUI("RemoveNullLoadedRuntime")]
        List<ReferencedAction<T>> runtimeLoadedListeners = new List<ReferencedAction<T>>();

        [Space, SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, Expanded = true),
         OnInspectorGUI("RemoveNullRuntime")]
        List<ReferencedAction> runtimeListeners = new List<ReferencedAction>();

        public List<ReferencedEvent<UnityEvent<T>>> PersistentListeners => persistentListeners;
        public List<ReferencedAction<T>> RuntimeLoadedListeners => runtimeLoadedListeners;
        public List<ReferencedAction> RuntimeListeners => runtimeListeners;

        void RemoveNullPersistent() => persistentListeners?.RemoveAll(l => l.reference == null);

        void RemoveNullLoadedRuntime() =>
            runtimeLoadedListeners?.RemoveAll(l => l.reference == null);

        void RemoveNullRuntime() => runtimeListeners?.RemoveAll(l => l.reference == null);

        public void Add(Action<T> callback, Object listener)
        {
            // look in listeners if the listener already exists
            var existingListener = runtimeLoadedListeners.Find(l => l.reference == listener);
            if (existingListener.callbacks == null || existingListener.reference == null)
            {
                existingListener = new ReferencedAction<T>(listener, new List<Action<T>> { callback });
                runtimeLoadedListeners.Add(existingListener);
            }
            else
                existingListener.callbacks.Add(callback);
        }

        public void Add(Action callback, Object listener)
        {
            // look in listeners if the listener already exists
            var existingListener = runtimeListeners.Find(l => l.reference == listener);
            if (existingListener.callbacks == null || existingListener.reference == null)
            {
                existingListener = new ReferencedAction(listener, new List<Action> { callback });
                runtimeListeners.Add(existingListener);
            }
            else
                existingListener.callbacks.Add(callback);
        }

        public void Remove(Action<T> callback, Object listener)
        {
            var existingListener = runtimeLoadedListeners.Find(l => l.reference == listener);
            existingListener.callbacks?.Remove(callback);
        }

        public void Remove(Action callback, Object listener)
        {
            var existingListener = runtimeListeners.Find(l => l.reference == listener);
            existingListener.callbacks?.Remove(callback);
        }

        public void RemoveAll(Func<ReferencedEvent<UnityEvent<T>>, bool> match)
        {
            if (persistentListeners == null)
                return;

            for (int i = persistentListeners.Count - 1; i >= 0; i--)
            {
                if (match(persistentListeners[i]))
                {
                    persistentListeners.RemoveAt(i);
                }
            }
        }

        public void Add(ReferencedEvent<UnityEvent<T>> refAction)
        {
            persistentListeners ??= new List<ReferencedEvent<UnityEvent<T>>>();
            persistentListeners.Add(refAction);
        }

        public void Remove(ReferencedEvent<UnityEvent<T>> refAction)
        {
            if (persistentListeners == null)
                return;

            var listener = persistentListeners.Find(l => l.reference == refAction.reference);
            persistentListeners.Remove(listener);
        }
    }
}