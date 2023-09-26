using System.Collections.Generic;
using AS.Toolbox.ScriptableObjects;
using AS.Toolbox.Utils;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace AS.Toolbox.UI
{
    [RequireComponent(typeof(Canvas)), RequireComponent(typeof(CanvasGroup))]
    public class CanvasAnimator : MonoBehaviour
    {
        public enum SlideDirection { Left, Right, Up, Down }

        [Header("Start behaviours")]
        public bool startVisible = true;
        public bool resetPosOnStart = true;
        public bool blockRaycastWhenVisible = true;
        public GameStateSO associatedState;
        [ListDrawerSettings(DefaultExpandedState = true, AlwaysAddDefaultValue = true), InlineProperty(LabelWidth = 20)]
        public GameStateSO[] associatedStates;

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
        [FoldoutGroup("On Show"), FoldoutGroup("On Show")]
        public bool scaleIn;
        [FoldoutGroup("On Show"), ShowIf("scaleIn"), Indent]
        public Ease scaleInEase = Ease.InOutSine;
        [FoldoutGroup("On Show"), ShowIf("scaleIn"), Indent]
        public float scaleInDuration = 0.25f;

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
        public bool scaleOut;
        [FoldoutGroup("On Hide"), ShowIf("scaleOut"), Indent]
        public float scaleOutDuration = 0.25f;
        [FoldoutGroup("On Hide"), ShowIf("scaleOut"), Indent]
        public Ease scaleOutEase = Ease.InOutSine;

        [FoldoutGroup("On Hide")]
        public UnityEvent onHideEvents;

        [Header("State"), SerializeField, ReadOnly]
        public bool isHidden;

        Canvas _canvas;
        CanvasGroup _canvasGroup;
        List<Tween> _childTweens;
        public bool IsInitialized { get; private set; }

        static float OutOfScreenWidth => Screen.width * 1.5f;
        static float OutOfScreenHeight => Screen.height * 1.5f;

        public void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();

            if (resetPosOnStart)
                transform.localPosition = Vector3.zero;

            InitChildTweens();

            isHidden = true;

            if (associatedState != null)
            {
                associatedState.AddOnEnter(Show);
                associatedState.AddOnExit(Hide);
            }

            if (associatedStates.Length > 0)
            {
                foreach (GameStateSO state in associatedStates)
                {
                    state.AddOnEnter(Show);
                    state.AddOnExit(Hide);
                }
            }

            if (startVisible)
                Show();
            else
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
                _canvas.enabled = false;
                StartCoroutine(Coroutines.WaitForEndOfFrame(() => gameObject.SetActive(false)));
            }

            IsInitialized = true;
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

                // Debug.Log($"{anim.gameObject.name} contains {anim.GetTweens().Count} tweens");
            }
        }

        [Button]
        public void Show()
        {
            // if already visible, do nothing
            if (!isHidden)
                return;
            isHidden = false;

            gameObject.SetActive(true);
            _canvas.enabled = true;
            _canvasGroup.blocksRaycasts = blockRaycastWhenVisible;

            DOTween.Kill(_canvasGroup);
            if (playChildTweensOnShow) _childTweens.ForEach(tween => tween.Play());

            if (fadeIn)
                _canvasGroup.DOFade(1f, slideInDuration).SetEase(slideInEase).SetUpdate(true);
            else
                _canvasGroup.alpha = 1;

            if (slideIn)
            {
                // Debug.Log("Slide In " + slideInDirection);
                transform.localPosition = GetSlideDirection();
                transform.DOLocalMove(Vector3.zero, slideInDuration).SetEase(slideInEase).SetUpdate(true);
            }

            if (scaleIn)
            {
                transform.localScale = Vector3.zero;
                transform.DOScale(Vector3.one, scaleInDuration).SetEase(scaleInEase).SetUpdate(true);
            }

            onShowEvents.Invoke();
        }

        //todo: maybe not it responsibility
        // refactoring this with a new component dedicated to slide panels? 
        // in case of multiple tab panels
        public void Show(int lastIndex, int newIndex)
        {
            slideInDirection = newIndex > lastIndex ? SlideDirection.Right : SlideDirection.Left;
            Show();
        }

        [Button]
        public void Hide()
        {
            // if already hidden, do nothing
            if (isHidden)
                return;
            isHidden = true;

            _canvasGroup.blocksRaycasts = false;

            DOTween.Kill(_canvasGroup);
            if (rewindChildTweensOnHide) _childTweens.ForEach(tween => tween.Rewind());

            if (fadeOut)
                _canvasGroup.DOFade(0f, fadeOutDuration).SetEase(fadeOutEase).SetUpdate(true).OnComplete(FinalizeHide);
            if (slideOut)
            {
                transform.DOLocalMove(GetSlideDirection(), slideOutDuration)
                    .SetEase(slideOutEase)
                    .SetUpdate(true)
                    .OnComplete(FinalizeHide);
            }

            if (scaleOut)
            {
                transform.localScale = Vector3.one;
                transform.DOScale(Vector3.zero, scaleOutDuration).SetEase(scaleOutEase).SetUpdate(true).OnComplete(FinalizeHide);
            }

            onHideEvents.Invoke();

            if (!slideOut && !fadeOut && !scaleOut)
                FinalizeHide();
            return;

            void FinalizeHide()
            {
                _canvasGroup.alpha = 0;
                _canvas.enabled = false;
                gameObject.SetActive(false);
            }
        }

        // in case of multiple tab panels
        public void Hide(int lastIndex, int newIndex)
        {
            slideOutDirection = newIndex > lastIndex ? SlideDirection.Left : SlideDirection.Right;
            Hide();
        }

        Vector3 GetSlideDirection() => slideOutDirection switch
        {
            SlideDirection.Left => new Vector3(-OutOfScreenWidth, 0, 0),
            SlideDirection.Right => new Vector3(OutOfScreenWidth, 0, 0),
            SlideDirection.Up => new Vector3(0, OutOfScreenHeight, 0),
            SlideDirection.Down => new Vector3(0, -OutOfScreenHeight, 0),
            _ => Vector3.zero
        };
    }
}
