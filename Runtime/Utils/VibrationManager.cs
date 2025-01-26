using AS.Toolbox;
using AS.Toolbox.ScriptableObjects;
using AS.Toolbox.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;

public class VibrationManager : SingletonMono<VibrationManager>
{
    [SerializeField] [Required] BoolVar isHapticVar;
    void Start()
    {
        OnHapticChange();
        isHapticVar.AddOnChange(OnHapticChange);
    }

    void OnDestroy() => isHapticVar?.RemoveOnChange(OnHapticChange);
    void OnHapticChange() => Vibrations.IsHaptic = isHapticVar.v;
}
