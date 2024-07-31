using UnityEngine.Events;

namespace AS.Toolbox.UI
{
    public class CanvasAnimatorOnHide : CanvasAnimatorComponent
    {
        public UnityEvent onHideEvents;

        public override void Initialize()
        {
            if (onHideEvents != null)
            {
                CanvasAnimator.OnHide += onHideEvents.Invoke;
            }
        }
    }
}
