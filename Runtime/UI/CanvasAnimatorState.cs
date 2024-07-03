using AS.Toolbox.ScriptableObjects;
using UnityEngine;

namespace AS.Toolbox.UI
{
    [RequireComponent(typeof(CanvasAnimator))]
    public class CanvasAnimatorState : MonoBehaviour
    {
        public GameStateSO associatedState;

        CanvasAnimator _canvasAnimator;

        void Awake()
        {
            _canvasAnimator = GetComponent<CanvasAnimator>();
            if (associatedState != null)
            {
                associatedState.AddOnEnter(_canvasAnimator.Show);
                associatedState.AddOnExit(_canvasAnimator.Hide);
            }
        }
    }

}
