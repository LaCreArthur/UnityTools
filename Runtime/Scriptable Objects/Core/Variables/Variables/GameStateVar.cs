using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    public enum EventEnum { OnEnter, OnExit }

    [CreateAssetMenu(menuName = "Scriptable Objects/Variables/Game State Variable")]
    public class GameStateVar : SOVar<GameStateSO>
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
                FinalizeSetValue(newVal);
            else
                Debug.LogWarning($"Game state try to switch from {v.name} to {newVal.name} which is not possible, state not changed !");
        }

        [Button("Force SetValue")]
        public void FinalizeSetValue(GameStateSO newVal)
        {
            if (v != null) v.RaiseOnExit();
            v = newVal;
            v.RaiseOnEnter();
        }
    }
}
