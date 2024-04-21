using AS.Toolbox;
using AS.Toolbox.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
public class VibrationManager : MonoBehaviour
{
    [SerializeField] [Required] BoolVar isHapticVar;
    void Start() => OnHapticChange();
    void OnEnable() => isHapticVar.AddOnChange(OnHapticChange);
    void OnDisable() => isHapticVar.RemoveOnChange(OnHapticChange);
    void OnHapticChange() => Vibrations.IsHaptic = isHapticVar.v;
}
