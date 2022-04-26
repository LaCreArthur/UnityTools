using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityReusables.ScriptableObjects.Variables
{
    [Serializable]
    class GameStateCallbacks
    {
        public UnityEvent onEnter, onLeave;
    }

    public class GameStateListener : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<GameStateSO, GameStateCallbacks> _callbacks = new Dictionary<GameStateSO, GameStateCallbacks>();

        public GameStateVariable gameState;

        private void Start()
        {
            gameState.AddOnChangeCallback(OnChange);
        }

        void OnChange()
        {
            if (_callbacks.ContainsKey(gameState.PreviousValue))
                _callbacks[gameState.PreviousValue].onLeave.Invoke();

            if (_callbacks.ContainsKey(gameState.v)) _callbacks[gameState.v].onEnter.Invoke();
        }

        private void OnDestroy()
        {
            gameState.RemoveOnChangeCallback(OnChange);
        }

        public GameStateVariable GetCurrentState()
        {
            return gameState;
        }
    }
}