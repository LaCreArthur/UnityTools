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
            variable.onChange.Add(OnVariableChanged, this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            variable.onChange.Remove(OnVariableChanged, this);
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
