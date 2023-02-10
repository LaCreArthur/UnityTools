using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    public abstract class LoadedEventListener<T> : EventListenerBase<LoadedSOEvent<T>, UnityEvent<T>> {}
}