using Sirenix.OdinInspector;
using Toolbox.Utils;
using UltEvents;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Events
{
    public abstract class LoadedEventSO<T> : EventSOBase<UltEvent<T>>
    {
        [TitleGroup("Debug"), SerializeField, InlineButton("RaiseWithTestValue")] T testValue;
        public void RaiseWithTestValue() => Raise(testValue);

        public void Raise(T t)
        {
            if (logRaise) Debug.Log(
                $"{this.TypeAndNameToString()} has been raise with <color=yellow>{t}</color>");
            
            listeners.ForEach(l =>
            {
                if (logListeners) l.LogCallback(this, t);
                l.callbacks?.InvokeSafe(t);
            });
        }
    }
}