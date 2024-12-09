using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    [ExecuteAlways]
    public abstract class VarListenerBase<T, TVariable> : MonoBehaviour where TVariable : SOVar<T>
    {
        [SerializeField] protected TVariable variable;

        public bool callEventsOnStart;
        public UnityEvent<T> events;

        void Awake()
        {
            if (events?.GetPersistentEventCount() > 0)
                variable?.AddOnChange(new ReferencedEvent<UnityEvent<T>>(events, this));
            OnAwake();
        }

        protected virtual void OnAwake() {}

        void Start()
        {
            if (callEventsOnStart)
                events.Invoke(variable.v);
        }

        void OnDestroy()
        {
            if (events?.GetPersistentEventCount() > 0)
                variable?.RemoveOnChange(new ReferencedEvent<UnityEvent<T>>(events, this));
            OnOnDestroy();
        }
        protected virtual void OnOnDestroy() {}
    }
}
