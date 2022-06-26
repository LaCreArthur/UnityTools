using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Toolbox.ScriptableObjects.Events
{
    public class ReferencedEvent<T> where T : UnityEventBase
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
            string header = $"[<color=cyan>{so.name}</color>] callback from <b>{reference.name}</b> ";

            foreach (var c in UnityEventHelper.GetPersistentCalls("events", reference))
            {
                LogMethodCall(c, t, header);
            }
        }

        void LogMethodCall(PersistentCall callback, object t, string header)
        {
            StringBuilder methodCall = new StringBuilder($" ~> {callback.GetTarget().GetType()}.{callback.MethodName()}");
            // if dynamic params, the params is t
            if (callback.IsDynamicParams())
            {
                methodCall.Append(C($"{t} [∞]"));
                methodCall = methodCall.Replace($" ({t.GetType()})", "");
            }
            else
            {
                methodCall.Append(C(callback.GetParams()));
            }

            methodCall = methodCall.Replace("Void ", "");
            Debug.Log(methodCall.Insert(0, header), callback.GetTarget());
        }

        static string C(string s) => $"(<color=yellow>{s}</color>)";
    }
}