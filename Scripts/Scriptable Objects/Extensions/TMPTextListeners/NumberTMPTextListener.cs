using Sirenix.OdinInspector;
using TMPro;
using Toolbox.ScriptableObjects.Variables;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Utils
{
    [RequireComponent(typeof(TMP_Text))]
    public abstract class NumberTMPTextListener<T, TVar> : MonoBehaviour where TVar : VariableSOBase<T>
    {
        public TVar var;
        public int decimals;
        public string prefix;
        public string suffix;
        public bool autoUpdateOnChange = true;

        public bool isValueOffset;
        [ShowIf("isValueOffset")] public float valueOffset;

        public bool isValueMultiplied;
        [ShowIf("isValueMultiplied")] public float multiple;

        protected TMP_Text text;

        void Start()
        {
            text = GetComponent<TMP_Text>();
            if (autoUpdateOnChange) var.onChange.Add(SetText, this);
            SetText();
        }

        protected virtual void SetText() {}
    }
}