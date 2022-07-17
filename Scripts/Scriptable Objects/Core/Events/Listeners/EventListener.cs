#pragma warning disable CS0108, CS0114
using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    public class EventListener : EventListenerBase<EventSO, UnityEvent>
    {
        protected const string Filter = "t:EventSO";
    }
}