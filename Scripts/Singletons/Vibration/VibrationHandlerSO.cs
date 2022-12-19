#if MOREMOUNTAINS_NICEVIBRATIONS
using MoreMountains.NiceVibrations;
#endif
using UnityEngine;

namespace AS.Toolbox.Singletons.Vibration
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Managers/Vibration Manager")]
    public class VibrationHandlerSO : ScriptableObject
    {
        public void Vibrate() => VibrationHandler.Vibrate();
        public void Haptic(HapticTypes type) => VibrationHandler.Haptic(type);
        public void HapticSuccess() => VibrationHandler.HapticSuccess();
        public void HapticFailure() => VibrationHandler.HapticFailure();
        public void HapticSelection() => VibrationHandler.HapticSelection();
        public void HapticSoft() => VibrationHandler.HapticSoft();
        public void StopAllHaptics(bool alsoRumble) => VibrationHandler.StopAllHaptics(alsoRumble);
        public void StopContinuousHaptic(bool alsoRumble) => VibrationHandler.StopContinuousHaptic(alsoRumble);
    }


#if !MOREMOUNTAINS_NICEVIBRATIONS
// create class HapticTypes so the class compile and references are not broken if NiceVibration is not imported
    public class HapticTypes
    {
        public const HapticTypes None = null;
        public const HapticTypes MediumImpact = null;
        public const HapticTypes HeavyImpact = null;
        public const HapticTypes LightImpact = null;
        public const HapticTypes SoftImpact = null;
    }
#endif
}