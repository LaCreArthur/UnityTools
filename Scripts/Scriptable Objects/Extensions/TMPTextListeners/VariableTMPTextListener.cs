using Sirenix.OdinInspector;
using TMPro;

namespace AS.Toolbox.ScriptableObjects
{
    public class VariableTMPTextListener : SerializedMonoBehaviour
    {
        [ValidateInput("@variable != null", "a variable must be provided"), AssetsOnly]
        public IVariableSO variable;
        public string prefix;
        public string suffix;
        public bool autoUpdateOnChange = true;

        TMP_Text _tmp;

        void Start()
        {
            _tmp = GetComponent<TMP_Text>();
            if (variable != null && autoUpdateOnChange)
                variable.AddOnChangeCallback(SetText, this);
            SetText();
        }

        public void SetText() => _tmp.text = $"{prefix}{variable.ToString()}{suffix}";
    }
}