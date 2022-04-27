using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    public abstract class LoadedEventSO<T> : EventSOBase<UnityEvent<T>>
    {
        [TitleGroup("Debug"), SerializeField, InlineButton("RaiseWithTestValue")] T testValue;
        public void RaiseWithTestValue() => Raise(testValue);

        public void Raise(T t)
        {
            if (logRaise) Debug.Log(
                $"{GetType().ToString().Replace("ToolBox.ScriptableObjects.Events.", "")} [<color=cyan>{name}</color>] has been raise with <color=yellow>{t}</color>");
            
            listeners.ForEach(l =>
            {
                if (logListeners) l.LogCallback(this, t);
                l.callbacks.Invoke(t);
            });
        }
    }
}