using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;

namespace Toolbox.ScriptableObjects.Events
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Events/Event", fileName = "E_")]
    public class EventSO : EventSOBase<UltEvent>
    {
        [TitleGroup("Debug"), Button]
        public void Raise()
        {
            if (logRaise) Debug.Log(
                $"{GetType().ToString().Replace("Toolbox.ScriptableObjects.Events.", "")} [<color=cyan>{name}</color>] has been raised");
            
            listeners.ForEach(l =>
            {
                if (logListeners) l.LogCallback(this, null);
                l.callbacks?.InvokeSafe();
            });
        }
    }
}