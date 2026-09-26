using System.Collections;
using System.Collections.Generic;
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

        static readonly bool IsLog = false;
        static float s_soundVolume;
        static float s_musicVolume;
        static bool s_isAudio;
        static bool s_isMusic;

        [Header("SFX")]
        [SerializeField] [Required] FloatVar sfxVolumeVar;
        [SerializeField] AudioMixerGroup sfxMixerGroup;
        [SerializeField] [Range(0.0f, 1.0f)] float baseSfxVolume = .2f;

        [Header("Music")]
        [SerializeField] AudioMixerGroup musicMixerGroup;
        [SerializeField] [Required] FloatVar musicVolumeVar;
        [SerializeField] SoundSO musics;
        [SerializeField] [Range(0.0f, 1.0f)] float baseMusicVolume = .2f;
        [SerializeField] bool musicAutoPlayStart;
        [SerializeField] bool musicAutoPlayRandom;
        [SerializeField] bool musicAutoPlayNext;
        [SerializeField] float musicFadeOutDuration;

        AudioSource _currentMusic;
        Coroutine _musicFadeOutCoroutine;
        Coroutine _musicWaitNextCoroutine;
        readonly Dictionary<SoundSO, AudioSource> _sources = new Dictionary<SoundSO, AudioSource>();

        protected override void OnAwake()
        {
            // The app's only listener, owned by the persistent audio manager so it survives scene
            // transitions (all game audio is 2D, so its position is irrelevant). Scene cameras carry none.
            gameObject.AddComponent<AudioListener>();
            if (musics != null)
                GetOrCreateAudioSource(musics, true);
        }

        void OnEnable()
        {
            sfxVolumeVar.AddOnChange(OnSfxVolumeChange);
            musicVolumeVar.AddOnChange(OnMusicVolumeChange);
            // Initialize s_isAudio before calling OnSfxVolumeChange to avoid playing sounds on start
            s_isAudio = sfxVolumeVar.v > 0;
            OnSfxVolumeChange();
            OnMusicVolumeChange();
        }

        void OnDisable()
        {
            sfxVolumeVar.RemoveOnChange(OnSfxVolumeChange);
            musicVolumeVar.RemoveOnChange(OnMusicVolumeChange);
        }

        void StartAutoPlayMusic()
        {
            if (musicAutoPlayStart && musics != null)
                PlayMusic(musicAutoPlayRandom ? Random.Range(0, musics.clips.Length) : 0);
        }

        void PlayMusic(int clipId = 0)
        {
            if (musicVolumeVar.v == 0)
                return;
            if (_currentMusic && _currentMusic.isPlaying)
            {
                if (IsLog) Debug.Log($"[Audio] PlayMusic: {_currentMusic.clip.name} is playing. Fading out...");
                if (_musicFadeOutCoroutine != null)
                    StopCoroutine(_musicFadeOutCoroutine);
                _musicFadeOutCoroutine = StartCoroutine(FadeOutAndPlayNextClip(clipId));
                return;
            }

            if (musics == null)
            {
                if (IsLog) Debug.LogWarning("[Audio] PlayMusic: Musics is null!");
                return;
            }

            AudioClip clip = musics.clips[clipId];
            _currentMusic = GetOrCreateAudioSource(musics, true);
            _currentMusic.clip = clip;
            _currentMusic.volume = musics.volume * s_musicVolume;
            _currentMusic.pitch = 1;
            _currentMusic.Play();
            if (IsLog) Debug.Log($"[Audio] PlayMusic: Playing {clip.name}");

            if (musicAutoPlayNext)
            {
                if (_musicWaitNextCoroutine != null)
                    StopCoroutine(_musicWaitNextCoroutine);
                _musicWaitNextCoroutine = StartCoroutine(WaitForEndAndPlayNextClip(clip.length, clipId));
            }
        }

        IEnumerator WaitForEndAndPlayNextClip(float clipLength, int clipId)
        {
            yield return new WaitForSeconds(clipLength);
            PlayMusic(++clipId % musics.clips.Length);
        }

        IEnumerator FadeOutAndPlayNextClip(int nextClipId)
        {
            float elapsed = 0.0f;
            if (IsLog) Debug.Log($"[Audio] Fading Out: {_currentMusic.name}");
            while (elapsed <= musicFadeOutDuration)
            {
                yield return new WaitForEndOfFrame();
                elapsed += Time.deltaTime;
                _currentMusic.volume = (musicFadeOutDuration - elapsed) / musicFadeOutDuration * s_musicVolume;
            }

            _currentMusic.Stop();
            PlayMusic(nextClipId);
        }

        static AudioSource GetOrCreateAudioSource(SoundSO s, bool isMusic = false)
        {
            AudioSM instance = Instance;
            if (instance._sources.TryGetValue(s, out AudioSource source) && source != null)
                return source;

            source = instance.gameObject.AddComponent<AudioSource>();
            if (s.clips == null || s.clips.Length == 0)
                Debug.LogWarning($"[Audio] InitAudioSource: {s.name} has no clips!");
            else
                source.clip = s.clips.GetRandom();
            source.loop = s.loop;
            source.outputAudioMixerGroup = isMusic ? instance.musicMixerGroup : instance.sfxMixerGroup;
            instance._sources[s] = source;
            return source;
        }

        public static void Play(SoundSO s)
        {
            if (!s_isAudio || Instance.sfxVolumeVar.v == 0)
                return;
            if (s == null)
            {
                if (IsLog) Debug.LogWarning("[Audio] Play sound: SoundSO is null!");
                return;
            }

            if (s.clips == null || s.clips.Length == 0)
            {
                Debug.LogWarning($"[Audio] Play sound: {s.name} has no clips!");
                return;
            }

            AudioSource source = GetOrCreateAudioSource(s);
            source.clip = s.clips.GetRandom();
            if (IsLog) Debug.Log($"[Audio] Play sound: {source.clip.name}");
            source.volume = s.volume * (1f + Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * s_soundVolume;
            source.pitch = s.pitch * (1f + Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
            if (s.loop)
                source.Play();
            else
                source.PlayOneShot(source.clip);
        }

        public void Stop(SoundSO s)
        {
            if (s == null)
            {
                if (IsLog) Debug.LogWarning("Stop sound: SoundSO is null!");
                return;
            }

            if (_sources.TryGetValue(s, out AudioSource source) && source != null)
                source.Stop();
        }
        void OnSfxVolumeChange()
        {
            bool isSwitchingToEnabled = sfxVolumeVar.v > 0 && !s_isAudio;
            s_isAudio = sfxVolumeVar.v > 0;
            s_soundVolume = sfxVolumeVar.v * baseSfxVolume;
            if (isSwitchingToEnabled)
            {
                Play(Sounds.AudioEnabled);
            }
        }


        void OnMusicVolumeChange()
        {
            bool isSwitchingToEnabled = musicVolumeVar.v > 0 && !s_isMusic;
            s_isMusic = musicVolumeVar.v > 0;
            s_musicVolume = musicVolumeVar.v * baseMusicVolume;
            if (IsLog) Debug.Log($"[Audio] OnMusicVolumeChange: isSwitchingToEnabled={isSwitchingToEnabled} musicVolumeVar={musicVolumeVar.v} s_isMusic={s_isMusic}");
            if (_currentMusic != null)
            {
                if (s_isMusic)
                    _currentMusic.volume = musics.volume * s_musicVolume;
                else
                {
                    _currentMusic.Stop();
                    _currentMusic = null;
                }
            }
            if (isSwitchingToEnabled)
            {
                StartAutoPlayMusic();
            }
        }
    }
}
