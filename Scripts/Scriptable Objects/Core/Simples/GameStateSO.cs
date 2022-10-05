using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Toolbox.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Game State")]
    public class GameStateSO : ScriptableObject
    {
        public List<GameStateSO> validNextStates = new List<GameStateSO>();
        [SerializeField, InlineButton("NewOnEnter"), HideIf("_onEnterCreating")]
        EventSO onEnter;
        [SerializeField, InlineButton("NewOnExit"), HideIf("_onExitCreating")]
        EventSO onExit;
        public void AddOnEnter(Action callback, Object listener)
        {
            if (onEnter != null) onEnter.Add(callback, listener);
            else Debug.Log($"{name} onEnter is null", this);
        }
        public void AddOnExit(Action callback, Object listener)
        {
            if (onExit != null) onExit.Add(callback, listener);
            else Debug.Log($"{name} onExit is null", this);
        }
        public void AddOnEnter(UnityEvent uEvent, Object listener)
        {
            if (onEnter != null) onEnter.Add(uEvent, listener);
            else Debug.Log($"{name} onEnter is null", this);
        }
        public void AddOnExit(UnityEvent uEvent, Object listener)
        {
            if (onExit != null) onExit.Add(uEvent, listener);
            else Debug.Log($"{name} onExit is null", this);
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
        [SerializeField, ShowIf("@onEnter==null"), InlineButton("CancelOnEnter"), InlineButton("CreateOnEnter"),
         LabelText("Create new SO, enter name:"),
         ShowIf("_onEnterCreating")]
        string onEnterName = "E_onEnter";
        [SerializeField, ShowIf("@onExit==null"), InlineButton("CancelOnExit"), InlineButton("CreateOnExit"),
         LabelText("Create new SO, enter name:"),
         ShowIf("_onExitCreating")]
        string onExitName = "E_onExit";

#pragma warning disable CS0414
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