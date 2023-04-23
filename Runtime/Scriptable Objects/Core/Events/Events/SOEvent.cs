using System;
using AS.Toolbox.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Events/Event", fileName = "E_")]
    public class SOEvent : SOEventBase, ISOEvent<UnityEvent>
    {
        [TitleGroup("Listener"), HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullElements")]
        public ReferencedCallbacks listeners = new ReferencedCallbacks();

        public void Add(ReferencedEvent<UnityEvent> rEvent) => listeners.Add(rEvent);

        public void Remove(ReferencedEvent<UnityEvent> rEvent) => listeners.Remove(rEvent);

        void RemoveNullElements() => listeners.RemoveAll(l => l.reference == null);
        public void Add(Action callback) => listeners.Add(callback, (Object)callback.Target);
        public void Remove(Action callback) => listeners.Remove(callback, (Object)callback.Target);

        [TitleGroup("Debug"), Button]
        public void Raise()
        {
            if (logRaise)
                Debug.Log($"{this.TypeAndNameToString()} has been raised");

            listeners.Invoke(this, logListeners);
        }
    }
}
