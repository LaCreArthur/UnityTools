using AS.Toolbox.Utils;
using TMPro;
using UnityEngine;

namespace AS.Toolbox.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMPRandomTextAndColor : MonoBehaviour
    {
        public bool onEnable;
        public string[] queries;
        public Color[] colors;

        TMP_Text _text;

        void Awake()
        {
            _text = GetComponent<TMP_Text>();
            SetRandomTextAndColor();
        }

        void OnEnable()
        {
            if (onEnable) SetRandomTextAndColor();
        }

        public void SetRandomTextAndColor()
        {
            _text.text = queries.GetRandom();
            _text.color = colors.GetRandom();
        }
    }
}
