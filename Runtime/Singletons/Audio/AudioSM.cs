using System.Collections;
using AS.Toolbox.ScriptableObjects;
using AS.Toolbox.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace AS.Toolbox.Singletons.Audio
{
    public class AudioSM : SingletonMono<AudioSM>
    {
        [SerializeField] [Required] BoolVar isAudioVar;
        [SerializeField] SoundSO musics;
        [SerializeField] AudioMixerGroup musicMixerGroup;
        [SerializeField] AudioMixerGroup sfxMixerGroup;

        [SerializeField] [Range(0.0f, 1.0f)]
        float soundMasterVolume = 1.0f;

        [SerializeField] [Range(0.0f, 1.0f)]
        float musicMasterVolume = 1.0f;

        [SerializeField] bool musicAutoPlayStart;
        [SerializeField] bool musicAutoPlayRandomClip;
        [SerializeField] bool musicAutoPlayNext;
        [SerializeField] float musicFadeOutDuration;

        AudioSource _currentMusic;

        void OnEnable() => isAudioVar.onChange.Add(SetAudio, this);
        void OnDisable() => isAudioVar.onChange.Remove(SetAudio, this);

        protected override void OnAwake()
        {
            InitAudioSource(musics, true);
            AutoPlayMusic();
        }

        void AutoPlayMusic()
        {
            if (musicAutoPlayStart && (musics != null))
                PlayMusic(musicAutoPlayRandomClip ? Random.Range(0, musics.clips.Length) : 0);
        }

        void PlayMusic(int clipId = 0)
        {
            if ((isAudioVar != null) && !isAudioVar.v)
                return;
            if (_currentMusic && _currentMusic.isPlaying)
            {
                StartCoroutine(FadeOutPlayNextMusic());
                return;
            }

            if (musics == null)
            {
                Debug.LogWarning("Musics is null!");
                return;
            }

            AudioClip clip = musics.clips[clipId];
            _currentMusic = musics.source;
            _currentMusic.clip = clip;
            _currentMusic.volume = musics.volume * musicMasterVolume;
            _currentMusic.pitch = 1;
            _currentMusic.Play();

            //Debug.Log($"Music {name} starts");
            if (musicAutoPlayNext)
                StartCoroutine(MusicAutoPlayNext(clip.length, clipId));
        }

        IEnumerator MusicAutoPlayNext(float clipLength, int clipId)
        {
            yield return new WaitForSeconds(clipLength);
            clipId++;
            PlayMusic(clipId % musics.clips.Length);
        }

        IEnumerator FadeOutPlayNextMusic()
        {
            float elapsed = 0.0f;
            while (elapsed <= musicFadeOutDuration)
            {
                yield return new WaitForEndOfFrame();
                elapsed += Time.deltaTime;
                _currentMusic.volume = (musicFadeOutDuration - elapsed) / musicFadeOutDuration * musicMasterVolume;
            }

            _currentMusic.Stop();
            PlayMusic(musicAutoPlayRandomClip ? Random.Range(0, musics.clips.Length) : 0);
        }

        static void InitAudioSource(SoundSO s, bool isMusic = false)
        {
            s.source = Instance.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clips.Length > 0 ? s.clips.GetRandom() : s.clips[0];
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = isMusic ? Instance.musicMixerGroup : Instance.sfxMixerGroup;
        }

        public static void Play(SoundSO s)
        {
            if ((Instance.isAudioVar != null) && !Instance.isAudioVar.v)
                return;
            if (s == null)
            {
                Debug.LogWarning("Play sound: SoundSO is null!");
                return;
            }

            if (s.source == null)
                InitAudioSource(s);

            s.source.clip = s.clips.Length > 0 ? s.clips.GetRandom() : s.clips[0];
            s.source.volume = s.volume * (1f + Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * Instance.soundMasterVolume;
            s.source.pitch = s.pitch * (1f + Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
            if (s.loop)
                s.source.Play();
            else
                s.source.PlayOneShot(s.source.clip);
        }

        public void Stop(SoundSO s)
        {
            if (s == null)
            {
                Debug.LogWarning("Stop sound: SoundSO is null!");
                return;
            }

            s.source.Stop();
        }

        void SetAudio()
        {
            if (isAudioVar.v)
            {
                Play(SOSounds.AudioEnabled);
                AutoPlayMusic();
            }

            AudioListener.pause = !isAudioVar.v;
        }
    }
}
