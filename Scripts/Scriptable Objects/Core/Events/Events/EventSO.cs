using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    [CreateAssetMenu(menuName = "Events/Event", fileName = "E_")]
    public class EventSO : EventSOBase<UnityEvent>
    {
        [TitleGroup("Debug"), Button]
        public void Raise()
        {
            if (logRaise) Debug.Log(
                $"{GetType().ToString().Replace("Toolbox.ScriptableObjects.Events.", "")} [<color=cyan>{name}</color>] has been raised");
            
            listeners.ForEach(l =>
            {
                if (logListeners) l.LogCallback(this, null);
                l.callbacks?.Invoke();
            });
        }
    }
}