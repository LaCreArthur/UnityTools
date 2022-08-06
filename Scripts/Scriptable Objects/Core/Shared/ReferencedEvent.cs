using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace Toolbox.ScriptableObjects.Events
{
    public readonly struct ReferencedEvent<T> where T : UnityEventBase
    {
        [HideLabel]
        public readonly Object reference;

        [HideReferenceObjectPicker, ListDrawerSettings(Expanded = true)]
        public readonly T callbacks;

        public ReferencedEvent(Object reference, T callbacks)
        {
            this.reference = reference;
            this.callbacks = callbacks;
        }

        public void LogCallback(ScriptableObject so, object t)
        {
            string header = LogHelper.HeaderStr(so.name, reference.name);
            UnityEventHelper.GetPersistentCalls("events", reference).ForEach(c => LogHelper.LogMethodCall(c, header, t));
        }
    }
}