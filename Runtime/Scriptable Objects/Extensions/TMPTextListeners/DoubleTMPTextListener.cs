using AS.Toolbox.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    [RequireComponent(typeof(TMP_Text))]
    public class DoubleTMPTextListener : NumberTMPTextListener<double, DoubleVar>
    {
        [SerializeField] bool formatAsCurrency;
        [SerializeField] bool isCurrency;
        [SerializeField] [ShowIf("isCurrency")] bool isDoge;

        protected override void SetText()
        {
            double val = isValueOffset ? var.v + valueOffset :
                isValueMultiplied ? var.v * multiple : var.v;

            text.text = $"{prefix}{(formatAsCurrency ? ((float)val).FormatCurrencyValue(true) : isCurrency ? val.ToCurrencyString(isDoge) : val.ToString($"N{decimals}"))}{suffix}";
        }
    }
}
