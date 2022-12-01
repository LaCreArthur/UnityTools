using Sirenix.OdinInspector;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Events
{
    [AssetSelector]
    public abstract class EventSOBase : ScriptableObject
    {
        [TitleGroup("Debug"), SerializeField]
        protected bool logRaise;

        [TitleGroup("Listener"), SerializeField]
        protected bool logListeners;
    }
}