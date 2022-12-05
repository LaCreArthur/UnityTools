using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects.Events;
using Toolbox.ScriptableObjects.Variables;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Game State")]
    public class GameStateSO : ScriptableObject
    {
        public StateEnum stateEnum;
        public List<GameStateSO> validNextStates = new List<GameStateSO>();
        [SerializeField, InlineButton("NewOnEnter"), HideIf("_onEnterCreating"), Required]
        EventSO onEnter;
        [SerializeField, InlineButton("NewOnExit"), HideIf("_onExitCreating"), Required]
        EventSO onExit;

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
            if (onEnter != null) onEnter.Raise();
            else Debug.Log($"{name} onEnter is null", this);
        }
        public void RaiseOnExit()
        {
            if (onExit != null) onExit.Raise();
            else Debug.Log($"{name} onExit is null", this);
        }


         #region Create new SO

#if UNITY_EDITOR
#pragma warning disable CS0414
        [SerializeField, ShowIf("@onEnter==null"), InlineButton("CancelOnEnter"), InlineButton("CreateOnEnter"),
         LabelText("Create new SO, enter name:"),
         ShowIf("_onEnterCreating")]
        string onEnterName = "E_onEnter";
        [SerializeField, ShowIf("@onExit==null"), InlineButton("CancelOnExit"), InlineButton("CreateOnExit"),
         LabelText("Create new SO, enter name:"),
         ShowIf("_onExitCreating")]
        string onExitName = "E_onExit";

        bool _onEnterCreating;
        bool _onExitCreating;
#pragma warning restore CS0414
        void NewOnEnter() => _onEnterCreating = true;
        void NewOnExit() => _onExitCreating = true;

        void CancelOnEnter() => _onEnterCreating = false;
        void CancelOnExit() => _onExitCreating = false;

        void CreateOnEnter(string soName)
        {
            onEnter = CreateSO(soName);
            _onEnterCreating = false;
        }
        void CreateOnExit(string soName)
        {
            onExit = CreateSO(soName);
            _onExitCreating = false;
        }

        EventSO CreateSO(string soName)
        {
            EventSO newSO = CreateInstance<EventSO>();
            Directory.CreateDirectory(Application.dataPath + "/Scriptable Objects");

            // path has to start at "Assets"
            var path = "Assets/Scriptable Objects/" + soName + ".asset";
            AssetDatabase.CreateAsset(newSO, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            //Selection.activeObject = newSO;
            return newSO;
        }
#endif

        #endregion
    }
}