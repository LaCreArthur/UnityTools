using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.UI
{
    [RequireComponent(typeof(CanvasAnimator))]
    public class CanvasAnimatorOnHide : MonoBehaviour
    {
        public UnityEvent onHideEvents;

        CanvasAnimator _canvasAnimator;

        void Awake()
        {
            _canvasAnimator = GetComponent<CanvasAnimator>();
            if (onHideEvents != null)
            {
                _canvasAnimator.OnHide += onHideEvents.Invoke;
            }
        }
    }
}
