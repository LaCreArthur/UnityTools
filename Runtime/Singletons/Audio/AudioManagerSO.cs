using UnityEngine;

namespace AS.Toolbox.Singletons.Audio
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Managers/Audio Manager")]
    public class AudioManagerSO : ScriptableObject
    {
        public AudioSM audioSingleton;
        public void Play(SoundSO s) => audioSingleton.Play(s);
    }
}
