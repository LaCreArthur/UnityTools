using DG.Tweening;
using Sirenix.OdinInspector;
using Toolbox.Audio;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonPunchScaleOnClick : MonoBehaviour
{
    Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Play);
    }

    [Button]
    public void Play()
    {
        AudioSM.Instance.Play("Click");
        transform.localScale = Vector3.one;
        transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 6, 0.6f).SetEase(Ease.OutQuad)
            .OnComplete(() => transform.localScale = Vector3.one);
    }

    void OnDestroy() => _button.onClick.RemoveListener(Play);
}