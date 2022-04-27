using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Events
{
    public abstract class EventSOBase<T> : ScriptableObject, IEventSO<T> 
    {
        [TitleGroup("Debug"), SerializeField] 
        protected bool logRaise;
        [TitleGroup("Debug"), SerializeField] 
        protected bool logListeners;
    
        [TitleGroup("Listeners"), SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, Expanded = true), OnInspectorGUI("RemoveNullElements")]  
        protected List<ReferencedUltEvent<T>> listeners = new List<ReferencedUltEvent<T>>();


        public void AddListener(ReferencedUltEvent<T> ultEvent)
        {
            if (listeners == null) listeners = new List<ReferencedUltEvent<T>>();
            listeners.Add(ultEvent);
        }

        public void RemoveListener(ReferencedUltEvent<T> ultEvent)
        {
            if (listeners == null) return;
            var listener = listeners.Find(l => l.listener == ultEvent.listener && Equals(l.callbacks, ultEvent.callbacks));
            listeners.Remove(listener);
        }
    
        void RemoveNullElements() => listeners.RemoveAll(l => l.listener == null);
    }
}