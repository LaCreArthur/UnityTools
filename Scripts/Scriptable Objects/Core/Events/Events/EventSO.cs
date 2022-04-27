using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Event", fileName = "E_")]
public class EventSO : EventSOBase<UnityEvent>
{
    [Button]
    public void Raise()
    {
        if (logRaise) Debug.Log(
            $"{GetType().ToString().Replace("ToolBox.ScriptableObjects.Events.", "")} [<color=cyan>{name}</color>] has been raised");
            
        listeners.ForEach(l =>
        {
            if (logListeners) l.LogCallback(this, null);
            l.callbacks?.Invoke();
        });
    }
}