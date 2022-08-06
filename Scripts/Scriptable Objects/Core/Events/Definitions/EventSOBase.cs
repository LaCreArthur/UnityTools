using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace Toolbox.ScriptableObjects.Events
{
    public abstract class EventSOBase<T> : ScriptableObject, IEventSO<T> where T : UnityEventBase
    {
        [TitleGroup("Debug"), SerializeField]
        protected bool logRaise;

        [TitleGroup("Listeners"), SerializeField]
        protected bool logListeners;

        [TitleGroup("Listeners"), SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, Expanded = true),
         OnInspectorGUI("RemoveNullElements")]
        protected List<ReferencedEvent<T>> listeners = new List<ReferencedEvent<T>>();

        public void AddListener(ReferencedEvent<T> uEvent)
        {
            if (listeners == null)
                listeners = new List<ReferencedEvent<T>>();
            listeners.Add(uEvent);
        }

        public void RemoveListener(ReferencedEvent<T> uEvent)
        {
            if (listeners == null)
                return;
            var listener = listeners.Find(l => l.reference == uEvent.reference && Equals(l.callbacks, uEvent.callbacks));
            listeners.Remove(listener);
        }

        void RemoveNullElements() => listeners.RemoveAll(l => l.reference == null);
    }
}