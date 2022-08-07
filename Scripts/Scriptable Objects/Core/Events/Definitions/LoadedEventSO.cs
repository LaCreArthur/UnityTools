using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects.Variables;
using Toolbox.Utils;
using UnityEngine;
using UnityEngine.Events;
namespace Toolbox.ScriptableObjects.Events
{
    public abstract class LoadedEventSO<T> : EventSOBase, IEventSO<UnityEvent<T>>
    {
        [TitleGroup("Debug"), SerializeField, InlineButton("RaiseWithTestValue")]
        T testValue;

        public void RaiseWithTestValue() => Raise(testValue);

        [TitleGroup("Listener"), HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullElements")]
        public ReferencedCallbacks<T> listeners = new ReferencedCallbacks<T>();

        void RemoveNullElements() => listeners.RemoveAll(l => l.reference == null);

        public void AddListener(ReferencedEvent<UnityEvent<T>> rEvent) => listeners.Add(rEvent);

        public void RemoveListener(ReferencedEvent<UnityEvent<T>> rEvent) =>
            listeners.Remove(rEvent);

        public void Raise(T value)
        {
            if (logRaise)
                Debug.Log($"{this.TypeAndNameToString()} has been raise with <color=yellow>{value}</color>");

            listeners.Invoke(this, value, logListeners);
        }
    }
}