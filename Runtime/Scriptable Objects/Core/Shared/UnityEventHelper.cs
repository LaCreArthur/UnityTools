#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    public static class UnityEventHelper
    {
        /// <summary>
        ///     Get all persistent calls for debug purposes.
        /// </summary>
        /// <param name="srcObj">the Unity Object on which is the UnityEvent</param>
        /// <returns></returns>
        public static List<PersistentCall> GetPersistentCalls(string srcEventName, Object srcObj)
        {
            var src = new SerializedObject(srcObj);
            SerializedProperty srcCalls = src.FindProperty($"{srcEventName}.m_PersistentCalls.m_Calls");
            var calls = new List<PersistentCall>();
            if (srcCalls == null)
                return calls;
            for (int srcIndex = 0; srcIndex < srcCalls.arraySize; srcIndex++)
            {
                SerializedProperty srcCallProperty = srcCalls.GetArrayElementAtIndex(srcIndex);
                var srcCall = new PersistentCall(srcCallProperty, "{srcEventName}");
                calls.Add(srcCall);
            }

            return calls;
        }
    }

    public readonly struct PersistentCall
    {
        readonly SerializedProperty _target;
        readonly SerializedProperty _methodName;
        readonly SerializedProperty _mode;
        readonly SerializedProperty _callState;
        readonly SerializedProperty _args;
        readonly SerializedProperty _objectArg;
        readonly SerializedProperty _intArg;
        readonly SerializedProperty _floatArg;
        readonly SerializedProperty _stringArg;
        readonly SerializedProperty _boolArg;

        internal PersistentCall(in SerializedProperty callProperty, in string propertyPathBase)
        {
            _target = callProperty?.FindPropertyRelative("m_Target");
            _methodName = callProperty?.FindPropertyRelative("m_MethodName");
            _mode = callProperty?.FindPropertyRelative("m_Mode");
            _callState = callProperty?.FindPropertyRelative("m_CallState");
            _args = callProperty?.FindPropertyRelative("m_Arguments");
            _objectArg = _args?.FindPropertyRelative("m_ObjectArgument");
            _intArg = _args?.FindPropertyRelative("m_IntArgument");
            _floatArg = _args?.FindPropertyRelative("m_FloatArgument");
            _stringArg = _args?.FindPropertyRelative("m_StringArgument");
            _boolArg = _args?.FindPropertyRelative("m_BoolArgument");
        }

        public override string ToString() =>
            $"[{(UnityEventCallState)_callState.enumValueIndex}] {_target.objectReferenceValue}.{_methodName.stringValue}({GetParamSignature(_mode.enumValueIndex)})";

        string GetParamSignature(in int enumIndex)
        {
            switch (enumIndex)
            {
                case 0:// Event Defined
                    return $"{_objectArg.objectReferenceValue} (dynamic call)";
                case 1:// void
                    return "";
                case 2:// Object
                    return $"{_objectArg.objectReferenceValue}";
                case 3:// int
                    return $"{_intArg.intValue}";
                case 4:// float
                    return $"{_floatArg.floatValue}";
                case 5:// string
                    return $"{_stringArg.stringValue}";
                case 6:// bool
                    return $"{_boolArg.boolValue}";
                default:
                    return string.Empty;
            }
        }

        public string MethodName() => _methodName.stringValue;

        public Object GetTarget() => _target.objectReferenceValue;

        public string GetParams() => GetParamSignature(_mode.enumValueIndex);

        public bool IsDynamicParams() => _mode.enumValueIndex == 0;
    }
}
#endif
