using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.ScriptableObjects
{
    [RequireComponent(typeof(TMP_Text))]
    public abstract class NumberTMPTextListener<T, TVar> : MonoBehaviour where TVar : SOVar<T>
    {
        [SerializeField] protected TVar var;
        [SerializeField] protected int decimals;
        [SerializeField] protected string prefix;
        [SerializeField] protected string suffix;
        [SerializeField] protected bool autoUpdateOnChange = true;

        [SerializeField] protected bool isValueOffset;
        [ShowIf("isValueOffset")] [SerializeField] protected float valueOffset;

        [SerializeField] protected bool isValueMultiplied;
        [ShowIf("isValueMultiplied")] [SerializeField] protected float multiple;
        [SerializeField] UnityEvent onValueChanged;

        protected TMP_Text text;
        void Start()
        {
            text = GetComponent<TMP_Text>();
            if (autoUpdateOnChange) var.AddOnChange(SetTextInternal);
            SetTextInternal();
        }

        void OnDestroy()
        {
            if (autoUpdateOnChange) var.RemoveOnChange(SetTextInternal);
        }

        void SetTextInternal()
        {
            SetText();
            onValueChanged?.Invoke();
        }

        protected virtual void SetText() {}
    }
}
