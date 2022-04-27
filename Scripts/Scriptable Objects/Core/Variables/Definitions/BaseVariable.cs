using Sirenix.OdinInspector;
using UnityEngine;

namespace UnityReusables.ScriptableObjects.Variables
{
    public class BaseVariable<T> : RegistrableScriptableObject, IStorable<T>
    {
        #region Value
        
        [TitleGroup("Values"), SerializeField, PropertyOrder(0)]
        protected T initialValue;
        [TitleGroup("Values"), SerializeField, ReadOnly, PropertyOrder(0)]
        protected T previousValue;
        [TitleGroup("Values"), SerializeField, ReadOnly, PropertyOrder(0)]
        protected T value;
        [SerializeField] 
        protected bool isConstant;
        [HideIf("isConstant"), SerializeField] 
        bool isStored;
        
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

                    if (logOnChange) Debug.Log($"{name} is constant and cannot be modified", this);
                    return;
                }

                previousValue = this.value;
                this.value = value;
                
                if (Application.isPlaying)
                {    
                    if (logOnChange) Debug.Log($"{name} is set to : {value}", this);
                    if (isStored) Save();

                    TriggerChange();
                }
            }
        }

        public T PreviousValue => previousValue;
        #endregion
        
        #region Debug
        [TitleGroup("Debug"), SerializeField, InlineButton("SetValue")]
        private T newValue;
        protected virtual void SetValue(T newVal) => v = newVal;
        
        [TitleGroup("Debug"), HideIf("isConstant"), SerializeField] 
        protected bool logOnChange;
        #endregion


        protected override void OnEnable() => v = isStored ? Load() : initialValue;

        void OnValidate() => OnEnable();

        public virtual void Save() {}

        public virtual T Load()
        {
            T t = default(T);
            return t;
        }
    }
}