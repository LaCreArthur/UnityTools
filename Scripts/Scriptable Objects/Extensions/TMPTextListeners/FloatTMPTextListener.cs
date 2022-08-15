using Sirenix.OdinInspector;
using TMPro;
using Toolbox.ScriptableObjects.Variables;
using UnityEngine;
namespace Toolbox.ScriptableObjects.Utils
{
    [RequireComponent(typeof(TMP_Text))]
    public class FloatTMPTextListener : MonoBehaviour
    {
        public FloatVar var;
        public int decimals;
        public string prefix;
        public string suffix;
        public bool autoUpdateOnChange = true;

        public bool isValueOffset;
        [ShowIf("isValueOffset")] public float valueOffset;

        public bool isValueMultiplied;
        [ShowIf("isValueMultiplied")] public float multiple;

        TMP_Text m_text;

        void Start()
        {
            m_text = GetComponent<TMP_Text>();
            if (autoUpdateOnChange) var.onChange.Add(SetText, this);
            SetText();
        }

        public void SetText()
        {
            float val = isValueOffset ? var.v + valueOffset :
                isValueMultiplied ? var.v * multiple : var.v;

            m_text.text = $"{prefix}{val.ToString($"N{decimals}")}{suffix}";
        }
    }
}