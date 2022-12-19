using System;
using System.Collections;
using AS.Toolbox.ScriptableObjects;
using AS.Toolbox.Utils;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace AS.Toolbox.Singletons.Audio
{
    public class AudioSM : SingletonMono<AudioSM>
    {
        [SerializeField]
        AudioMixerGroup musicMixerGroup;

        [SerializeField]
        AudioMixerGroup sfxMixerGroup;

        [Header("Sounds")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        float soundVolume = 1.0f;

        [SerializeField]
        Sound[] sounds = Array.Empty<Sound>();

        [Header("Musics")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        float musicVolume = 1.0f;

        [SerializeField]
        Sound[] musics = Array.Empty<Sound>();

        [SerializeField]
        bool musicAutoPlayStart;

        [SerializeField]
        bool musicAutoPlayRandomClip;

        [SerializeField]
        bool musicAutoPlayNext;

        [SerializeField]
        float musicFadeOutDuration;

        [SerializeField]
        AudioManagerSO audioManagerSO;

        [SerializeField]
        BoolVar isAudio;

        AudioSource _currentMusic;

        protected override void OnAwake()
        {
            if (audioManagerSO != null)
                audioManagerSO.audioSingleton = this;
            InitSoundArray(sounds, false);
            InitSoundArray(musics, true);
            AutoPlayMusic();
        }

        void OnEnable()
        {
            if (isAudio != null)
                isAudio.onChange.Add(SetAudio, this);
        }

        void OnDisable()
        {
            if (isAudio != null)
                isAudio.onChange.Remove(SetAudio, this);
        }

        void AutoPlayMusic()
        {
            if (musicAutoPlayStart && musics.Length > 0)
                PlayMusic(musics[0].name, musicAutoPlayRandomClip ? Random.Range(0, musics[0].clips.Count) : 0);
        }

        void PlayMusic(string music, int clipId = 0)
        {
            if (isAudio != null && !isAudio.v)
                return;
            if (_currentMusic && _currentMusic.isPlaying)
            {
                StartCoroutine(FadeOutPlayNextMusic(music));
                return;
            }

            var m = Array.Find(musics, item => item.name == music);
            if (m == null)
            {
                Debug.LogWarning($"Play music: {music} not found!");
                return;
            }

            var clip = m.clips[clipId];
            _currentMusic = m.source;
            _currentMusic.clip = clip;
            _currentMusic.volume = m.volume * musicVolume;
            _currentMusic.pitch = 1;
            _currentMusic.Play();

            //Debug.Log($"Music {name} starts");
            if (musicAutoPlayNext)
                StartCoroutine(MusicAutoPlayNext(clip.length, Array.IndexOf(musics, m), clipId));
        }

        IEnumerator MusicAutoPlayNext(float length, int mIndex, int clipId)
        {
            yield return new WaitForSeconds(length);
            clipId++;
            PlayMusic(musics[mIndex].name, clipId % musics[mIndex].clips.Count);
        }

        IEnumerator FadeOutPlayNextMusic(string nextMusic)
        {
            float elapsed = 0.0f;
            while (elapsed <= musicFadeOutDuration)
            {
                yield return new WaitForEndOfFrame();
                elapsed += Time.deltaTime;
                _currentMusic.volume = (musicFadeOutDuration - elapsed) / musicFadeOutDuration * musicVolume;
            }

            _currentMusic.Stop();
            PlayMusic(nextMusic);
        }

        void InitSoundArray(Sound[] soundArray, bool isMusic)
        {
            if (soundArray == null)
                return;// scenes without audio manager are creating empty one
            foreach (Sound s in soundArray)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clips.Count > 0 ? s.clips.GetRandom() : s.clips[0];
                s.source.loop = s.loop;
                s.source.outputAudioMixerGroup = isMusic ? musicMixerGroup : sfxMixerGroup;
            }
        }

        public void Play(string sound)
        {
            if (isAudio != null && !isAudio.v)
                return;
            Sound s = Array.Find(sounds, item => item.name == sound);
            if (s == null)
            {
                Debug.LogWarning($"Play sound: {sound} not found!");
                return;
            }

            s.source.clip = s.clips.Count > 0 ? s.clips.GetRandom() : s.clips[0];
            s.source.volume = s.volume * (1f + Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * soundVolume;
            s.source.pitch = s.pitch * (1f + Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
            if (s.loop)
                s.source.Play();
            else
                s.source.PlayOneShot(s.source.clip);
        }

        public void Stop(string sound)
        {
            Sound s = Array.Find(sounds, item => item.name == sound);
            if (s == null)
            {
                Debug.LogWarning($"Stop sound: {sound} not found!");
                return;
            }

            s.source.Stop();
        }

        public void SetAudio()
        {
            if (isAudio.v)
            {
                Play("audio");
                AutoPlayMusic();
            }
            AudioListener.pause = !isAudio.v;
        }
    }
}