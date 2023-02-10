using System;
using AS.Toolbox.Utils;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AS.Toolbox.ScriptableObjects
{
    [AssetSelector]
    public class SOVar<T> : ScriptableObject, ISOVariable, IStorable<T>
    {

        protected virtual void OnEnable()
        {
#if UNITY_EDITOR// dont load if not on playmode
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
                return;
#endif
            v = isStored ? Load() : initialValue;
        }

        void OnValidate() => OnEnable();

        public virtual void Save() {}

        public virtual T Load()
        {
            T t = default(T);
            return t;
        }

        public void AddOnChangeCallback(Action callback, Object listener) => onChange.Add(callback, listener);

        public void RemoveOnChangeCallback(Action callback, Object listener) => onChange.Remove(callback, listener);

        void OnDisable() => onChange.RemoveRuntimeEvents();

        public override string ToString() => value.ToString().Replace($"({value.GetType()})", "");

        #region Value

        [TitleGroup("Values"), SerializeField, ReadOnly, PropertyOrder(0)]
        protected T value;

        [TitleGroup("Values"), SerializeField, ReadOnly, PropertyOrder(0)]
        protected T previousValue;

        [TitleGroup("Values"), SerializeField, PropertyOrder(0)]
        protected T initialValue;

        [TitleGroup("Values"), SerializeField]
        protected bool isConstant;

        [TitleGroup("Values"), HideIf("isConstant"), SerializeField]
        bool isStored;

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
#if UNITY_EDITOR// dont reset value for nothing on GUI refresh
                if (EditorApplication.isPlayingOrWillChangePlaymode && this.value != null && this.value.Equals(value))
                    return;
#endif
                if (isConstant)
                {
#if UNITY_EDITOR// constant value can be set only in editor when not playing
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
                    if (isStored)
                        Save();
                    OnChange();
                }
            }
        }

        protected virtual T ProcessValue(T newVal) => newVal;

        #endregion

        #region Debug

        [TitleGroup("Debug"), SerializeField, Delayed, OnValueChanged("SetValue"), InlineButton("SetValue", "Set")]
        T newValue;

        // used for serialized unity event callback
        public virtual void SetValue(T newVal) => v = newVal;

        [TitleGroup("Debug"), HideIf("isConstant"), SerializeField]
        protected bool logOnChange;

        #endregion

        #region OnChange

        [FoldoutGroup("On Change"), HideIf("isConstant"), SerializeField]
        bool logListeners;

        [FoldoutGroup("On Change"), HideLabel, InlineProperty, HideReferenceObjectPicker, OnInspectorGUI("RemoveNullElements")]
        public ReferencedCallbacks<T> onChange = new ReferencedCallbacks<T>();

        void RemoveNullElements() => onChange?.RemoveAll(c => c.reference == null);

        protected void OnChange()
        {
            if (logOnChange)
                Debug.Log($"{this.TypeAndNameToString()} has changed to <color=yellow>{value}</color>");

            onChange.Invoke(this, value, logListeners);
        }

        #endregion

        #region Create Asset

        #if UNITY_EDITOR
        public static bool IsCreating;
        public static bool IsNotCreating => !IsCreating;
        public static void Create()
        {
            IsCreating = true;
        }

        public static void CreateAsset()
        {
            var asset = CreateInstance<SOVar<T>>();
            AssetDatabase.CreateAsset(asset, $"Assets/{typeof(T).Name}.asset");
            AssetDatabase.SaveAssets();
            IsCreating = false;
        }

        public static void CancelCreate()
        {
            IsCreating = false;
        }
        #endif

        #endregion
    }
}