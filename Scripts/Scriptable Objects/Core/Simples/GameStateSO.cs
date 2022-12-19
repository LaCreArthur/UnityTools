using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Game State")]
    public class GameStateSO : ScriptableObject
    {
        public StateEnum stateEnum;
        public List<GameStateSO> validNextStates = new List<GameStateSO>();

        [TitleGroup("On Enter Listener")]
        public bool logOnEnterCallbacks;
        [HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullOnEnter")]
        ReferencedCallbacks onEnter = new ReferencedCallbacks();

        [TitleGroup("On Exit Listener")]
        public bool logOnExitCallbacks;
        [HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullOnExit")]
        ReferencedCallbacks onExit = new ReferencedCallbacks();

        void RemoveNullOnEnter() => onEnter.RemoveAll(l => l.reference == null);
        void RemoveNullOnExit() => onExit.RemoveAll(l => l.reference == null);


        public void Add(EventEnum eventEnum, Action callback, Object listener)
        {
            switch (eventEnum)
            {
                case EventEnum.OnEnter:
                    AddOnEnter(callback, listener);
                    break;
                case EventEnum.OnExit:
                    AddOnExit(callback, listener);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddOnEnter(Action callback, Object listener)
        {
            if (onEnter != null) onEnter.Add(callback, listener);
            else Debug.Log($"{name} onEnter is null, cannot add callback", this);
        }
        public void AddOnExit(Action callback, Object listener)
        {
            if (onExit != null) onExit.Add(callback, listener);
            else Debug.Log($"{name} onExit is null, cannot add callback", this);
        }
        public void AddOnEnter(UnityEvent uEvent, Object listener)
        {
            if (onEnter != null) onEnter.Add(uEvent, listener);
            else Debug.Log($"{name} onEnter is null, cannot add callback", this);
        }
        public void AddOnExit(UnityEvent uEvent, Object listener)
        {
            if (onExit != null) onExit.Add(uEvent, listener);
            else Debug.Log($"{name} onExit is null, cannot add callback", this);
        }
        public void RemoveOnEnter(Action callback, Object listener)
        {
            if (onEnter != null) onEnter.Remove(callback, listener);
            else Debug.Log($"{name} onEnter is null, cannot remove callback", this);
        }
        public void RemoveOnExit(Action callback, Object listener)
        {
            if (onExit != null) onExit.Remove(callback, listener);
            else Debug.Log($"{name} onExit is null, cannot remove callback", this);
        }
        public void RemoveOnEnter(UnityEvent uEvent, Object listener)
        {
            if (onEnter != null) onEnter.Remove(uEvent, listener);
            else Debug.Log($"{name} onEnter is null, cannot remove callback", this);
        }
        public void RemoveOnExit(UnityEvent uEvent, Object listener)
        {
            if (onExit != null) onExit.Remove(uEvent, listener);
            else Debug.Log($"{name} onExit is null, cannot remove callback", this);
        }
        public void RaiseOnEnter()
        {
            if (onEnter != null) onEnter.Invoke(this, logOnEnterCallbacks);
            else Debug.Log($"{name} onEnter is null", this);
        }
        public void RaiseOnExit()
        {
            if (onExit != null) onExit.Invoke(this, logOnEnterCallbacks);
            else Debug.Log($"{name} onExit is null", this);
        }
    }
}