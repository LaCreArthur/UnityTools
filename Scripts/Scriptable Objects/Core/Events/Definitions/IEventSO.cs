using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    public interface IEventSO<TCallbacks> 
        where TCallbacks : UnityEventBase
    //where TCallbacks : UltEventBase
    {
        public void AddListener(ReferencedUnityEvent<TCallbacks> referencedUnityEvent);
        public void RemoveListener(ReferencedUnityEvent<TCallbacks> referencedUnityEvent);
    }
}