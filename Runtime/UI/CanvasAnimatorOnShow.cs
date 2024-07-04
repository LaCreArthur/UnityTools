using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.UI
{
    [RequireComponent(typeof(CanvasAnimator))]
    public class CanvasAnimatorOnShow : MonoBehaviour
    {
        public UnityEvent onShowEvents;

        CanvasAnimator _canvasAnimator;

        void Awake()
        {
            _canvasAnimator = GetComponent<CanvasAnimator>();
            if (onShowEvents != null)
            {
                _canvasAnimator.onShow += onShowEvents.Invoke;
            }
        }
    }
}
