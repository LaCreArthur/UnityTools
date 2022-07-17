using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ModalWindowProvider : IModalWindowProvider
{
    [Header("Mandatory")]
    public string title;
    public string acceptBtnText;
    public UnityEvent onAccept;
    [Header("Body")]
    public BodyOrientation bodyOrientation;
    public string content;
    public Sprite sprite;
    [Header("Extra Buttons")]
    public string declineBtnText;
    public UnityEvent onDecline;
    public string altBtnText;
    public UnityEvent onAlt;
    public string Title() => title;
    public BodyOrientation BodyOrientation() => bodyOrientation;
    public string Content() => content;
    public Sprite Sprite() => sprite;
    public string AcceptBtnText() => acceptBtnText;
    public string DeclineBtnText() => declineBtnText;
    public string AltBtnText() => altBtnText;
    public UnityEvent OnAccept() => onAccept;
    public UnityEvent OnDecline() => onDecline;
    public UnityEvent OnAlt() => onAlt;
}