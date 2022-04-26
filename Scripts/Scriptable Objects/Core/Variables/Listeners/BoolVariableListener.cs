using UnityEngine;
using UnityEngine.Events;

namespace UnityReusables.ScriptableObjects.Variables
{
    public class BoolVariableListener : MonoBehaviour
    {
        public bool logOnChange;
        public bool callOnStart;
        public BoolVariable variable;
        public UnityEvent onTrue;
        public UnityEvent onFalse;

        void OnEnable() => variable.AddOnChangeCallback(OnChange);
        void OnDisable() => variable.RemoveOnChangeCallback(OnChange);

        void Start() { if (callOnStart) OnChange(); }

        void OnChange()
        {
            UnityEvent events = variable.v ? onTrue : onFalse;
            if (logOnChange) Debug.Log(variable.v ? "onTrue Called" : "onFalse called");
            events.Invoke();
        }
    }
}