using NaughtyAttributes;
using UnityEngine;

namespace UnityReusables.ScriptableObjects.Variables
{
    public class BaseVariable<T> : RegistrableScriptableObject, IStorable<T>
    {
        [Header("Values")]
        [SerializeField] protected T initialValue;
        [SerializeField] [ReadOnly] protected T previousValue;
        [SerializeField] protected T value;
        [Header("Changes")]
        [SerializeField] protected bool isConstant;
        [HideIf("isConstant")]
        [SerializeField] protected bool debugChange;
        [HideIf("isConstant")]
        [SerializeField] private bool isPlayerPref = false;
        
        public T InitialValue
        {
            get => initialValue;
            set => initialValue = value;
        }
        
        public T v
        {
            get => value;
            set
            {
                #if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    if (this.value.Equals(value)) return;
                }
                #endif
                if (isConstant)
                {
                    #if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        previousValue = this.value;
                        this.value = value;
                    }
                    #endif

                    if (debugChange) Debug.Log($"{name} is constant and cannot be modified", this);
                    return;
                }

                previousValue = this.value;
                this.value = value;
                
                if (Application.isPlaying)
                {    
                    if (debugChange) Debug.Log($"{name} is set to : {value}", this);
                    if (isPlayerPref) Save();

                    TriggerChange();
                }
            }
        }

        public T PreviousValue => previousValue;

        protected override void OnEnable() => v = isPlayerPref ? Load() : initialValue;

        void OnValidate() => OnEnable();
        
        public virtual void SetValue(T newVal)
        {
            v = newVal;
#if UNITY_EDITOR
            if (Application.isPlaying) TriggerChange();
#endif
        }

        public virtual void Save() {}

        public virtual T Load()
        {
            T t = default(T);
            return t;
        }
    }
}