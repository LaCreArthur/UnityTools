using DG.Tweening;
using Sirenix.OdinInspector;
using Toolbox.Audio;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonOnClickFeedbacks : MonoBehaviour
{
    public bool punchScale, haptic, sfx, spine;
    [ShowIf("punchScale")] public float punchScaleAmount = 0.2f;

    Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Play);
    }

    [Button]
    public void Play()
    {
        if (sfx)
            AudioSM.Instance.Play("click");
        if (haptic)
            VibrationHandler.HapticSelection();
        if (punchScale)
        {
            transform.localScale = Vector3.one;
            transform.DOPunchScale(Vector3.one * punchScaleAmount, 0.5f, 6, 0.6f)
                .SetEase(Ease.OutQuad).OnComplete(() => transform.localScale = Vector3.one);
        }
        if (spine)
        {
            transform.localScale = Vector3.one;
            transform.DOPunchRotation(Vector3.forward * 45f, 0.5f, 6, 0.6f)
                .SetEase(Ease.OutQuad).OnComplete(() => transform.localRotation = Quaternion.identity);
        }
    }
}