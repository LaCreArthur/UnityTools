#region

using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

#endregion
namespace Toolbox.ScriptableObjects.Variables
{
    [Serializable, InlineProperty, HideReferenceObjectPicker, HideLabel]
    internal class GameStateCallbacks
    {
        [HideReferenceObjectPicker, HideLabel]
        public UnityEvent onEnter, onLeave;
    }

    public class GameStateListener : SerializedMonoBehaviour
    {
        [SerializeField, AssetSelector, Required("A variable must be assigned for this listener!"), InlineButton("New"), HideIf("_isCreating")]
        protected GameStateVariable gameState;

        [SerializeField, DictionaryDrawerSettings(KeyLabel = "State", DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        readonly Dictionary<GameStateSO, GameStateCallbacks> callbacks = new Dictionary<GameStateSO, GameStateCallbacks>();

        void OnEnable()
        {
            if (gameState != null)
            {
                gameState.onChange?.Add(OnChange, this);
            }
        }

        void OnChange()
        {
            if (callbacks.ContainsKey(gameState.PreviousValue))
                callbacks[gameState.PreviousValue].onLeave.Invoke();

            if (callbacks.ContainsKey(gameState.v))
                callbacks[gameState.v].onEnter.Invoke();
        }

        public GameStateVariable GetCurrentState() => gameState;

        #region Create new SO

#if UNITY_EDITOR
        [SerializeField, ShowIf("@variable==null"), InlineButton("Cancel"), InlineButton("Create"), LabelText("Create new SO, enter name:"),
         ShowIf("_isCreating")]
        string soName = typeof(GameStateVariable).ToString()[(typeof(GameStateVariable).ToString().LastIndexOf(".") + 1)..];

#pragma warning disable CS0414
        bool _isCreating;
#pragma warning restore CS0414
        protected void New() => _isCreating = true;

        protected void Cancel() => _isCreating = false;

        protected void Create(string soName)
        {
            GameStateVariable newSO = ScriptableObject.CreateInstance<GameStateVariable>();
            Directory.CreateDirectory(Application.dataPath + "/Scriptable Objects");

            // path has to start at "Assets"
            var path = "Assets/Scriptable Objects/" + soName + ".asset";
            AssetDatabase.CreateAsset(newSO, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();

            //Selection.activeObject = newSO;
            gameState = newSO;
            _isCreating = false;
        }
#endif

        #endregion
    }
}