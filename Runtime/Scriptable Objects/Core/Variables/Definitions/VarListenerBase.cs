using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    [ExecuteAlways]
    public abstract class VarListenerBase<T, TVariable> : MonoBehaviour where TVariable : SOVar<T>
    {
        [SerializeField]
        protected TVariable variable;

        public bool callEventsOnStart;
        public UnityEvent<T> events;

        void Subscribe()
        {
            if (variable != null)
                variable.onChange?.Add(new ReferencedEvent<UnityEvent<T>>(events, this));
        }

        void Unsubscribe()
        {
            if (variable != null)
                variable.onChange?.Remove(new ReferencedEvent<UnityEvent<T>>(events, this));
        }

        protected virtual void OnEnable() => Subscribe();

        protected virtual void OnDisable() => Unsubscribe();

        void Start()
        {
            if (callEventsOnStart)
                events.Invoke(variable.v);
        }
    }
}
