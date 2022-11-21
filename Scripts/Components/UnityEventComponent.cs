using UnityEngine;
using UnityEngine.Events;

public class UnityEventComponent : MonoBehaviour
{
    public UnityEvent events;
    public bool onlyOnce;
    bool _invoked;

    public enum When { OnStart, OnUpdate, OnFixedUpdate, OnLateUpdate, OnDisable, OnEnable, OnDestroy, OnBecameVisible, OnBecameInvisible }

    public When when;

    void TryInvokeEvents(When whenEvent)
    {
        if (when != whenEvent || onlyOnce && _invoked)
            return;

        events.Invoke();
        _invoked = true;
    }

    void Start() => TryInvokeEvents(When.OnStart);

    void Update() => TryInvokeEvents(When.OnUpdate);

    void FixedUpdate() => TryInvokeEvents(When.OnFixedUpdate);

    void LateUpdate() => TryInvokeEvents(When.OnLateUpdate);

    void OnEnable() => TryInvokeEvents(When.OnEnable);

    void OnDisable() => TryInvokeEvents(When.OnDisable);

    void OnDestroy() => TryInvokeEvents(When.OnDestroy);

    void OnBecameVisible() => TryInvokeEvents(When.OnBecameVisible);

    void OnBecameInvisible() => TryInvokeEvents(When.OnBecameInvisible);
}