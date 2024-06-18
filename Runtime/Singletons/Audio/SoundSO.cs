using Sirenix.OdinInspector;
using UnityEngine;

namespace AS.Toolbox.Singletons.Audio
{
    public class SoundSO : ScriptableObject
    {
        [ListDrawerSettings(ShowFoldout = false)]
        public AudioClip[] clips;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(0f, 1f)]
        public float volumeVariance;

        [Range(.1f, 3f)]
        public float pitch = 1f;

        [Range(0f, 1f)]
        public float pitchVariance;

        public bool loop;

        [HideInInspector] public AudioSource source;
    }
}
