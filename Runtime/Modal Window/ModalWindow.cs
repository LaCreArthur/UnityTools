using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AS.Toolbox.ModalWindow
{
    public class ModalWindow : MonoBehaviour
    {
        [Header("Mandatory")]
        public TextMeshProUGUI title;
        public TextMeshProUGUI acceptBtnTmp;
        [Header("Body")]
        public BodyOrientation bodyOrientation;
        public GameObject horizontalBodyGO;
        public GameObject verticalBodyGO;
        public TextMeshProUGUI content;
        public Image image;
        [Header("Extra Buttons")]
        public GameObject declineBtnGO;
        public TextMeshProUGUI declineBtnTmp;
        public GameObject altBtnGO;
        public TextMeshProUGUI altBtnTmp;

        bool _init;
        Action _onAccept, _onDecline, _onAlt;

        public static event Action OnClose;

        void Init() => _init = true;

        public void SetContent(string titleStr, string contentStr, Sprite sprite, string acceptBtnText, string declineBtnText,
            string altBtnText,
            BodyOrientation orientation, Action onAccept, Action onDecline, Action onAlt)
        {
            if (!_init) Init();
            SetMandatoryContent(titleStr, acceptBtnText, onAccept);
            SetBody(contentStr, sprite, orientation);
            SetDeclineButton(declineBtnText, onDecline);
            SetAltButton(altBtnText, onAlt);
        }

        void SetMandatoryContent(string titleStr, string acceptBtnText, Action onAccept)
        {
            title.text = titleStr;
            acceptBtnTmp.text = acceptBtnText;
            _onAccept = onAccept;
        }

        void SetBody(string contentStr = null, Sprite sprite = null, BodyOrientation orientation = BodyOrientation.Horizontal)
        {
            if (string.IsNullOrEmpty(contentStr) && sprite == null)
            {
                horizontalBodyGO.SetActive(false);
                verticalBodyGO.SetActive(false);
                return;
            }

            horizontalBodyGO.SetActive(orientation == BodyOrientation.Horizontal);
            verticalBodyGO.SetActive(orientation == BodyOrientation.Vertical);
            content.text = contentStr;
            image.sprite = sprite;
        }

        void SetDeclineButton(string declineBtnText = "Back", Action onDecline = null) =>
            SetExtraButton(ref declineBtnGO, ref declineBtnTmp, ref _onDecline, onDecline, declineBtnText);

        void SetAltButton(string altBtnText = "Other", Action onAlt = null) =>
            SetExtraButton(ref altBtnGO, ref altBtnTmp, ref _onAlt, onAlt, altBtnText);

        void SetExtraButton(ref GameObject btnGO, ref TextMeshProUGUI btnTMP, ref Action _action, Action action, string btnText)
        {
            btnGO.SetActive(action != null);
            btnTMP.text = btnText;
            _action = action;
        }

        public void OnAccept() => OnButton(ref _onAccept);
        public void OnDecline() => OnButton(ref _onDecline);
        public void OnAlt() => OnButton(ref _onAlt);

        void OnButton(ref Action action)
        {
            action?.Invoke();
            Close();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            OnClose?.Invoke();
        }
    }

    public enum BodyOrientation { Vertical, Horizontal }
}
