using UltEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    [ExecuteAlways]
    public abstract class EventListenerBase<TEvent, TCallbacks> : MonoBehaviour 
        where TEvent : IEventSO<TCallbacks> where TCallbacks : UnityEventBase
    //where TCallbacks : UltEventBase
    {
        [SerializeField] protected TEvent eventSO;
        [SerializeField] protected TCallbacks callbacks;
        [SerializeField] protected UltEvent ultCallbacks;
    
        protected void AddListener()
        {
            if (eventSO != null)
                eventSO.AddListener(new ReferencedUnityEvent<TCallbacks>(this, callbacks), new ReferencedUltEvent<UltEvent>(this, ultCallbacks));
        }

        protected void RemoveListener()
        {
            if (eventSO != null)
                eventSO.RemoveListener(new ReferencedUnityEvent<TCallbacks>(this, callbacks), new ReferencedUltEvent<UltEvent>(this, ultCallbacks));
        }
    
        protected void OnEnable() => AddListener();
        protected void OnDisable() => RemoveListener();
    }
}