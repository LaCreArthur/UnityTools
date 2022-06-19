using UnityEngine;

namespace Toolbox.Audio
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Managers/Audio Manager")]
    public class AudioManagerSO : ScriptableObject
    {
        public AudioSingletonMono audioSingleton;
        public void Play(string sound) => audioSingleton.Play(sound);
    }
}
