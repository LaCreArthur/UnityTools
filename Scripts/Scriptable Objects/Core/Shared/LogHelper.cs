using System.Text;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    public static class LogHelper
    {
        public static string HeaderStr(string soName, string refName) => $"[<color=cyan>{soName}</color>] callback from <b>{refName}</b> ";

        static StringBuilder MethodStr(string typeName, string methodName) => new StringBuilder($" ~> {typeName}.{methodName}");

        public static void LogMethodCall(object target, string header, string methodName, object t = null)
        {
            StringBuilder methodCall = MethodStr(target.GetType().Name, methodName);
            methodCall.Append(t != null ? LP(t).P() : "()");
            DebugLogMethodCall(methodCall, header, target as Object);
        }

        #if UNITY_EDITOR
        public static void LogMethodCall(PersistentCall p, string header, object t = null)
        {
            StringBuilder methodCall = MethodStr(p.GetTarget().GetType().Name, p.MethodName());
            methodCall.Append(p.IsDynamicParams() ? LP(t).P() : p.GetParams().P());
            DebugLogMethodCall(methodCall, header, p.GetTarget());
        }
        #endif

        static void DebugLogMethodCall(StringBuilder mc, string h, Object t) => Debug.Log(mc.Insert(0, h), t);

        static string LP(object p) => $"{p} [∞]";

        static string P(this string p) => $"(<color=yellow>{p}</color>)";
    }
}