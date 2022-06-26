using Sirenix.OdinInspector;
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
        protected const string Filter = ""; 
        [SerializeField, AssetSelector(Filter = Filter)] protected TEvent eventSO;
        [FormerlySerializedAs("callbacks")]
        [SerializeField] protected TCallbacks events;
    
        protected void AddListener()
        {
            if (eventSO != null)
                eventSO.AddListener(new ReferencedEvent<TCallbacks>(this, events));
        }

        protected void RemoveListener()
        {
            if (eventSO != null)
                eventSO.RemoveListener(new ReferencedEvent<TCallbacks>(this, events));
        }
    
        protected void OnEnable() => AddListener();
        protected void OnDisable() => RemoveListener();
    }
}