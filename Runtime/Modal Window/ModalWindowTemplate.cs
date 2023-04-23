using UnityEngine;

namespace AS.Toolbox.ModalWindow
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Modal Window Template", fileName = "ModalWindowTemplate", order = 0)]
    public class ModalWindowTemplate : ScriptableObject
    {
        public ModalWindowProvider provider;
    }
}
