using AS.Toolbox.Attributes;
using AS.Toolbox.ScriptableObjects;
using AS.Toolbox.Singletons;
using AS.Toolbox.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.Components
{
    [RequireComponent(typeof(Collider))]
    public class ColliderEventComponent : MonoBehaviour
    {
        public ColliderEventType type;
        public bool useTag;
        [ShowIf("useTag"), TagDropdown]
         public string otherTag = "";
        [HideIf("useTag")]
        public LayerMask otherLayer;

        public bool onlyOnce;
        [ShowIf("onlyOnce")]
        public bool destroyGO;
        [ShowIf("destroyGO")]
        public float delay;

        public bool resetTriggerOnStateEvent;
        [ShowIf("resetTriggerOnStateEvent"), Indent]
        public StateEnum whatState;
        [ShowIf("resetTriggerOnStateEvent"), Indent]
        public EventEnum whatEvent;
        [SerializeField, ReadOnly]
        bool triggered;

        [ShowIf("@type==ColliderEventType.CollisionEnter||type==ColliderEventType.CollisionStay||type==ColliderEventType.CollisionExit")]
        public UnityEvent<Collision> collisionEvent;
        [HideIf("@type==ColliderEventType.CollisionEnter||type==ColliderEventType.CollisionStay||type==ColliderEventType.CollisionExit")]
        public UnityEvent<Collider> triggerEvent;

        void Start()
        {
            if (!resetTriggerOnStateEvent)
                return;

            GameState.GetState(whatState).Add(whatEvent, ResetTrigger);
        }

        void ResetTrigger() => triggered = false;
        void OnCollisionEnter(Collision other) => CheckCollisions(other, ColliderEventType.CollisionEnter);
        void OnCollisionStay(Collision other) => CheckCollisions(other, ColliderEventType.CollisionStay);
        void OnCollisionExit(Collision other) => CheckCollisions(other, ColliderEventType.CollisionExit);
        void OnTriggerEnter(Collider other) => CheckTrigger(other, ColliderEventType.TriggerEnter);
        void OnTriggerStay(Collider other) => CheckTrigger(other, ColliderEventType.TriggerStay);
        void OnTriggerExit(Collider other) => CheckTrigger(other, ColliderEventType.TriggerExit);

        void CheckCollisions(Collision other, ColliderEventType t)
        {
            if (ShouldEventsBeInvoked(other.collider, t))
            {
                collisionEvent.Invoke(other);
                AfterInvoke();
            }
        }

        void CheckTrigger(Collider other, ColliderEventType t)
        {
            if (ShouldEventsBeInvoked(other, t))
            {
                triggerEvent.Invoke(other);
                AfterInvoke();
            }
        }

        bool ShouldEventsBeInvoked(Collider other, ColliderEventType t)
        {
            if (type != t) return false;
            if (onlyOnce && triggered) return false;
            if (useTag && !other.gameObject.CompareTag(otherTag)) return false;
            if (!useTag && !otherLayer.CollidesWith(other.gameObject.layer)) return false;
            return true;
        }

        void AfterInvoke()
        {
            triggered = true;
            if (destroyGO)
                Destroy(gameObject, delay);
        }
    }

    public enum ColliderEventType { CollisionEnter, CollisionStay, CollisionExit, TriggerEnter, TriggerStay, TriggerExit }
}
