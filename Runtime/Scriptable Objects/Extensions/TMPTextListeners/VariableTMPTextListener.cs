using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    public class VariableTMPTextListener : SerializedMonoBehaviour
    {
        [SerializeField] string prefix;
        [SerializeField] string suffix;
        [SerializeField] bool autoUpdateOnChange = true;
        TMP_Text _tmp;
        [SerializeField] [Required] [AssetsOnly] ISOVariable variable;

        void Start()
        {
            _tmp = GetComponent<TMP_Text>();
            if (autoUpdateOnChange) variable.AddOnChange(SetText);
            SetText();
        }

        void OnDestroy()
        {
            if (autoUpdateOnChange) variable.RemoveOnChange(SetText);
        }

        void SetText() => _tmp.text = $"{prefix}{variable.ToString()}{suffix}";
    }
}
