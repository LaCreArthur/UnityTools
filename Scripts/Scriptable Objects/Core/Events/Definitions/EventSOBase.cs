using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnityEventAndHolder<T>
{
    public Object holder;
    public T unityEvent;

    public UnityEventAndHolder(Object holder, T unityEvent)
    {
        this.holder = holder;
        this.unityEvent = unityEvent;
    }
}

public abstract class EventSOBase<T> : ScriptableObject, IEventSO<T>
{
    [SerializeField] protected List<UnityEventAndHolder<T>> listeners = new List<UnityEventAndHolder<T>>();
    [SerializeField] protected bool logRaise;

    public void AddListener(UnityEventAndHolder<T> eventAndHolder) => listeners.Add(eventAndHolder);

    public void RemoveListener(UnityEventAndHolder<T> eventAndHolder)
    {
        var listener = listeners.Find(l => l.holder == eventAndHolder.holder && Equals(l.unityEvent, eventAndHolder.unityEvent));
        if (listener != null)
            listeners.Remove(listener);
    }
}