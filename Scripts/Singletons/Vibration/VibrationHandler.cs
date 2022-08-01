using Toolbox.ScriptableObjects.Variables;
using UnityEngine;

#if MOREMOUNTAINS_NICEVIBRATIONS
using MoreMountains.NiceVibrations;
#else
#endif
public class VibrationHandler : MonoBehaviour
{
    [SerializeField] BoolVariable isHaptic = default;
    void OnEnable() => isHaptic.onChange.Add(SetHapticsActive, this);
    void OnDisable() => isHaptic.onChange.Remove(SetHapticsActive, this);

    void Awake() => SetHapticsActive();

    public static void AndroidVibrate()
    {
#if UNITY_ANDROID
        Handheld.Vibrate();
#endif
    }

    public void SetHapticsActive()
    {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.SetHapticsActive(isHaptic.v);
        MMVibrationManager.Haptic(HapticTypes.Selection);
#endif
    } 
    
    public static void Vibrate() {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.Vibrate();
#endif
    }
    
    public static void Haptic(HapticTypes type, bool defaultToRegularVibrate, bool alsoRumble, MonoBehaviour coroutineSupport, int controllerID) {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.Haptic(type, defaultToRegularVibrate, alsoRumble, coroutineSupport, controllerID);
#endif
    }
    
    public static void Haptic(HapticTypes type) {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.Haptic(type);
#endif
    }
    
    public static void HapticSuccess() {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.Haptic(HapticTypes.Success);
#endif
    }
    
    public static void HapticFailure() {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.Haptic(HapticTypes.Failure);
#endif
    }
    
    public static void HapticSelection() {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.Haptic(HapticTypes.Selection);
#endif
    }
    
    public static void HapticSoft() {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.Haptic(HapticTypes.SoftImpact);
#endif
    }
    
    public static void TransientHaptic(float intensity, float sharpness, bool alsoRumble, MonoBehaviour coroutineSupport, int controllerID)
    {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.TransientHaptic(intensity, sharpness, alsoRumble, coroutineSupport, controllerID);
#endif
    }
    
    public static void TransientHaptic(bool vibrateiOS, float iOSIntensity, float iOSSharpness, bool vibrateAndroid, float androidIntensity, float androidSharpness, bool vibrateAndroidIfNoSupport, bool rumble, float rumbleIntensity, float rumbleSharpness, int controllerID, MonoBehaviour coroutineSupport, bool threaded) {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.TransientHaptic(vibrateiOS, iOSIntensity, iOSSharpness, vibrateAndroid, androidIntensity, androidSharpness, vibrateAndroidIfNoSupport, rumble, rumbleIntensity, rumbleSharpness, controllerID, coroutineSupport, threaded);
#endif
    }
   
    
    public static void ContinuousHaptic(float intensity, float sharpness, float duration, HapticTypes fallbackOldiOS = HapticTypes.None) {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.ContinuousHaptic(intensity, sharpness, duration, fallbackOldiOS);
#endif
    }
    
    public static void ContinuousHaptic(float intensity, float sharpness, float duration, HapticTypes fallbackOldiOS, MonoBehaviour mono, bool alsoRumble, int controllerID, bool threaded, bool fullIntensity) {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.ContinuousHaptic(intensity, sharpness, duration, fallbackOldiOS, mono, alsoRumble, controllerID, threaded, fullIntensity);
#endif
    }
    
    public static void ContinuousHaptic(bool vibrateiOS, float iOSIntensity, float iOSSharpness, HapticTypes fallbackOldiOS, bool vibrateAndroid, float androidIntensity, float androidSharpness, bool vibrateAndroidIfNoSupport, bool rumble, float rumbleIntensity, float rumbleSharpness, int controllerID, float duration, MonoBehaviour mono, bool threaded, bool fullIntensity) {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.ContinuousHaptic(vibrateiOS, iOSIntensity, iOSSharpness, fallbackOldiOS, vibrateAndroid, androidIntensity, androidSharpness, vibrateAndroidIfNoSupport, rumble, rumbleIntensity, rumbleSharpness, controllerID, duration, mono, threaded, fullIntensity);
#endif
    }
    
    public static void UpdateContinuousHaptic(float intensity, float sharpness, bool alsoRumble, int controllerID, bool threaded) {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.UpdateContinuousHaptic(intensity, sharpness, alsoRumble, controllerID, threaded);
#endif
    }
    
    public static void UpdateContinuousHaptic(bool ios, float iosIntensity, float iosSharpness, bool android, float androidIntensity, float androidSharpness, bool rumble, float rumbleIntensity, float rumbleSharpness, int controllerID, bool threaded) {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.UpdateContinuousHaptic(ios, iosIntensity, iosSharpness, android, androidIntensity, androidSharpness, rumble, rumbleIntensity, rumbleSharpness, controllerID, threaded);
#endif
    }
    
    public static void StopAllHaptics(bool alsoRumble) {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.StopAllHaptics(alsoRumble);
#endif
    }
    
    public static void StopContinuousHaptic(bool alsoRumble) {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.StopContinuousHaptic(alsoRumble);
#endif
    }
}