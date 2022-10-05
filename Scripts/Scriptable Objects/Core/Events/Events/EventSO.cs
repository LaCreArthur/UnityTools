using System;
using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects.Variables;
using Toolbox.Utils;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Toolbox.ScriptableObjects.Events
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Events/Event", fileName = "E_")]
    public class EventSO : EventSOBase, IEventSO<UnityEvent>
    {
        [TitleGroup("Listener"), HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullElements")]
        public ReferencedCallbacks listeners = new ReferencedCallbacks();

        void RemoveNullElements() => listeners.RemoveAll(l => l.reference == null);

        public void Add(ReferencedEvent<UnityEvent> rEvent) => listeners.Add(rEvent);
        public void Add(UnityEvent uEvent, Object listener) => listeners.Add(new ReferencedEvent<UnityEvent>(uEvent, listener));
        public void Add(Action callback, Object listener) => listeners.Add(callback, listener);

        public void Remove(ReferencedEvent<UnityEvent> rEvent) => listeners.Remove(rEvent);
        public void Remove(UnityEvent uEvent, Object listener) => listeners.Remove(new ReferencedEvent<UnityEvent>(uEvent, listener));
        public void Remove(Action callback, Object listener) => listeners.Remove(callback, listener);

        [TitleGroup("Debug"), Button]
        public void Raise()
        {
            if (logRaise)
                Debug.Log($"{this.TypeAndNameToString()} has been raised");

            listeners.Invoke(this, logListeners);
        }
    }
}