using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityReusables.ScriptableObjects.Variables;

namespace UnityReusables.ScriptableObjects.Utils
{
    [RequireComponent(typeof(TMP_Text))]
    public class FloatTMPTextListener : MonoBehaviour
    {
        public FloatVariable variable;
        public int decimals;
        public string prefix;
        public string suffix;
        public bool autoUpdateOnChange = true;

        public bool isValueOffset;
        [ShowIf("isValueOffset")] public float valueOffset;
        
        public bool isValueMultiplied;
        [ShowIf("isValueMultiplied")] public float multiple;

        private TMP_Text m_text;

        private void Start()
        {
            m_text = GetComponent<TMP_Text>();
            if (autoUpdateOnChange) variable.AddOnChangeCallback(SetText);
            SetText();
        }

        public void SetText()
        {
            float val = 
                isValueOffset ? 
                    variable.v + valueOffset : 
                    isValueMultiplied ? 
                        variable.v * multiple : 
                        variable.v;
            
            m_text.text = $"{prefix}{val.ToString($"N{decimals}")}{suffix}";
        }

        private void OnDestroy()
        {
            if (autoUpdateOnChange) variable.RemoveOnChangeCallback(SetText);
        }
    }
}