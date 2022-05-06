using System;
using Sirenix.OdinInspector;
using Toolbox.Utils;
using UltEvents;
using UnityEditor;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Variables
{
    public class VariableSOBase<T> : ScriptableObject, IStorable<T>
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
        
        bool _isInit;
        
        public T InitialValue
        {
            get => initialValue;
            set => initialValue = value;
        }
        public T PreviousValue => previousValue;
        
        public T v
        {
            get => value;
            set
            {
#if UNITY_EDITOR // dont reset value for nothing on GUI refresh
                if (EditorApplication.isPlayingOrWillChangePlaymode && this.value != null && this.value.Equals(value)) return;
#endif
                if (isConstant)
                {
#if UNITY_EDITOR // constant value can be set only in editor when not playing
                    if (!EditorApplication.isPlayingOrWillChangePlaymode)
                    {
                        previousValue = this.value;
                        this.value = ProcessValue(value);
                    }
#endif
                    if (Application.isPlaying && logOnChange)
                        Debug.Log($"{this.TypeAndNameToString()} is constant and cannot be modified", this);
                    return;
                }

                previousValue = this.value;
                this.value = ProcessValue(value);

                if (Application.isPlaying)
                {
                    if (isStored) Save();
                    OnChange(this.value);
                }
            }
        }

        protected virtual T ProcessValue(T oldVal) => oldVal;
        
        #endregion
        
        #region Debug
        [TitleGroup("Debug"), SerializeField, InlineButton("SetValue")]
        T newValue;
        protected virtual void SetValue(T newVal) => v = newVal;
        
        [TitleGroup("Debug"), HideIf("isConstant"), SerializeField] 
        protected bool logOnChange;
        #endregion
        
        #region OnChange
        [TitleGroup("On Change"), HideIf("isConstant"), SerializeField]
        bool logListeners;
        [TitleGroup("On Change"), HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullElements")]
        public OnChangeCallbacks<T> onChange = new OnChangeCallbacks<T>();
        void RemoveNullElements() => onChange?.RemoveAll(c => c.listener == null);
        
        void OnChange(T t)
        {
            if (logOnChange)
                Debug.Log(
                    $"{this.TypeAndNameToString()} has changed to <color=yellow>{t}</color>");

            foreach (var referencedUltEvent in onChange.Listeners)
            {
                if (logListeners) referencedUltEvent.LogCallback(this, t);
                
                foreach (PersistentCall persistentCall in referencedUltEvent.callbacks.PersistentCallsList)
                {
                    if (persistentCall.PersistentArguments != null && persistentCall.PersistentArguments.Length > 0)
                        persistentCall.SetArguments(t);
                }
                referencedUltEvent.callbacks.Invoke(t);
            }
        }

        #endregion
        
        protected virtual void OnEnable()
        {
#if UNITY_EDITOR // dont load if not on playmode
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
#endif
            if (!_isInit)
            {
                v = isStored ? Load() : initialValue;
                _isInit = true;
            }
        }

        void OnValidate() => OnEnable();

        public virtual void Save() {}

        public virtual T Load()
        {
            T t = default(T);
            return t;
        }
    }
}