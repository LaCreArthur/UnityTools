using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Toolbox.ScriptableObjects.Variables;

namespace Toolbox.ScriptableObjects.Utils
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

        TMP_Text m_text;

        void Start()
        {
            m_text = GetComponent<TMP_Text>();
            if (autoUpdateOnChange) variable.onChange.Add(SetText, this);
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

        void OnDestroy()
        {
            if (autoUpdateOnChange) variable.onChange.Remove(SetText, this);
        }
    }
}