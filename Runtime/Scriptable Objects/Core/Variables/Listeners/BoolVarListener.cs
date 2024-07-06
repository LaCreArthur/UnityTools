using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    public class BoolVarListener : VarListenerBase<bool, BoolVar>
    {
        [SerializeField] UnityEvent onTrue, onFalse;

        protected override void OnEnable()
        {
            base.OnEnable();
            variable.AddOnChange(OnVariableChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            variable.RemoveOnChange(OnVariableChanged);
        }

        void OnVariableChanged(bool value)
        {
            if (value)
                onTrue?.Invoke();
            else
                onFalse?.Invoke();
        }
    }
}
