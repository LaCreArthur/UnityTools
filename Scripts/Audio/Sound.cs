using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Toolbox.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public List<AudioClip> clips;

        [Foldout("Options")] [Range(0f, 1f)]
        public float volume = 1f;

        [Foldout("Options")] [Range(0f, 1f)]
        public float volumeVariance;

        [Foldout("Options")] [Range(.1f, 3f)]
        public float pitch = 1f;

        [Foldout("Options")] [Range(0f, 1f)]
        public float pitchVariance;

        [Foldout("Options")] public bool loop;

        [HideInInspector] public AudioSource source;
    }
}