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
        public void AddOnExit(Action callback) => OnExit?.Add(callback);
        public void AddOnEnter(UnityEvent uEvent, Object listener) => OnEnter?.Add(uEvent, listener);
        public void AddOnExit(UnityEvent uEvent, Object listener) => OnExit?.Add(uEvent, listener);
        public void RemoveOnEnter(Action callback) => OnEnter?.Remove(callback);
        public void RemoveOnExit(Action callback) => OnExit?.Remove(callback);

        public void RemoveAllCallbacks()
        {
            OnEnter?.RemoveAll();
            OnExit?.RemoveAll();
        }

        internal void RaiseOnEnter() => OnEnter?.Invoke(this, logOnEnterCallbacks, true);
        internal void RaiseOnExit() => OnExit?.Invoke(this, logOnExitCallbacks, false);
    }
}
