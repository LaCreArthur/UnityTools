using TMPro;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{

    [RequireComponent(typeof(TMP_Text))]
    public class FloatTMPTextListener : NumberTMPTextListener<float, FloatVar>
    {
        protected override void SetText()
        {
            float val = isValueOffset ? var.v + valueOffset :
                isValueMultiplied ? var.v * multiple : var.v;

            text.text = $"{prefix}{val.ToString($"N{decimals}")}{suffix}";
        }
    }

}