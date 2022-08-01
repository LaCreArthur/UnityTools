using TMPro;
using Toolbox.Utils;
using UnityEngine;

namespace UnityReusables.Utils.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMPRandomTextAndColor : MonoBehaviour
    {
        public string[] queries;
        public Color[] colors;

        TMP_Text text;

        void Awake()
        {
            text = GetComponent<TMP_Text>();
            SetRandomTextAndColor();
        }

        public void SetRandomTextAndColor()
        {
            text.text = queries.GetRandom();
            text.color = colors.GetRandom();
        }
    }
}