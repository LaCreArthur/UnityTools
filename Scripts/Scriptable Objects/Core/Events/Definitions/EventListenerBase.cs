using UnityEngine;

public interface IEventSO<TCallbacks>
{
    public void AddListener(UnityEventAndHolder<TCallbacks> eventAndHolder);
    public void RemoveListener(UnityEventAndHolder<TCallbacks> eventAndHolder);
}

[ExecuteAlways]
public abstract class EventListenerBase<TEvent, TCallbacks> : MonoBehaviour where TEvent : IEventSO<TCallbacks>
{
    [SerializeField] protected TEvent eventSO;
    [SerializeField] protected TCallbacks callbacks;
    
    protected void AddListener()
    {
        if (eventSO != null)
            eventSO.AddListener(new UnityEventAndHolder<TCallbacks>(this, callbacks));
    }

    protected void RemoveListener()
    {
        if (eventSO != null)
            eventSO.RemoveListener(new UnityEventAndHolder<TCallbacks>(this, callbacks));
    }
    
    protected void OnEnable() => AddListener();
    protected void OnDisable() => RemoveListener();
}