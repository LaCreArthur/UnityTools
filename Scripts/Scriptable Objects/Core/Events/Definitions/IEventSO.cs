using UltEvents;

namespace Toolbox.ScriptableObjects.Events
{
    public interface IEventSO<TCallbacks> 
        where TCallbacks : UltEventBase
    {
        public void AddListener(ReferencedUltEvent<TCallbacks> ultEvent);
        public void RemoveListener(ReferencedUltEvent<TCallbacks> ultEvent);
    }
}