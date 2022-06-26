using Sirenix.OdinInspector;
using Toolbox.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.ScriptableObjects.Events
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Events/Event", fileName = "E_")]
    public class EventSO : EventSOBase<UnityEvent>
    {
        [TitleGroup("Debug"), Button]
        public void Raise()
        {
            if (logRaise) Debug.Log(
                $"{this.TypeAndNameToString()} has been raised");
            
            listeners.ForEach(l =>
            {
                if (logListeners) l.LogCallback(this, null);
                l.callbacks?.Invoke();
            });
        }
    }
}