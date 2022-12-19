using TMPro;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    [RequireComponent(typeof(TMP_Text))]
    public class DoubleTMPTextListener : NumberTMPTextListener<double, DoubleVar>
    {
        protected override void SetText()
        {
            double val = isValueOffset ? var.v + valueOffset :
                isValueMultiplied ? var.v * multiple : var.v;

            text.text = $"{prefix}{val.ToString($"N{decimals}")}{suffix}";
        }
    }
}