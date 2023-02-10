#region

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

#endregion
namespace AS.Toolbox.ScriptableObjects
{
    [Serializable, InlineProperty, HideReferenceObjectPicker, HideLabel]
    internal class GameStateCallbacks
    {
        [HideReferenceObjectPicker]
        public UnityEvent onEnter, onLeave;
    }

    public class GameStateListener : SerializedMonoBehaviour
    {
        [SerializeField, Required("A variable must be assigned for this listener!")]
        protected GameStateVar gameState;

        [SerializeField, DictionaryDrawerSettings(KeyLabel = "State", DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        readonly Dictionary<GameStateSO, GameStateCallbacks> callbacks = new Dictionary<GameStateSO, GameStateCallbacks>();

        void OnEnable()
        {
            foreach (var callback in callbacks)
            {
                callback.Key.AddOnEnter(callback.Value.onEnter, this);
                callback.Key.AddOnExit(callback.Value.onLeave, this);
            }
        }
    }
}