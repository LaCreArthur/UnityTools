using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Toolbox.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class CanvasAnimator : MonoBehaviour
{
    [Header("Start behaviours")]
    public bool startVisible = true;
    public bool resetPosOnStart = true;
    public GameStateSO associatedState;

    [FoldoutGroup("On Show")]
    public UnityEvent onShowEvents;
    public bool playChildTweensOnShow;
    public bool blockRaycastWhenVisible = true;
    public bool fadeIn;

    [ShowIf("fadeIn")]
    public Ease fadeInEase = Ease.InOutSine;

    [ShowIf("fadeIn")]
    public float fadeInDuration;

    [FoldoutGroup("On Hide Event")]
    public UnityEvent onHideEvents;
    public bool rewindChildTweensOnHide;
    public bool fadeOut;

    [ShowIf("fadeOut")]
    public Ease fadeOutEase = Ease.InOutSine;

    [ShowIf("fadeOut")]
    public float fadeOutDuration;

    [Header("Infos")]
    [SerializeField]
    [ReadOnly]
    bool isHidden;
    public bool IsHidden => isHidden;

    List<Tween> _childTweens;
    CanvasGroup _canvasGroup;
    Canvas _canvas;

    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();

        if (resetPosOnStart)
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        InitChildTweens();
        if (startVisible)
            Show();
        else
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvas.enabled = false;
        }
    }

    void OnEnable()
    {
        if (associatedState != null)
        {
            GameStateSM.Instance.state.onChange.Add(OnGameStateChange, this);
            OnGameStateChange();
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
            _childTweens.AddRange((anim.GetTweens()));

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
        if (playChildTweensOnShow)
        {
            foreach (var tween in _childTweens)
            {
                tween.Play();
            }
        }

        if (fadeIn)
            _canvasGroup.DOFade(1f, fadeInDuration).SetEase(fadeInEase).SetUpdate(true);
        else
            _canvasGroup.alpha = 1;
    }

    [Button]
    public void Hide()
    {
        onHideEvents.Invoke();
        _canvasGroup.blocksRaycasts = false;
        isHidden = true;

        DOTween.Kill(_canvasGroup);
        if (rewindChildTweensOnHide)
        {
            foreach (var tween in _childTweens)
            {
                tween.Rewind();
            }
        }

        if (fadeOut)
        {
            _canvasGroup.DOFade(0f, fadeOutDuration).SetEase(fadeOutEase).SetUpdate(true).OnComplete(() => _canvas.enabled = false);
        }
        else
            _canvas.enabled = false;
    }

    void OnGameStateChange()
    {
        if (GameStateSM.Instance.state.PreviousValue == associatedState)
            Hide();

        if (GameStateSM.Instance.state.v == associatedState)
            Show();
    }
}
