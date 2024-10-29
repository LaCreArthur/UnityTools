using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    [AssetSelector] [CreateAssetMenu(menuName = "Scriptable Objects/Game State")]
    public class GameStateSO : ScriptableObject
    {
        public List<GameStateSO> validNextStates = new List<GameStateSO>();

        [FoldoutGroup("On Enter Listener")] public bool logOnEnterCallbacks;
        [FoldoutGroup("On Exit Listener")] public bool logOnExitCallbacks;

        [FoldoutGroup("On Enter Listener")] [HideLabel] [InlineProperty] [HideReferenceObjectPicker] [OnInspectorGUI("RemoveNullOnEnter")]
        public ReferencedCallbacks OnEnter { get; private set; } = new ReferencedCallbacks();

        [FoldoutGroup("On Exit Listener")] [HideLabel] [InlineProperty] [HideReferenceObjectPicker] [OnInspectorGUI("RemoveNullOnExit")]
        public ReferencedCallbacks OnExit { get; private set; } = new ReferencedCallbacks();

        void RemoveNullOnEnter() => OnEnter.RemoveAll(l => l.reference == null);
        void RemoveNullOnExit() => OnExit.RemoveAll(l => l.reference == null);


        public void Add(EventEnum eventEnum, Action callback)
        {
            switch (eventEnum)
            {
                case EventEnum.OnEnter:
                    AddOnEnter(callback);
                    break;
                case EventEnum.OnExit:
                    AddOnExit(callback);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddOnEnter(Action callback) => OnEnter?.Add(callback);
        public void AddOnEnter(Action callback, Object listener) => OnEnter?.Add(callback, listener);
        public void AddOnExit(Action callback) => OnExit?.Add(callback);
        public void AddOnExit(Action callback, Object listener) => OnExit?.Add(callback, listener);
        public void AddOnEnter(UnityEvent uEvent, Object listener) => OnEnter?.Add(uEvent, listener);
        public void AddOnExit(UnityEvent uEvent, Object listener) => OnExit?.Add(uEvent, listener);
        public void RemoveOnEnter(Action callback) => OnEnter?.Remove(callback);
        public void RemoveOnExit(Action callback) => OnExit?.Remove(callback);
        public void RemoveOnEnter(UnityEvent uEvent, Object listener) => OnEnter?.Remove(uEvent, listener);
        public void RemoveOnExit(UnityEvent uEvent, Object listener) => OnExit?.Remove(uEvent, listener);
        public void RaiseOnEnter() => OnEnter?.Invoke(this, logOnEnterCallbacks, true);
        public void RaiseOnExit() => OnExit?.Invoke(this, logOnExitCallbacks, false);
    }
}
