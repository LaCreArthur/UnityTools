using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects.Variables;
using Toolbox.Utils;
using UnityEngine;
using UnityEngine.Events;
namespace Toolbox.ScriptableObjects.Events
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Events/Event", fileName = "E_")]
    public class EventSO : EventSOBase, IEventSO<UnityEvent>
    {
        [TitleGroup("Listener"), HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullElements")]
        public ReferencedCallbacks listeners = new ReferencedCallbacks();

        void RemoveNullElements() => listeners.RemoveAll(l => l.reference == null);

        public void AddListener(ReferencedEvent<UnityEvent> rEvent) => listeners.Add(rEvent);

        public void RemoveListener(ReferencedEvent<UnityEvent> rEvent) => listeners.Remove(rEvent);

        [TitleGroup("Debug"), Button]
        public void Raise()
        {
            if (logRaise)
                Debug.Log($"{this.TypeAndNameToString()} has been raised");

            listeners.Invoke(this, logListeners);
        }
    }
}