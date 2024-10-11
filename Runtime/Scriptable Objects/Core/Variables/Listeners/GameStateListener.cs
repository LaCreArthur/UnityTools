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
        public GameStateSO state;
        [HideReferenceObjectPicker]
        public UnityEvent onEnter, onExit;
    }

    public class GameStateListener : MonoBehaviour
    {
        [SerializeField]
        List<GameStateCallbacks> callbacks = new List<GameStateCallbacks>();

        bool _isRegistered;

        void OnEnable() => RegisterCallbacks();

        void RegisterCallbacks()
        {
            if (_isRegistered) return;
            _isRegistered = true;
            foreach (var callback in callbacks)
            {
                callback.state.AddOnEnter(callback.onEnter, this);
                callback.state.AddOnExit(callback.onExit, this);
            }
        }
    }
}
