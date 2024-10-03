using AS.Toolbox;
using AS.Toolbox.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    [SerializeField, Required] BoolVar isHapticVar;
    void Awake() => OnHapticChange();
    void OnEnable() => isHapticVar.AddOnChange(OnHapticChange);
    void OnHapticChange() => Vibrations.IsHaptic = isHapticVar.v;
}
