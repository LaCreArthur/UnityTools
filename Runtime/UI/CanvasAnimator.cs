using System;
using AS.Toolbox.Utils;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.UI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public class CanvasAnimator : MonoBehaviour
    {
        public enum SlideDirection { Left, Right, Up, Down }

        [Header("Start behaviours")]
        public bool startVisible = true;
        public bool resetPosOnStart = true;
        public bool blockRaycastWhenVisible = true;


        [FoldoutGroup("On Show")] public bool fadeIn;
        [FoldoutGroup("On Show")] [ShowIf("fadeIn")] [Indent] public Ease fadeInEase = Ease.InOutSine;
        [FoldoutGroup("On Show")] [ShowIf("fadeIn")] [Indent] public float fadeInDuration = 0.25f;
        [FoldoutGroup("On Show")] public bool slideIn;
        [FoldoutGroup("On Show")] [ShowIf("slideIn")] [Indent] public SlideDirection slideInDirection;
        [FoldoutGroup("On Show")] [ShowIf("slideIn")] [Indent] public Ease slideInEase = Ease.InOutSine;
        [FoldoutGroup("On Show")] [ShowIf("slideIn")] [Indent] public float slideInDuration = 0.25f;
        [FoldoutGroup("On Show")] public bool scaleIn;
        [FoldoutGroup("On Show")] [ShowIf("scaleIn")] [Indent] public Ease scaleInEase = Ease.InOutSine;
        [FoldoutGroup("On Show")] [ShowIf("scaleIn")] [Indent] public float scaleInDuration = 0.25f;


        [FoldoutGroup("On Hide")] public bool fadeOut;
        [FoldoutGroup("On Hide")] [ShowIf("fadeOut")] [Indent] public float fadeOutDuration = 0.25f;
        [FoldoutGroup("On Hide")] [ShowIf("fadeOut")] [Indent] public Ease fadeOutEase = Ease.InOutSine;
        [FoldoutGroup("On Hide")] public bool slideOut;
        [FoldoutGroup("On Hide")] [ShowIf("slideOut")] [Indent] public SlideDirection slideOutDirection;
        [FoldoutGroup("On Hide")] [ShowIf("slideOut")] [Indent] public Ease slideOutEase = Ease.InOutSine;
        [FoldoutGroup("On Hide")] [ShowIf("slideOut")] [Indent] public float slideOutDuration = 0.25f;
        [FoldoutGroup("On Hide")] public bool scaleOut;
        [FoldoutGroup("On Hide")] [ShowIf("scaleOut")] [Indent] public float scaleOutDuration = 0.25f;
        [FoldoutGroup("On Hide")] [ShowIf("scaleOut")] [Indent] public Ease scaleOutEase = Ease.InOutSine;

        [Header("State")]
        [SerializeField] [ReadOnly] public bool isHidden;
        Canvas _canvas;
        CanvasGroup _canvasGroup;

        public event Action OnShow;
        public event Action OnHide;

        public bool IsInitialized { get; private set; }

        static float OutOfScreenWidth => Screen.width * 1.5f;
        static float OutOfScreenHeight => Screen.height * 1.5f;

        public void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();

            if (resetPosOnStart)
                transform.localPosition = Vector3.zero;


            isHidden = true;

            if (startVisible)
                Show();
            else
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
                _canvas.enabled = false;
                gameObject.SetActive(false);
            }

            IsInitialized = true;
        }



        [Button]
        public void Show()
        {
            if (!IsInitialized)
            {
                //Debug.Log("Delay show because not initialized yet", this);
                StartCoroutine(Coroutines.WaitForEndOfFrame(Show));
                return;
            }

            // if already visible, do nothing
            if (!isHidden)
                return;

            isHidden = false;

            gameObject.SetActive(true);
            _canvas.enabled = true;
            _canvasGroup.blocksRaycasts = blockRaycastWhenVisible;

            // DOTween.Kill(this); // kill the potential set inactive on awake
            DOTween.Kill(_canvasGroup);

            if (fadeIn)
                _canvasGroup.DOFade(1f, slideInDuration).SetEase(slideInEase).SetUpdate(true);
            else
                _canvasGroup.alpha = 1;

            if (slideIn)
            {
                transform.localPosition = GetSlideDirection();
                transform.DOLocalMove(Vector3.zero, slideInDuration).SetEase(slideInEase).SetUpdate(true);
            }

            if (scaleIn)
            {
                transform.localScale = Vector3.zero;
                transform.DOScale(Vector3.one, scaleInDuration).SetEase(scaleInEase).SetUpdate(true);
            }

            OnShow?.Invoke();
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

            OnHide?.Invoke();

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
