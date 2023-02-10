using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.ScriptableObjects
{
    [AssetSelector]
    public abstract class SOEventBase : ScriptableObject
    {
        [TitleGroup("Debug"), SerializeField]
        protected bool logRaise;

        [TitleGroup("Listener"), SerializeField]
        protected bool logListeners;
    }
}