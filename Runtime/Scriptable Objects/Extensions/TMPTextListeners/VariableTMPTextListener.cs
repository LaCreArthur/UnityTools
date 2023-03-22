using Sirenix.OdinInspector;
using TMPro;

namespace AS.Toolbox.ScriptableObjects
{
    public class VariableTMPTextListener : SerializedMonoBehaviour
    {
        [Required, AssetsOnly]
        public ISOVariable variable;
        public string prefix;
        public string suffix;
        public bool autoUpdateOnChange = true;

        TMP_Text _tmp;

        void Start()
        {
            _tmp = GetComponent<TMP_Text>();
            if (variable != null && autoUpdateOnChange)
                variable.AddOnChange(SetText, this);
            SetText();
        }

        public void SetText() => _tmp.text = $"{prefix}{variable.ToString()}{suffix}";
    }
}