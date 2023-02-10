using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

/*
 * Created by CreArthur - 2019
 * Class for updating a TMP_Text based on a IntVariable
 * Support prefix, suffix and value offset
 */

namespace AS.Toolbox.ScriptableObjects
{
    [RequireComponent(typeof(TMP_Text))]
    public class IntTMPTextListener : MonoBehaviour
    {
        public IntVar var;

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
            if (autoUpdateOnChange) var.onChange.Add(SetText, this);
            SetText();
        }

        public void SetText()
        {
            int val = isValueOffset ? var.v + valueOffset :
                isValueMultiplied ? var.v * multiple : var.v;

            m_text.text = $"{prefix}{val}{suffix}";
        }
    }
}