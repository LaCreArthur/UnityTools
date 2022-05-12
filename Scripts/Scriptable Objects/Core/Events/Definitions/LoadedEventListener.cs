using UltEvents;

namespace Toolbox.ScriptableObjects.Events
{
    public abstract class LoadedEventListener<T> : EventListenerBase<LoadedEventSO<T>, UltEvent<T>> {}
}