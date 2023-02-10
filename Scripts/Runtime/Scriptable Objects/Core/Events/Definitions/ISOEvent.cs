using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    public interface ISOEvent<TCallbacks> where TCallbacks : UnityEventBase
    {
        public void Add(ReferencedEvent<TCallbacks> rEvent);
        public void Remove(ReferencedEvent<TCallbacks> rEvent);
    }
}