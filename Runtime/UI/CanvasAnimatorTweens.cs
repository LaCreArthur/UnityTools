using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AS.Toolbox.UI
{
    public class CanvasAnimatorTweens : CanvasAnimatorComponent
    {
        public bool playChildTweensOnShow;
        public bool rewindChildTweensOnHide;

        List<Tween> _childTweens;

        public override void Initialize()
        {
            InitChildTweens();

            if (playChildTweensOnShow) CanvasAnimator.OnShow += PlayTweens;
            if (rewindChildTweensOnHide) CanvasAnimator.OnHide += RewindTweens;
        }

        void InitChildTweens()
        {
            // don't search for tweens if not needed
            if (!playChildTweensOnShow || !rewindChildTweensOnHide)
                return;

            // Init the list
            _childTweens = new List<Tween>();

            // get all DOTweenAnimation components in child
            DOTweenAnimation[] tweenAnimations = GetComponentsInChildren<DOTweenAnimation>();

            GameObject previousChild = null;
            foreach (DOTweenAnimation anim in tweenAnimations)
            {
                // no need to add tweens if there is more than one DOTweenAnimation component on the same GO
                // since anim.GetTweens() returns all the tweens
                if (anim.gameObject == previousChild)
                    continue;

                // add the tweens of this animation to the list
                _childTweens.AddRange(anim.GetTweens());

                // keep track of previous GO to avoid duplicates
                previousChild = anim.gameObject;
            }
        }

        void PlayTweens() => _childTweens.ForEach(tween => tween.Play());
        void RewindTweens() => _childTweens.ForEach(tween => tween.Rewind());
    }
}
