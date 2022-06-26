using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    public abstract class LoadedEventListener<T> : EventListenerBase<LoadedEventSO<T>, UnityEvent<T>> {}
}