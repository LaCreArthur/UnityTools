using System;
using System.Text;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Toolbox.ScriptableObjects.Events
{
    public class ReferencedUltEvent<T> 
        where T : UltEventBase
    {
        [HideLabel]
        public readonly Object listener;
        [HideReferenceObjectPicker, ListDrawerSettings(Expanded = true)]
        public readonly T callbacks;

        public ReferencedUltEvent(Object listener, T callbacks)
        {
            this.listener = listener;
            this.callbacks = callbacks;
        }

        public void LogCallback(ScriptableObject so, object t)
        {
            string header = $"[<color=cyan>{so.name}</color>] callback from <b>{listener.name}</b> ";
            
            if (callbacks.PersistentCallsList != null)
            {
                foreach (var callback in callbacks.PersistentCallsList)
                {
                    LogMethodCall(t, callback, header);
                }
            }
            
            // if (callbacks.DynamicCallInvocationList != null)
            // {
            //     foreach (var callback in callbacks.DynamicCallInvocationList)
            //     {
            //         LogMethodCall(t, callback, header);
            //     }
            // }
        }

        void LogMethodCall(object t, PersistentCall callback, string header)
        {
            StringBuilder methodCall = new StringBuilder($" ~> {callback.Target.GetType()}.{callback.Method.Name}");
            if (callback.PersistentArguments.Length != 0)
            {
                // if the call has an argument but it's not of type t
                object argVal = callback.PersistentArguments[0].Value;
                string type = "";
                if (t == null || t.GetType() != argVal.GetType())
                {
                    methodCall.Append($"(<color=yellow>{argVal}</color>)");
                    type = argVal.GetType().ToString();
                    type = type.Substring(0,type.LastIndexOf(".") + 1);
                    methodCall = methodCall.Replace($"({type})", "");
                }
                else
                {
                    methodCall.Append($"(<color=yellow>{t}</color>)");
                    type = t.GetType().ToString();
                    type = type.Substring(0, type.LastIndexOf(".") + 1);
                    methodCall = methodCall.Replace($"({type})", "");
                }
            }
            // else methodCall.Append($"()");
            methodCall = methodCall.Replace("Void ", "");
            Debug.Log(methodCall.Insert(0, header), callback.Target);
        }

        void LogMethodCall(object t, Delegate callback, string header) =>
            LogMethodCall(t, new PersistentCall(callback), header);
    }
}