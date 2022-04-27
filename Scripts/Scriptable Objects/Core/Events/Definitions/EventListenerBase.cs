using UltEvents;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Events
{
    [ExecuteAlways]
    public abstract class EventListenerBase<TEvent, TCallbacks> : MonoBehaviour 
        where TEvent : IEventSO<TCallbacks> 
        //where TCallbacks : UltEventBase
    {
        [SerializeField] protected TEvent eventSO;
        [SerializeField] protected TCallbacks callbacks;
    
        protected void AddListener()
        {
            if (eventSO != null)
                eventSO.AddListener(new ReferencedUltEvent<TCallbacks>(this, callbacks));
        }

        protected void RemoveListener()
        {
            if (eventSO != null)
                eventSO.RemoveListener(new ReferencedUltEvent<TCallbacks>(this, callbacks));
        }
    
        protected void OnEnable() => AddListener();
        protected void OnDisable() => RemoveListener();
    }
}