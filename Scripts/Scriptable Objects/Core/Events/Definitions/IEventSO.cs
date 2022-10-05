using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    public interface IEventSO<TCallbacks> where TCallbacks : UnityEventBase
    {
        public void Add(ReferencedEvent<TCallbacks> rEvent);
        public void Remove(ReferencedEvent<TCallbacks> rEvent);
    }
}