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
        [Space] [SerializeField] [InlineProperty] [HideReferenceObjectPicker] [ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true)]
        [OnInspectorGUI("RemoveNullLoadedRuntime")]
        List<ReferencedAction<T>> runtimeLoadedListeners = new List<ReferencedAction<T>>();

        void RemoveNullLoadedRuntime() => runtimeLoadedListeners?.RemoveAll(l => l.reference == null && !l.isStatic);

        internal void Add(Action<T> callback, bool dontAddDuplicate = false)
        {
            // Handle static methods differently
            Object listener = null;
            if (callback.Target is Object target) listener = target;
            bool isStatic = listener == null;

            // Look for existing listener based on reference or static status
            ReferencedAction<T> existingListener = runtimeLoadedListeners.Find(l =>
                isStatic && l.isStatic || !isStatic && l.reference == listener);

            if (existingListener?.callbacks == null || !isStatic && existingListener.reference == null)
            {
                existingListener = new ReferencedAction<T>(new List<Action<T>>
                    {
                        callback,
                    },
                    listener,
                    isStatic);
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
            Object listener = null;
            if (callback.Target is Object target) listener = target;
            bool isStatic = listener == null;

            ReferencedAction<T> existingListener = runtimeLoadedListeners.Find(l =>
                isStatic && l.isStatic || !isStatic && l.reference == listener);

            existingListener?.callbacks?.Remove(callback);

            // Clean up empty listeners
            if (existingListener?.callbacks?.Count == 0)
            {
                runtimeLoadedListeners.Remove(existingListener);
            }
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
        internal override void Invoke(ScriptableObject caller, bool logListeners, bool? onEnter = null)
        {
            foreach (ReferencedEvent<UnityEvent> referencedEvent in persistentListeners)
            {
                if (logListeners)
                    referencedEvent.LogCallback(caller, null, onEnter);

                referencedEvent.callbacks.Invoke();
            }

            base.Invoke(caller, logListeners, onEnter);
        }

        internal void Add(UnityEvent uEvent, Object listener) => Add(new ReferencedEvent<UnityEvent>(uEvent, listener));
        internal void Remove(UnityEvent uEvent, Object listener) => Remove(new ReferencedEvent<UnityEvent>(uEvent, listener));
    }

    public class ReferencedCallbacksBase<T> where T : UnityEventBase
    {
        // persistent listeners is a list of UnityEvent 
        [Space] [SerializeField] [InlineProperty] [HideReferenceObjectPicker] [ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true)]
        [OnInspectorGUI("RemoveNullPersistent")]
        protected List<ReferencedEvent<T>> persistentListeners = new List<ReferencedEvent<T>>();

        [Space] [SerializeField] [InlineProperty] [HideReferenceObjectPicker] [ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true)]
        [OnInspectorGUI("RemoveNullRuntime")]
        protected List<ReferencedAction> runtimeListeners = new List<ReferencedAction>();

        void RemoveNullPersistent() => persistentListeners?.RemoveAll(l => l.reference == null);
        void RemoveNullRuntime() => runtimeListeners?.RemoveAll(l => l.reference == null && !l.isStatic);

        public void RemoveAll()
        {
            persistentListeners.Clear();
            runtimeListeners.Clear();
        }

        internal void Add(Action callback, Object listener = null, bool dontAddDuplicate = false)
        {
            if (listener == null && callback.Target is Object target)
                listener = target;
            bool isStatic = listener == null;

            // Look for existing listener based on reference or static status
            ReferencedAction existingListener = runtimeListeners.Find(l =>
                isStatic && l.isStatic || !isStatic && l.reference == listener);

            if (existingListener?.callbacks == null || !isStatic && existingListener.reference == null)
            {
                existingListener = new ReferencedAction(new List<Action>
                    {
                        callback,
                    },
                    listener,
                    isStatic);
                runtimeListeners.Add(existingListener);
            }
            else
            {
                if (dontAddDuplicate && existingListener.callbacks.Contains(callback))
                    return;
                existingListener.callbacks.Add(callback);
            }
        }

        internal void Remove(Action callback)
        {
            Object listener = null;
            if (callback.Target is Object target) listener = target;
            bool isStatic = listener == null;

            ReferencedAction existingListener = runtimeListeners.Find(l =>
                isStatic && l.isStatic || !isStatic && l.reference == listener);

            existingListener?.callbacks?.Remove(callback);

            // Clean up empty listeners
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

        internal virtual void Invoke(ScriptableObject caller, bool logListeners, bool? onEnter = null)
        {
            foreach (ReferencedAction referencedAction in runtimeListeners)
            {
                if (logListeners)
                    referencedAction.LogCallback(caller, onEnter);

                referencedAction.callbacks.ForEach(c => c.Invoke());
            }
        }
    }
}
