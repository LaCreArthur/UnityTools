using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    public class ReferencedEvent<T> : ReferencedActionBase<T> where T : UnityEventBase
    {
        public ReferencedEvent(T callbacks, Object reference) : base(callbacks, reference) { }

        public void LogCallback(ScriptableObject so, object t)
        {
            string header = LogHelper.HeaderStr(so.name, reference.name);
            #if UNITY_EDITOR
            UnityEventHelper.GetPersistentCalls("events", reference).ForEach(c => LogHelper.LogMethodCall(c, header, t));
            #endif
        }
    }
}