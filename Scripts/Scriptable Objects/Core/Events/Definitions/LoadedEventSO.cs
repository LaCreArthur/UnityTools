using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class LoadedEventSO<T> : EventSOBase<UnityEvent<T>>
{
    [SerializeField] T testVal;

    [Button]
    public void TestRaise() => Raise(testVal);
    
    public void Raise(T t)
    {
        if (logRaise) Debug.Log($"{name} raised with value {t}!");
        listeners.ForEach(l => l.unityEvent.Invoke(t));
    }
}