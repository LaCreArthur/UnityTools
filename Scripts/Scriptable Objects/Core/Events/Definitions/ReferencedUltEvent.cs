using Sirenix.OdinInspector;
using UnityEngine;

public class ReferencedUltEvent<T>
{
    [HideLabel]
    public readonly Object listener;
    [HideReferenceObjectPicker, ListDrawerSettings(Expanded = true)]
    public readonly T callbacks;

    public ReferencedUltEvent(Object listener, T callbacks)
    {
        this.listener = listener;
        this.callbacks = callbacks;
    }
}