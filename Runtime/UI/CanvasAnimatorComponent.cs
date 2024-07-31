using UnityEngine;

namespace AS.Toolbox.UI
{
    [RequireComponent(typeof(CanvasAnimator))]
    public abstract class CanvasAnimatorComponent : MonoBehaviour
    {
        CanvasAnimator _canvasAnimator;
        protected CanvasAnimator CanvasAnimator => _canvasAnimator ??= GetComponent<CanvasAnimator>();
        public abstract void Initialize();
    }
}
