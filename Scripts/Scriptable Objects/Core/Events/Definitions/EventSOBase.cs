using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EventSOBase<T> : ScriptableObject, IEventSO<T> 
{
    [SerializeField, InlineProperty, HideReferenceObjectPicker, ListDrawerSettings(IsReadOnly = true, Expanded = true), OnInspectorGUI("RemoveNullElements")]  
    protected List<ReferencedUltEvent<T>> listeners = new List<ReferencedUltEvent<T>>();

    [SerializeField] 
    protected bool logRaise;
    [SerializeField] 
    protected bool logListeners;

    public void AddListener(ReferencedUltEvent<T> ultEvent)
    {
        if (listeners == null) listeners = new List<ReferencedUltEvent<T>>();
        listeners.Add(ultEvent);
    }

    public void RemoveListener(ReferencedUltEvent<T> ultEvent)
    {
        if (listeners == null) return;
        var listener = listeners.Find(l => l.listener == ultEvent.listener && Equals(l.callbacks, ultEvent.callbacks));
        listeners.Remove(listener);
    }
    
    void RemoveNullElements() => listeners.RemoveAll(l => l.listener == null);
}