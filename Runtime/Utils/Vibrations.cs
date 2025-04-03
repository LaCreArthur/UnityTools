using Interhaptics.Core;
using Interhaptics.Utils;
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
                HAR.SetGlobalIntensity(value ? 1 : 0);
            }
        }

        [RuntimeInitializeOnLoadMethod]
        static void Awake()
        {
            if (HAR.Init())
            {
                Debug.Log("Haptic and Audio Reactive initialized");
                IsHaptic = true;
            }
            else
            {
                Debug.LogWarning("Haptic and Audio Reactive failed to initialize");
                IsHaptic = false;
            }
            HAR.PlayConstant(0.5, 0.5);
        }

        public static void Haptic(HapticPreset.PresetType type) => HapticPreset.Play(type);
        public static void HapticSuccess() => HapticPreset.Play(HapticPreset.PresetType.Success);
        public static void HapticFailure() => HapticPreset.Play(HapticPreset.PresetType.Failure);
        public static void HapticSelection() => HapticPreset.Play(HapticPreset.PresetType.Selection);
        public static void HapticSoft() => HapticPreset.Play(HapticPreset.PresetType.Soft);
        public static void HapticLight() => HapticPreset.Play(HapticPreset.PresetType.Light);
        public static void HapticMedium() => HapticPreset.Play(HapticPreset.PresetType.Medium);
        public static void HapticHeavy() => HapticPreset.Play(HapticPreset.PresetType.Heavy);
        public static void HapticRigid() => HapticPreset.Play(HapticPreset.PresetType.Rigid);
    }
}
