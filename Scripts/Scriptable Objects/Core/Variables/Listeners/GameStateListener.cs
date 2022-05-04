using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Variables
{
    [Serializable]
    internal class GameStateCallbacks
    {
        public UnityEvent onEnter, onLeave;
    }

    public class GameStateListener : MonoBehaviour
    {
        [SerializeField]
        Dictionary<GameStateSO, GameStateCallbacks> _callbacks = new Dictionary<GameStateSO, GameStateCallbacks>();

        public GameStateVariable gameState;

        void Start() => gameState.onChange.Add(OnChange, this);

        void OnChange()
        {
            if (_callbacks.ContainsKey(gameState.PreviousValue))
                _callbacks[gameState.PreviousValue].onLeave.Invoke();

            if (_callbacks.ContainsKey(gameState.v)) _callbacks[gameState.v].onEnter.Invoke();
        }

        void OnDestroy() => gameState.onChange.Remove(OnChange, this);

        public GameStateVariable GetCurrentState() => gameState;
    }
}