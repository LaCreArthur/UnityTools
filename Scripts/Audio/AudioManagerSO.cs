using UnityEngine;

namespace Toolbox.Audio
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Managers/Audio Manager")]
    public class AudioManagerSO : ScriptableObject
    {
        public AudioSingletonMB audioSingleton;
        public void Play(string sound) => audioSingleton.Play(sound);
    }
}
