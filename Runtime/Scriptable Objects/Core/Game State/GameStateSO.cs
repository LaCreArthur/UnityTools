using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    [AssetSelector]
    [CreateAssetMenu(menuName = "Scriptable Objects/Game State")]
    public class GameStateSO : ScriptableObject
    {
        public List<GameStateSO> validNextStates = new List<GameStateSO>();

        [FoldoutGroup("On Enter Listener")] public bool logOnEnterCallbacks;

        [FoldoutGroup("On Exit Listener")] public bool logOnExitCallbacks;
        [FoldoutGroup("On Enter Listener")]
        [HideLabel]
        [InlineProperty]
        [HideReferenceObjectPicker]
        [OnInspectorGUI("RemoveNullOnEnter")]
        public ReferencedCallbacks OnEnter { get; private set; } = new ReferencedCallbacks();
        [FoldoutGroup("On Exit Listener")]
        [HideLabel]
        [InlineProperty]
        [HideReferenceObjectPicker]
        [OnInspectorGUI("RemoveNullOnExit")]
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

        public void AddOnEnter(Action callback, bool dontAddDuplicate = false)
        {
            if (OnEnter != null) OnEnter.Add(callback, (Object)callback.Target, dontAddDuplicate);
            else Debug.Log($"{name} onEnter is null, cannot add callback", this);
        }
        public void AddOnExit(Action callback)
        {
            if (OnExit != null) OnExit.Add(callback, (Object)callback.Target);
            else Debug.Log($"{name} onExit is null, cannot add callback", this);
        }
        public void AddOnEnter(UnityEvent uEvent, Object listener)
        {
            if (OnEnter != null) OnEnter.Add(uEvent, listener);
            else Debug.Log($"{name} onEnter is null, cannot add callback", this);
        }
        public void AddOnExit(UnityEvent uEvent, Object listener)
        {
            if (OnExit != null) OnExit.Add(uEvent, listener);
            else Debug.Log($"{name} onExit is null, cannot add callback", this);
        }
        public void RemoveOnEnter(Action callback)
        {
            if (OnEnter != null) OnEnter.Remove(callback, (Object)callback.Target);
            else Debug.Log($"{name} onEnter is null, cannot remove callback", this);
        }
        public void RemoveOnExit(Action callback)
        {
            if (OnExit != null) OnExit.Remove(callback, (Object)callback.Target);
            else Debug.Log($"{name} onExit is null, cannot remove callback", this);
        }
        public void RemoveOnEnter(UnityEvent uEvent, Object listener)
        {
            if (OnEnter != null) OnEnter.Remove(uEvent, listener);
            else Debug.Log($"{name} onEnter is null, cannot remove callback", this);
        }
        public void RemoveOnExit(UnityEvent uEvent, Object listener)
        {
            if (OnExit != null) OnExit.Remove(uEvent, listener);
            else Debug.Log($"{name} onExit is null, cannot remove callback", this);
        }
        public void RaiseOnEnter()
        {
            if (OnEnter != null) OnEnter.Invoke(this, logOnEnterCallbacks);
            else Debug.Log($"{name} onEnter is null", this);
        }
        public void RaiseOnExit()
        {
            if (OnExit != null) OnExit.Invoke(this, logOnExitCallbacks);
            else Debug.Log($"{name} onExit is null", this);
        }
    }
}
