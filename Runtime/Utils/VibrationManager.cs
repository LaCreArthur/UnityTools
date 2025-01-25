using AS.Toolbox;
using AS.Toolbox.ScriptableObjects;
using AS.Toolbox.Singletons;
using Sirenix.OdinInspector;
using UnityEngine;

public class VibrationManager : SingletonMono<VibrationManager>
{
    [SerializeField] [Required] BoolVar isHapticVar;
    void OnEnable() => isHapticVar.AddOnChange(OnHapticChange);
    void Start() => OnHapticChange();
    void OnHapticChange() => Vibrations.IsHaptic = isHapticVar.v;
}
