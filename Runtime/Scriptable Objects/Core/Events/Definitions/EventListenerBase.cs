using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    [ExecuteAlways]
    public abstract class EventListenerBase<TEvent, TCallbacks> : MonoBehaviour
        where TEvent : ISOEvent<TCallbacks>
        where TCallbacks : UnityEventBase
    {
        [SerializeField] protected TEvent eventSO;
        [SerializeField] protected TCallbacks callbacks;

        protected void OnEnable()
        {
            if (eventSO != null)
                eventSO.Add(new ReferencedEvent<TCallbacks>(callbacks, this));
        }
        protected void OnDisable()
        {
            if (eventSO != null)
                eventSO.Remove(new ReferencedEvent<TCallbacks>(callbacks, this));
        }
    }
}
