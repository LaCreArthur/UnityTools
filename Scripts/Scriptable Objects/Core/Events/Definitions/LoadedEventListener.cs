using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    public class LoadedEventListener<T> : EventListenerBase<LoadedEventSO<T>, UnityEvent<T>> {}
}