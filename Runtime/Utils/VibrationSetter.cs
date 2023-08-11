using AS.Toolbox.ScriptableObjects;
using UnityEngine;

public class VibrationSetter : MonoBehaviour
{
    [SerializeField] BoolVar isHapticVar;
    void OnEnable() => isHapticVar.AddOnChange(OnHapticChange);
    void Start() => OnHapticChange();
    void OnDisable() => isHapticVar.RemoveOnChange(OnHapticChange);
    void OnHapticChange() => VibrationManager.SetHaptics(isHapticVar.v);
}
