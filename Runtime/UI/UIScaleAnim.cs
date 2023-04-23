using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.UI
{
    public enum AnimType { In, Out, InAndOut }
    public class UIScaleAnim : MonoBehaviour
    {
        [SerializeField] float scaleTime = 1f;
        [SerializeField] float scaleTo = 0.5f;
        [SerializeField] float scaleFrom = 0.5f;
        [SerializeField] Ease easeIn = Ease.InOutSine;
        [SerializeField] Ease easeOut = Ease.InOutSine;
        [SerializeField] bool onEnable;
        [ShowIf("onEnable"), SerializeField]
        AnimType animType;
        [SerializeField] bool loop;
        [ShowIf("loop"), SerializeField]
        int loops;
        [ShowIf("loop"), SerializeField]
        LoopType loopType;

        RectTransform _rect;
        Sequence _inAndOutLoopSeq;
        void Awake() => _rect = GetComponent<RectTransform>();

        void OnEnable()
        {
            if (!onEnable)
                return;
            switch (animType)
            {
                case AnimType.In:
                    ScaleIn();
                    break;
                case AnimType.Out:
                    ScaleOut();
                    break;
                case AnimType.InAndOut:
                    ScaleInAndOut();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void OnDisable()
        {
            if (!loop)
                return;
            _rect.DOKill();
            _inAndOutLoopSeq?.Kill();

        }

        public void ScaleIn()
        {
            _rect.localScale = new Vector3(scaleFrom, scaleFrom, scaleFrom);
            if (loop)
                _rect.DOScale(scaleTo, scaleTime).SetEase(easeIn).SetLoops(loops, loopType);
            else
                _rect.DOScale(scaleTo, scaleTime).SetEase(easeIn);
        }

        public void ScaleOut()
        {
            _rect.localScale = new Vector3(scaleTo, scaleTo, scaleTo);
            if (loop)
                _rect.DOScale(scaleFrom, scaleTime).SetEase(easeOut).SetLoops(loops, loopType);
            else
                _rect.DOScale(scaleFrom, scaleTime).SetEase(easeOut);
        }

        public void ScaleInAndOut()
        {
            _rect.localScale = new Vector3(scaleFrom, scaleFrom, scaleFrom);
            if (loop)
            {
                // create a sequence to loop the whole in and out animation
                _inAndOutLoopSeq = DOTween.Sequence();
                _inAndOutLoopSeq.Append(_rect.DOScale(scaleTo, scaleTime).SetEase(easeIn));
                _inAndOutLoopSeq.Append(_rect.DOScale(scaleFrom, scaleTime).SetEase(easeOut));
                _inAndOutLoopSeq.SetLoops(loops, loopType);
            }
            else
                _rect.DOScale(scaleTo, scaleTime).SetEase(easeIn).OnComplete(ScaleOut);
        }
    }
}
