using UnityEngine;
using UnityEngine.Events;
namespace Toolbox.ScriptableObjects.Events
{
    public class ReferencedEvent<T> : ReferencedActionBase<T> where T : UnityEventBase
    {
        public ReferencedEvent(Object reference, T callbacks) : base(reference, callbacks) { }

        public void LogCallback(ScriptableObject so, object t)
        {
            string header = LogHelper.HeaderStr(so.name, reference.name);
            UnityEventHelper.GetPersistentCalls("events", reference).ForEach(c => LogHelper.LogMethodCall(c, header, t));
        }
    }
}