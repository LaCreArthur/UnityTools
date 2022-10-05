using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Toolbox.ScriptableObjects.Events
{
    [ExecuteAlways]
    public abstract class EventListenerBase<TEvent, TCallbacks> : MonoBehaviour
        where TEvent : IEventSO<TCallbacks>
        where TCallbacks : UnityEventBase
    {
        [SerializeField] protected TEvent eventSO;
        [FormerlySerializedAs("callbacks")]
        [SerializeField] protected TCallbacks events;

        protected void AddListener()
        {
            if (eventSO != null)
                eventSO.Add(new ReferencedEvent<TCallbacks>(events, this));
        }

        protected void RemoveListener()
        {
            if (eventSO != null)
                eventSO.Remove(new ReferencedEvent<TCallbacks>(events, this));
        }

        protected void OnEnable() => AddListener();
        protected void OnDisable() => RemoveListener();
    }
}