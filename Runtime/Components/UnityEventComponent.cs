using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.Components
{
    public class UnityEventComponent : MonoBehaviour
    {
        public enum When
        {
            OnStart, OnUpdate, OnFixedUpdate, OnLateUpdate, OnDisable, OnEnable, OnDestroy, OnBecameVisible, OnBecameInvisible
        }

        public UnityEvent events;
        public bool onlyOnce;
        public When when;

        bool _invoked;

        void Start() => TryInvokeEvents(When.OnStart);

        void Update() => TryInvokeEvents(When.OnUpdate);

        void FixedUpdate() => TryInvokeEvents(When.OnFixedUpdate);

        void LateUpdate() => TryInvokeEvents(When.OnLateUpdate);

        void OnEnable() => TryInvokeEvents(When.OnEnable);

        void OnDisable() => TryInvokeEvents(When.OnDisable);

        void OnDestroy() => TryInvokeEvents(When.OnDestroy);

        void OnBecameInvisible() => TryInvokeEvents(When.OnBecameInvisible);

        void OnBecameVisible() => TryInvokeEvents(When.OnBecameVisible);

        void TryInvokeEvents(When whenEvent)
        {
            if (when != whenEvent || (onlyOnce && _invoked))
                return;

            events.Invoke();
            _invoked = true;
        }
    }
}
