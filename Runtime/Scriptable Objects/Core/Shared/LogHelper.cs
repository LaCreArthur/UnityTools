using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    public static class LogHelper
    {
        public static string HeaderStr(string soName, [CanBeNull] string refName, bool? onEnter = false) =>
            $"[<color=#00FFFF>{soName}</color>] {(onEnter != null ? onEnter.Value ? "OnEnter" : "OnExit" : "")} callback from <b>{refName ?? "?"}</b> ";

        static StringBuilder MethodStr([CanBeNull] string typeName, string methodName) => new StringBuilder($" ~> {typeName ?? "?"}.{methodName}");

        public static void LogMethodCall([CanBeNull] object target, string header, string methodName, object t = null)
        {
            StringBuilder methodCall = MethodStr(target?.GetType().Name, methodName);
            methodCall.Append(t != null ? LP(t).P() : "()");
            DebugLogMethodCall(methodCall, header, target as Object);
        }

        #if UNITY_EDITOR
        public static void EditorLogMethodCall(EditorPersistentCall p, string header, object t = null)
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
