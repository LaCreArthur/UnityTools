using AS.Toolbox;
using AS.Toolbox.ScriptableObjects;
using AS.Toolbox.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;

public class VibrationManager : SingletonMono<VibrationManager>
{
    [SerializeField] [Required] BoolVar isHapticVar;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        isHapticVar?.RemoveOnChange(OnHapticChange);
    }
    void Start()
    {
        OnHapticChange();
        isHapticVar.AddOnChange(OnHapticChange);
    }
    void OnHapticChange() => Vibrations.IsHaptic = isHapticVar.v;
}
