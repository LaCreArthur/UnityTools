using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AS.Toolbox.UI
{
    [RequireComponent(typeof(CanvasAnimator))]
    public class CanvasAnimatorTweens : MonoBehaviour
    {
        public bool playChildTweensOnShow;
        public bool rewindChildTweensOnHide;

        CanvasAnimator _canvasAnimator;
        List<Tween> _childTweens;

        void Awake()
        {
            _canvasAnimator = GetComponent<CanvasAnimator>();
            InitChildTweens();

            if (playChildTweensOnShow) _canvasAnimator.onShow += PlayTweens;
            if (rewindChildTweensOnHide) _canvasAnimator.onHide += RewindTweens;
        }

        void InitChildTweens()
        {
            // don't search for tweens if not needed
            if (!playChildTweensOnShow || !rewindChildTweensOnHide) //todo: move this to a separate component
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
