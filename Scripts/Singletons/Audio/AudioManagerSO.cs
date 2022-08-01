using UnityEngine;

namespace Toolbox.Audio
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Managers/Audio Manager")]
    public class AudioManagerSO : ScriptableObject
    {
        public AudioSM audioSingleton;
        public void Play(string sound) => audioSingleton.Play(sound);
    }
}
