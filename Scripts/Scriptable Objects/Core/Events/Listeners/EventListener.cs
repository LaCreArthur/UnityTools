using UltEvents;

namespace Toolbox.ScriptableObjects.Events
{
    public class EventListener : EventListenerBase<EventSO, UltEvent>
    {
        protected const string Filter = "t:EventSO";
    }
}