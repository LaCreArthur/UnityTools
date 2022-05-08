using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Toolbox.ScriptableObjects.Variables;

/*
 * Created by CreArthur - 2019
 * Class for updating a TMP_Text based on a IntVariable
 * Support prefix, suffix and value offset
 */

namespace Toolbox.ScriptableObjects.Utils
{
    [RequireComponent(typeof(TMP_Text))]
    public class IntTMPTextListener : MonoBehaviour
    {
        public IntVariable variable;

        public string prefix;
        public string suffix;
        public bool autoUpdateOnChange = true;

        public bool isValueOffset;
        [ShowIf("isValueOffset")] public int valueOffset;
        
        public bool isValueMultiplied;
        [ShowIf("isValueMultiplied")] public int multiple;

        TMP_Text m_text;

        void Start()
        {
            m_text = GetComponent<TMP_Text>();
            if (autoUpdateOnChange) variable.onChange.Add(SetText, this);
            SetText();
        }

        public void SetText()
        {
            int val = 
                isValueOffset ? 
                    variable.v + valueOffset : 
                isValueMultiplied ? 
                    variable.v * multiple : 
                variable.v;
            
            m_text.text = $"{prefix}{val}{suffix}";
        }

        void OnDestroy()
        {
            if (autoUpdateOnChange) variable.onChange.Remove(SetText, this);
        }
    }
}