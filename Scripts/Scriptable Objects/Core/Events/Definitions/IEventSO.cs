using UnityEngine.Events;
namespace Toolbox.ScriptableObjects.Events
{
    public interface IEventSO<TCallbacks> where TCallbacks : UnityEventBase
    {
        public void AddListener(ReferencedEvent<TCallbacks> rEvent);
        public void RemoveListener(ReferencedEvent<TCallbacks> rEvent);
    }
}