using System.Collections.Generic;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    public abstract class EventSOBase<T> : ScriptableObject, IEventSO<T> where T : UnityEventBase
    //where T : UltEventBase
    {
        [TitleGroup("Debug"), SerializeField] 
        protected bool logRaise;
        
        [TitleGroup("Listeners"), SerializeField] 
        protected bool logListeners;
    
        [TitleGroup("Listeners"), SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, Expanded = true), OnInspectorGUI("RemoveNullElements")]  
        protected List<ReferencedUnityEvent<T>> unityEventListeners = new List<ReferencedUnityEvent<T>>();
        
        [TitleGroup("Listeners"), SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, Expanded = true), OnInspectorGUI("RemoveNullElements")]  
        protected List<ReferencedUltEvent<UltEvent>> ultEventListeners = new List<ReferencedUltEvent<UltEvent>>();

        public void AddListener(ReferencedUnityEvent<T> unityEvent, ReferencedUltEvent<UltEvent> ultEvent)
        {
            if (unityEventListeners == null) unityEventListeners = new List<ReferencedUnityEvent<T>>();
            unityEventListeners.Add(unityEvent);
            if (ultEventListeners == null) ultEventListeners = new List<ReferencedUltEvent<UltEvent>>();
            ultEventListeners.Add(ultEvent);
        }

        public void RemoveListener(ReferencedUnityEvent<T> unityEvent, ReferencedUltEvent<UltEvent> ultEvent)
        {
            if (unityEventListeners == null) return;
            var listener = unityEventListeners.Find(l => l.listener == unityEvent.listener && Equals(l.callbacks, unityEvent.callbacks));
            unityEventListeners.Remove(listener);
            
            if (ultEventListeners == null) return;
            var ultListener = ultEventListeners.Find(l => l.listener == ultEvent.listener && Equals(l.callbacks, ultEvent.callbacks));
            ultEventListeners.Remove(ultListener);
        }
    
        void RemoveNullElements() => unityEventListeners.RemoveAll(l => l.listener == null);
    }
}