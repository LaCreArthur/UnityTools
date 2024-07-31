using UnityEngine.Events;

namespace AS.Toolbox.UI
{
    public class CanvasAnimatorOnShow : CanvasAnimatorComponent
    {
        public UnityEvent onShowEvents;

        public override void Initialize()
        {
            if (onShowEvents != null)
            {
                CanvasAnimator.OnShow += onShowEvents.Invoke;
            }
        }
    }
}
