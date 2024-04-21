using Lofelt.NiceVibrations;
using UnityEngine;

public static class VibrationManager
{
    static bool IsHaptic { get; set; } = true;

    [RuntimeInitializeOnLoadMethod]
    static void Awake()
    {
        Debug.Log($"Setting haptics to {IsHaptic}");
        HapticController.hapticsEnabled = IsHaptic;
    }

    public static void SetHaptics(bool v) => IsHaptic = v;

    public static void Vibrate() => HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
    public static void Haptic(HapticPatterns.PresetType type) => HapticPatterns.PlayPreset(type);
    public static void HapticSuccess() => HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);

    public static void HapticFailure() => HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
    public static void HapticSelection() => HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);

    public static void HapticSoft() => HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

    public static void HapticLight() => HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

    public static void HapticMedium() => HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);

    public static void HapticHeavy() => HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
}
