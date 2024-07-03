using AS.Toolbox.ScriptableObjects;
using UnityEngine;

namespace AS.Toolbox.UI
{
    [RequireComponent(typeof(CanvasAnimator))]
    public class CanvasAnimatorEvent : MonoBehaviour
    {
        public SOEvent associatedEvent;

        CanvasAnimator _canvasAnimator;

        void Awake()
        {
            _canvasAnimator = GetComponent<CanvasAnimator>();
            if (associatedEvent != null)
            {
                associatedEvent.Add(_canvasAnimator.Show);
            }
        }

        void OnDestroy()
        {
            if (associatedEvent != null)
            {
                associatedEvent.Remove(_canvasAnimator.Show);
            }
        }
    }
}
    
