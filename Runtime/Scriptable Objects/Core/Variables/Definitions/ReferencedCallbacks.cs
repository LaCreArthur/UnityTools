using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    public class ReferencedCallbacks<T> : ReferencedCallbacksBase<UnityEvent<T>>
    {
        [Space, SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true),
         OnInspectorGUI("RemoveNullLoadedRuntime")]
        List<ReferencedAction<T>> runtimeLoadedListeners = new List<ReferencedAction<T>>();

        void RemoveNullLoadedRuntime() => runtimeLoadedListeners?.RemoveAll(l => l.reference == null);

        internal void Add(Action<T> callback, bool dontAddDuplicate = false)
        {
            var listener = (Object)callback.Target;
            // look in listeners if the listener already exists
            ReferencedAction<T> existingListener = runtimeLoadedListeners.Find(l => l.reference == listener);
            if ((existingListener?.callbacks == null) || (existingListener.reference == null))
            {
                existingListener = new ReferencedAction<T>(new List<Action<T>>
                    {
                        callback
                    },
                    listener);
                runtimeLoadedListeners.Add(existingListener);
            }
            else
            {
                if (dontAddDuplicate && existingListener.callbacks.Contains(callback))
                    return;
                existingListener.callbacks.Add(callback);
            }
        }

        internal void RemoveRuntimeEvents()
        {
            runtimeLoadedListeners.Clear();
            runtimeListeners.Clear();
        }

        internal void Remove(Action<T> callback)
        {
            var listener = (Object)callback.Target;
            ReferencedAction<T> existingListener = runtimeLoadedListeners.Find(l => l.reference == listener);
            existingListener?.callbacks?.Remove(callback);
        }

        internal void Invoke(ScriptableObject caller, T param, bool logListeners)
        {
            foreach (ReferencedEvent<UnityEvent<T>> referencedEvent in persistentListeners)
            {
                if (logListeners)
                    referencedEvent.LogCallback(caller, param);

                referencedEvent.callbacks.Invoke(param);
            }

            foreach (ReferencedAction<T> referencedAction in runtimeLoadedListeners)
            {
                if (logListeners)
                    referencedAction.LogCallback(caller, param);

                referencedAction.callbacks.ForEach(c => c.Invoke(param));
            }

            base.Invoke(caller, logListeners);
        }
    }

    public class ReferencedCallbacks : ReferencedCallbacksBase<UnityEvent>
    {
        internal override void Invoke(ScriptableObject caller, bool logListeners)
        {
            foreach (ReferencedEvent<UnityEvent> referencedEvent in persistentListeners)
            {
                if (logListeners)
                    referencedEvent.LogCallback(caller, null);

                referencedEvent.callbacks.Invoke();
            }

            base.Invoke(caller, logListeners);
        }

        internal void Add(UnityEvent uEvent, Object listener) => Add(new ReferencedEvent<UnityEvent>(uEvent, listener));
        internal void Remove(UnityEvent uEvent, Object listener) => Remove(new ReferencedEvent<UnityEvent>(uEvent, listener));
    }

    public class ReferencedCallbacksBase<T> where T : UnityEventBase
    {
        // persistent listeners is a list of UnityEvent 
        [Space, SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true),
         OnInspectorGUI("RemoveNullPersistent")]
        protected List<ReferencedEvent<T>> persistentListeners = new List<ReferencedEvent<T>>();

        // runtime listeners is a list of 
        [Space, SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true), OnInspectorGUI("RemoveNullRuntime")]
        protected List<ReferencedAction> runtimeListeners = new List<ReferencedAction>();

        void RemoveNullPersistent() => persistentListeners?.RemoveAll(l => l.reference == null);

        void RemoveNullRuntime() => runtimeListeners?.RemoveAll(l => l.reference == null);

        internal void Add(Action callback, Object listener = null)
        {

            if (listener == null)
            {
                listener = (Object)callback.Target;
            }
            // look in listeners if the listener already exists
            ReferencedAction existingListener = runtimeListeners.Find(l => l.reference == listener);
            if ((existingListener?.callbacks == null) || (existingListener.reference == null))
            {
                existingListener = new ReferencedAction(new List<Action>
                    {
                        callback
                    },
                    listener);

                runtimeListeners.Add(existingListener);
            }
            else
            {
                existingListener.callbacks.Add(callback);
            }
        }

        internal void Remove(Action callback)
        {
            var listener = (Object)callback.Target;
            ReferencedAction existingListener = runtimeListeners.Find(l => l.reference == listener);
            existingListener?.callbacks?.Remove(callback);
            if (existingListener?.callbacks?.Count == 0)
            {
                runtimeListeners.Remove(existingListener);
            }
        }

        internal void RemoveAll(Func<ReferencedEvent<T>, bool> match)
        {
            if (persistentListeners == null)
                return;

            for (int i = persistentListeners.Count - 1; i >= 0; i--)
            {
                if (match(persistentListeners[i]))
                    persistentListeners.RemoveAt(i);
            }
        }

        internal void Add(ReferencedEvent<T> refAction)
        {
            persistentListeners ??= new List<ReferencedEvent<T>>();
            persistentListeners.Add(refAction);
        }

        internal void Remove(ReferencedEvent<T> refAction)
        {
            if (persistentListeners == null)
                return;

            ReferencedEvent<T> listener = persistentListeners.Find(l => l.reference == refAction.reference);
            persistentListeners.Remove(listener);
        }

        internal virtual void Invoke(ScriptableObject caller, bool logListeners)
        {
            foreach (ReferencedAction referencedAction in runtimeListeners)
            {
                if (logListeners)
                    referencedAction.LogCallback(caller);

                referencedAction.callbacks.ForEach(c => c.Invoke());
            }
        }
    }
}
