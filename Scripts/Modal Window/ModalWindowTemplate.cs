using UnityEngine;

namespace AS.Toolbox.Modal_Window
{
    [CreateAssetMenu(menuName = "Modal Window Template", fileName = "ModalWindowTemplate", order = 0)]
    public class ModalWindowTemplate : ScriptableObject
    {
        public ModalWindowProvider provider;
    }
}