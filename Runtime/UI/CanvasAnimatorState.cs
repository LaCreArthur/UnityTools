using AS.Toolbox.ScriptableObjects;

namespace AS.Toolbox.UI
{
    public class CanvasAnimatorState : CanvasAnimatorComponent
    {
        public GameStateSO associatedState;

        public override void Initialize()
        {
            if (associatedState != null)
            {
                associatedState.AddOnEnter(CanvasAnimator.Show);
                associatedState.AddOnExit(CanvasAnimator.Hide);
            }
        }
    }
}
