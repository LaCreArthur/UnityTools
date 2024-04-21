using Lofelt.NiceVibrations;
using UnityEngine;

namespace AS.Toolbox
{
    public static class Vibrations
    {
        static bool s_isHaptic = true;
        public static bool IsHaptic
        {
            get => s_isHaptic;
            set {
                s_isHaptic = value;
                HapticController.hapticsEnabled = value;
                Debug.Log($"Haptics set to {value}");
            }
        }

        [RuntimeInitializeOnLoadMethod]
        static void Awake() => HapticController.hapticsEnabled = IsHaptic;
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

}
