using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    public class BoolVarListener : VarListenerBase<bool, BoolVar>
    {
        [SerializeField] UnityEvent onTrue, onFalse;

        protected override void OnAwake() => variable?.AddOnChange(OnVariableChanged);
        protected override void OnOnDestroy() => variable?.RemoveOnChange(OnVariableChanged);

        void OnVariableChanged(bool value)
        {
            if (value)
                onTrue?.Invoke();
            else
                onFalse?.Invoke();
        }
    }
}
