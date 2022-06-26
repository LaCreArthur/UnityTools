using Sirenix.OdinInspector;
using Toolbox.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    public abstract class LoadedEventSO<T> : EventSOBase<UnityEvent<T>>
    {
        [TitleGroup("Debug"), SerializeField, InlineButton("RaiseWithTestValue")] T testValue;
        public void RaiseWithTestValue() => Raise(testValue);

        public void Raise(T value)
        {
            if (logRaise) Debug.Log(
                $"{this.TypeAndNameToString()} has been raise with <color=yellow>{value}</color>");
            
            listeners.ForEach(l =>
            {
                if (logListeners) l.LogCallback(this, value);
                l.callbacks?.Invoke(value);
            });
        }
    }
}