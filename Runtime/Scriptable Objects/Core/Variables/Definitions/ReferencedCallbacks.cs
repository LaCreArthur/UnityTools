using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    [Serializable]
    public class ReferencedCallbacks<T> : ReferencedCallbacksBase<UnityEvent<T>>
    {
        [Space] [NonSerialized] [ShowInInspector] [InlineProperty] [HideReferenceObjectPicker] [ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true)]
        [OnInspectorGUI("RemoveNullLoadedRuntime")]
        List<ReferencedAction<T>> runtimeLoadedListeners = new List<ReferencedAction<T>>();

        void RemoveNullLoadedRuntime() => runtimeLoadedListeners?.RemoveAll(l => l.reference == null && !l.isStatic);

        internal void Add(Action<T> callback, bool dontAddDuplicate = false) => Add(callback, null, dontAddDuplicate);

        internal void Add(Action<T> callback, Object owner, bool dontAddDuplicate = false)
        {
            if (IsDispatching) { DeferMutation(() => Add(callback, owner, dontAddDuplicate)); return; }
            // Handle static methods differently; an explicit owner routes a closure/lambda under that owner so it becomes prunable
            Object listener = owner != null ? owner : callback.Target as Object;
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
            if (IsDispatching) { DeferMutation(RemoveRuntimeEvents); return; }
            runtimeLoadedListeners.Clear();
            runtimeListeners.Clear();
        }

        internal void Remove(Action<T> callback)
        {
            if (IsDispatching) { DeferMutation(() => Remove(callback)); return; }
            // Search by containment rather than reconstructing an owner from callback.Target —
            // owner-routed groups (Add(callback, owner)) can't be found from the callback alone otherwise.
            ReferencedAction<T> existingListener = runtimeLoadedListeners.Find(l => l.callbacks != null && l.callbacks.Contains(callback));

            existingListener?.callbacks?.Remove(callback);

            // Clean up empty listeners
            if (existingListener?.callbacks?.Count == 0)
            {
                runtimeLoadedListeners.Remove(existingListener);
            }
        }

        internal void Remove(Action<T> callback, Object owner)
        {
            if (IsDispatching) { DeferMutation(() => Remove(callback, owner)); return; }
            bool isStatic = owner == null;

            ReferencedAction<T> existingListener = runtimeLoadedListeners.Find(l =>
                isStatic && l.isStatic || !isStatic && l.reference == owner);

            existingListener?.callbacks?.Remove(callback);

            // Clean up empty listeners
            if (existingListener?.callbacks?.Count == 0)
            {
                runtimeLoadedListeners.Remove(existingListener);
            }
        }

        internal void Invoke(ScriptableObject caller, T param, bool logListeners)
        {
            BeginDispatch();
            try
            {
                for (int i = 0; i < persistentListeners.Count; i++)
                {
                    ReferencedEvent<UnityEvent<T>> referencedEvent = persistentListeners[i];
                    if (referencedEvent.reference == null)
                    {
                        ReferencedEvent<UnityEvent<T>> toPrune = referencedEvent;
                        DeferMutation(() => persistentListeners.Remove(toPrune));
                        continue;
                    }
                    if (logListeners)
                        referencedEvent.LogCallback(caller, param);

                    referencedEvent.callbacks.Invoke(param);
                }

                for (int i = 0; i < runtimeLoadedListeners.Count; i++)
                {
                    ReferencedAction<T> referencedAction = runtimeLoadedListeners[i];
                    if (referencedAction?.callbacks == null)
                    {
                        Debug.LogError($"[Callbacks] null runtime listener on '{caller?.name}' — skipped. Likely stale serialized data; report this asset.");
                        continue;
                    }
                    if (!referencedAction.isStatic && referencedAction.reference == null)
                    {
                        ReferencedAction<T> toPrune = referencedAction;
                        DeferMutation(() => runtimeLoadedListeners.Remove(toPrune));
                        continue;
                    }
                    if (logListeners)
                        referencedAction.LogCallback(caller, param);

                    List<Action<T>> callbacks = referencedAction.callbacks;
                    for (int callbackIndex = 0; callbackIndex < callbacks.Count; callbackIndex++)
                        callbacks[callbackIndex].Invoke(param);
                }

                base.Invoke(caller, logListeners);
            }
            finally
            {
                EndDispatch();
            }
        }
    }

    [Serializable]
    public class ReferencedCallbacks : ReferencedCallbacksBase<UnityEvent>
    {
        internal override void Invoke(ScriptableObject caller, bool logListeners, bool? onEnter = null)
        {
            BeginDispatch();
            try
            {
                for (int i = 0; i < persistentListeners.Count; i++)
                {
                    ReferencedEvent<UnityEvent> referencedEvent = persistentListeners[i];
                    if (referencedEvent?.callbacks == null)
                    {
                        Debug.LogError($"[Callbacks] null persistent listener on '{caller?.name}' (owner: {(referencedEvent?.reference ? referencedEvent.reference.name : "null")}) — skipped. Likely stale serialized data; report this asset.");
                        continue;
                    }
                    if (referencedEvent.reference == null)
                    {
                        ReferencedEvent<UnityEvent> toPrune = referencedEvent;
                        DeferMutation(() => persistentListeners.Remove(toPrune));
                        continue;
                    }
                    if (logListeners)
                        referencedEvent.LogCallback(caller, null, onEnter);

                    referencedEvent.callbacks.Invoke();
                }

                base.Invoke(caller, logListeners, onEnter);
            }
            finally
            {
                EndDispatch();
            }
        }

        internal void Add(UnityEvent uEvent, Object listener) => Add(new ReferencedEvent<UnityEvent>(uEvent, listener));
        internal void Remove(UnityEvent uEvent, Object listener) => Remove(new ReferencedEvent<UnityEvent>(uEvent, listener));
    }

    [Serializable]
    public class ReferencedCallbacksBase<T> where T : UnityEventBase
    {
        int dispatchDepth;
        readonly List<Action> deferredMutations = new List<Action>();

        protected bool IsDispatching => dispatchDepth != 0;

        protected bool DeferMutation(Action mutation)
        {
            if (dispatchDepth == 0) return false;
            deferredMutations.Add(mutation);
            return true;
        }

        protected void BeginDispatch() => dispatchDepth++;

        protected void EndDispatch()
        {
            dispatchDepth--;
            if (dispatchDepth != 0 || deferredMutations.Count == 0) return;

            for (int i = 0; i < deferredMutations.Count; i++)
                deferredMutations[i].Invoke();
            deferredMutations.Clear();
        }
        // persistent listeners is a list of UnityEvent 
        [Space] [SerializeField] [InlineProperty] [HideReferenceObjectPicker] [ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true)]
        [OnInspectorGUI("RemoveNullPersistent")]
        protected List<ReferencedEvent<T>> persistentListeners = new List<ReferencedEvent<T>>();

        [Space] [NonSerialized] [ShowInInspector] [InlineProperty] [HideReferenceObjectPicker] [ListDrawerSettings(IsReadOnly = true, DefaultExpandedState = true)]
        [OnInspectorGUI("RemoveNullRuntime")]
        protected List<ReferencedAction> runtimeListeners = new List<ReferencedAction>();

        void RemoveNullPersistent() => persistentListeners?.RemoveAll(l => l.reference == null);
        void RemoveNullRuntime()
        {
            if (!Application.isPlaying) runtimeListeners.Clear();
            else runtimeListeners?.RemoveAll(l => l.reference == null && !l.isStatic);
        }

        public void RemoveAll()
        {
            if (IsDispatching) { DeferMutation(RemoveAll); return; }
            persistentListeners.Clear();
            runtimeListeners.Clear();
        }

        internal void Add(Action callback, Object listener = null, bool dontAddDuplicate = false)
        {
            if (IsDispatching) { DeferMutation(() => Add(callback, listener, dontAddDuplicate)); return; }
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
            if (IsDispatching) { DeferMutation(() => Remove(callback)); return; }
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
            if (IsDispatching) { DeferMutation(() => RemoveAll(match)); return; }
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
            if (IsDispatching) { DeferMutation(() => Add(refAction)); return; }
            persistentListeners ??= new List<ReferencedEvent<T>>();
            persistentListeners.Add(refAction);
        }

        internal void Remove(ReferencedEvent<T> refAction)
        {
            if (IsDispatching) { DeferMutation(() => Remove(refAction)); return; }
            if (persistentListeners == null)
                return;

            ReferencedEvent<T> listener = persistentListeners.Find(l => l.reference == refAction.reference);
            persistentListeners.Remove(listener);
        }

        internal virtual void Invoke(ScriptableObject caller, bool logListeners, bool? onEnter = null)
        {
            BeginDispatch();
            try
            {
                for (int i = 0; i < runtimeListeners.Count; i++)
                {
                    ReferencedAction referencedAction = runtimeListeners[i];
                    if (referencedAction?.callbacks == null)
                    {
                        Debug.LogError($"[Callbacks] null runtime listener on '{caller?.name}' — skipped. Likely stale serialized data; report this asset.");
                        continue;
                    }
                    if (!referencedAction.isStatic && referencedAction.reference == null)
                    {
                        ReferencedAction toPrune = referencedAction;
                        DeferMutation(() => runtimeListeners.Remove(toPrune));
                        continue;
                    }
                    if (logListeners)
                        referencedAction.LogCallback(caller, onEnter);

                    List<Action> callbacks = referencedAction.callbacks;
                    for (int callbackIndex = 0; callbackIndex < callbacks.Count; callbackIndex++)
                        callbacks[callbackIndex].Invoke();
                }
            }
            finally
            {
                EndDispatch();
            }
        }
    }
}
