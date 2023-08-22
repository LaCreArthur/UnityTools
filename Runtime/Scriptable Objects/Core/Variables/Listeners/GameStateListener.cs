using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    [Serializable, InlineProperty, HideReferenceObjectPicker, HideLabel]
    internal class GameStateCallbacks
    {
        [HideReferenceObjectPicker]
        public UnityEvent onEnter, onExit;
    }

    public class GameStateListener : SerializedMonoBehaviour
    {
        [SerializeField, DictionaryDrawerSettings(KeyLabel = "State", DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        readonly Dictionary<GameStateSO, GameStateCallbacks> callbacks = new Dictionary<GameStateSO, GameStateCallbacks>();

        bool _isRegistered;

        void OnEnable() => RegisterCallbacks();

        void RegisterCallbacks()
        {
            if (_isRegistered) return;
            _isRegistered = true;
            foreach (KeyValuePair<GameStateSO, GameStateCallbacks> callback in callbacks)
            {
                callback.Key.AddOnEnter(callback.Value.onEnter, this);
                callback.Key.AddOnExit(callback.Value.onExit, this);
            }
        }
    }
}
