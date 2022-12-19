using System.Collections.Generic;
using AS.Toolbox.ScriptableObjects;
using AS.Toolbox.Singletons;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasAnimator : MonoBehaviour
    {
        public enum SlideDirection { Left, Right, Up, Down }

        [Header("Start behaviours")]
        public bool startVisible = true;
        public bool resetPosOnStart = true;
        public bool blockRaycastWhenVisible = true;
        public StateEnum associatedState;

        [FoldoutGroup("On Show")]
        public bool playChildTweensOnShow;
        [FoldoutGroup("On Show")]
        public bool fadeIn;
        [FoldoutGroup("On Show"), ShowIf("fadeIn"), Indent]
        public Ease fadeInEase = Ease.InOutSine;
        [FoldoutGroup("On Show"), ShowIf("fadeIn"), Indent]
        public float fadeInDuration = 0.25f;
        [FoldoutGroup("On Show")]
        public bool slideIn;
        [FoldoutGroup("On Show"), ShowIf("slideIn"), Indent]
        public SlideDirection slideInDirection;
        [FoldoutGroup("On Show"), ShowIf("slideIn"), Indent]
        public Ease slideInEase = Ease.InOutSine;
        [FoldoutGroup("On Show"), ShowIf("slideIn"), Indent]
        public float slideInDuration = 0.25f;
        [FoldoutGroup("On Show")]
        public UnityEvent onShowEvents;

        [FoldoutGroup("On Hide")]
        public bool rewindChildTweensOnHide;
        [FoldoutGroup("On Hide")]
        public bool fadeOut;
        [FoldoutGroup("On Hide"), ShowIf("fadeOut"), Indent]
        public float fadeOutDuration = 0.25f;
        [FoldoutGroup("On Hide"), ShowIf("fadeOut"), Indent]
        public Ease fadeOutEase = Ease.InOutSine;

        [FoldoutGroup("On Hide")]
        public bool slideOut;
        [FoldoutGroup("On Hide"), ShowIf("slideOut"), Indent]
        public SlideDirection slideOutDirection;
        [FoldoutGroup("On Hide"), ShowIf("slideOut"), Indent]
        public Ease slideOutEase = Ease.InOutSine;
        [FoldoutGroup("On Hide"), ShowIf("slideOut"), Indent]
        public float slideOutDuration = 0.25f;
        [FoldoutGroup("On Hide")]
        public UnityEvent onHideEvents;

        [Header("State"), SerializeField, ReadOnly]
        public bool isHidden;

        List<Tween> _childTweens;
        CanvasGroup _canvasGroup;
        Canvas _canvas;

        static float OutOfScreenWidth => Screen.width * 1.5f;
        static float OutOfScreenHeight => Screen.height * 1.5f;

        void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();

            if (resetPosOnStart)
                transform.localPosition = Vector3.zero;

            InitChildTweens();

            if (startVisible)
                Show();
            else
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
                _canvas.enabled = false;
            }

            if (associatedState != StateEnum.None)
            {
                var state = GameState.GetState(associatedState);
                state.AddOnEnter(Show, this);
                state.AddOnExit(Hide, this);
            }
        }

        void InitChildTweens()
        {
            // don't search for tweens if not needed
            if (!playChildTweensOnShow || !rewindChildTweensOnHide)
                return;

            // Init the list
            _childTweens = new List<Tween>();

            // get all DOTweenAnimation components in child
            var tweenAnimations = GetComponentsInChildren<DOTweenAnimation>();

            GameObject previousChild = null;
            foreach (var anim in tweenAnimations)
            {
                // no need to add tweens if there is more than one DOTweenAnimation component on the same GO
                // since anim.GetTweens() returns all the tweens
                if (anim.gameObject == previousChild)
                    continue;

                // add the tweens of this animation to the list
                _childTweens.AddRange(anim.GetTweens());

                // keep track of previous GO to avoid duplicates
                previousChild = anim.gameObject;

                // Debug.Log($"{anim.gameObject.name} contains {anim.GetTweens().Count} tweens");
            }
        }

        [Button]
        public void Show()
        {
            _canvas.enabled = true;
            onShowEvents.Invoke();
            _canvasGroup.blocksRaycasts = blockRaycastWhenVisible;
            isHidden = false;

            DOTween.Kill(_canvasGroup);
            if (playChildTweensOnShow) _childTweens.ForEach(tween => tween.Play());


            if (fadeIn)
                _canvasGroup.DOFade(1f, slideInDuration).SetEase(slideInEase).SetUpdate(true);
            else
            {
                _canvasGroup.alpha = 1;
            }

            if (slideIn)
            {
                // Debug.Log("Slide In " + slideInDirection);
                transform.localPosition = GetSlideDirection();
                transform.DOLocalMove(Vector3.zero, slideInDuration).SetEase(slideInEase).SetUpdate(true);
            }
        }

        //todo: maybe not it responsibility
        // in case of multiple tab panels
        public void Show(int lastIndex, int newIndex)
        {
            slideOutDirection = newIndex > lastIndex ? SlideDirection.Right : SlideDirection.Left;
            Show();
        }

        [Button]
        public void Hide()
        {
            onHideEvents.Invoke();
            _canvasGroup.blocksRaycasts = false;
            isHidden = true;

            DOTween.Kill(_canvasGroup);
            if (rewindChildTweensOnHide) _childTweens.ForEach(tween => tween.Rewind());


            if (fadeOut)
            {
                _canvasGroup.DOFade(0f, fadeOutDuration).SetEase(fadeOutEase).SetUpdate(true).OnComplete(() => _canvas.enabled = false);
            }
            if (slideOut)
            {
                transform.DOLocalMove(GetSlideDirection(), slideOutDuration).SetEase(slideOutEase).SetUpdate(true)
                    .OnComplete(() =>
                        {
                            _canvasGroup.alpha = 0;
                            _canvas.enabled = false;
                        }
                    );
            }
            if (!slideOut && !fadeOut)
            {
                _canvasGroup.alpha = 0;
                _canvas.enabled = false;
            }
        }

        // in case of multiple tab panels
        public void Hide(int lastIndex, int newIndex)
        {
            slideOutDirection = newIndex > lastIndex ? SlideDirection.Left : SlideDirection.Right;
            Hide();
        }

        Vector3 GetSlideDirection()
        {
            return slideOutDirection switch
            { SlideDirection.Left => new Vector3(-OutOfScreenWidth, 0, 0),
              SlideDirection.Right => new Vector3(OutOfScreenWidth, 0, 0),
              SlideDirection.Up => new Vector3(0, OutOfScreenHeight, 0),
              SlideDirection.Down => new Vector3(0, -OutOfScreenHeight, 0),
              _ => Vector3.zero };
        }
    }
}