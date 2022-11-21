using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Variables
{

    public enum StateEnum { Init, Home, Settings, InGame, GameOver }
    public enum EventEnum { OnEnter, OnExit }

    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Game State Variable")]
    public class GameStateVariable : VariableSOBase<GameStateSO>
    {
        [SerializeField]
        bool debugStateChange;

        public override void SetValue(GameStateSO newVal)
        {
            if (v == newVal)
            {
                Debug.LogWarning($"Trying to switch from and to the same state {newVal.name}");
                return;
            }

            if (v.validNextStates.Contains(newVal))
            {
                if (v != null) v.RaiseOnExit();
                v = newVal;
                v.RaiseOnEnter();
                if (debugStateChange)
                    Debug.Log($"Game state switch from {PreviousValue.name} to {newVal.name}");
            }
            else
            {
                Debug.LogWarning($"Game state try to switch from {v.name} to {newVal.name} which is not possible, state not changed !");
            }
        }

        [Button]
        public void SetValueForced(GameStateSO newVal) => v = newVal;
    }
}