using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using Toolbox.ScriptableObjects.Variables;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Utils
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class VariableTMPTextListener : SerializedMonoBehaviour
    {
        [ValidateInput("@variable != null", "a variable must be provided"), AssetsOnly, AssetSelector]
        public IVariableSO variable;
        public string prefix;
        public string suffix;
        public bool autoUpdateOnChange = true;

        TextMeshProUGUI _tmp;

        void Start()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            if (variable != null && autoUpdateOnChange) variable.AddOnChangeCallback(SetText, this);
            SetText();
        }

        public void SetText() => _tmp.text = $"{prefix}{variable.ToString()}{suffix}";

        void OnDestroy()
        {
            if (variable != null && autoUpdateOnChange) variable.RemoveOnChangeCallback(SetText, this);
        }
    }
}