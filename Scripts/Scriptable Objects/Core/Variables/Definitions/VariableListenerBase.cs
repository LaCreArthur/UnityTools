using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Variables
{
    [ExecuteAlways]
    public abstract class VariableListenerBase<T, TVariable> : MonoBehaviour where TVariable : VariableSOBase<T>
    {
        [SerializeField, AssetSelector, Required("A variable must be assigned for this listener!")] 
        protected TVariable variable;
        public UnityEvent<T> events;
    
        void Subscribe()
        {
            if (variable != null)
            {
                variable.onChange?.Add(new ReferencedUnityEvent<UnityEvent<T>>(this, events));
            }
        }

        void Unsubscribe()
        {
            if (variable != null)
            {
                variable.onChange?.Remove(new ReferencedUnityEvent<UnityEvent<T>>(this, events));
            }
        }

        protected virtual void OnEnable() => Subscribe();
        protected virtual void OnDisable() => Unsubscribe();
    }
}