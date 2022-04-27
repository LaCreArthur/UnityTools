using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Event", fileName = "E_")]
public class EventSO : EventSOBase<UnityEvent>
{
    [Button]
    public void Raise()
    {
        if (logRaise) Debug.Log($"{name} raised !");
        listeners.ForEach(l => l.callbacks.Invoke());
    }
}