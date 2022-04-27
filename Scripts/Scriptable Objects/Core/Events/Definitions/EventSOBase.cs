using System.Collections.Generic;
using Sirenix.OdinInspector;
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
        protected List<ReferencedUnityEvent<T>> listeners = new List<ReferencedUnityEvent<T>>();


        public void AddListener(ReferencedUnityEvent<T> unityEvent)
        {
            if (listeners == null) listeners = new List<ReferencedUnityEvent<T>>();
            listeners.Add(unityEvent);
        }

        public void RemoveListener(ReferencedUnityEvent<T> unityEvent)
        {
            if (listeners == null) return;
            var listener = listeners.Find(l => l.listener == unityEvent.listener && Equals(l.callbacks, unityEvent.callbacks));
            listeners.Remove(listener);
        }
    
        void RemoveNullElements() => listeners.RemoveAll(l => l.listener == null);
    }
}