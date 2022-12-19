using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.Modal_Window
{
    public interface IModalWindowProvider
    {
        public string Title();
        public BodyOrientation BodyOrientation();
        public string Content();
        public Sprite Sprite();
        public string AcceptBtnText();
        public string DeclineBtnText();
        public string AltBtnText();
        public UnityEvent OnAccept();
        public UnityEvent OnDecline();
        public UnityEvent OnAlt();
    }
}