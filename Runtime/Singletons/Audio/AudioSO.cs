using UnityEngine;

namespace AS.Toolbox.Singletons.Audio
{
    /// <summary>
    /// Simple SO to play a sound inside a Unity Event in the inspector
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Managers/AudioSO")]
    public class AudioSO : ScriptableObject
    {
        public void Play(SoundSO s) => AudioSM.Play(s);
    }
}
