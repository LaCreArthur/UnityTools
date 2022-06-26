using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    public class EventListener : EventListenerBase<EventSO, UnityEvent>
    {
        protected const string Filter = "t:EventSO";
    }
}