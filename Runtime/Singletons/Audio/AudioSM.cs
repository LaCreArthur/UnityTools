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

        protected override void OnAwake() => InitAudioSource(musics, true);

        void OnEnable()
        {
            sfxVolumeVar.AddOnChange(OnSfxVolumeChange);
            musicVolumeVar.AddOnChange(OnMusicVolumeChange);
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
                Debug.Log($"[Audio] PlayMusic: {_currentMusic.clip.name} is playing. Fading out...");
                if (_musicFadeOutCoroutine != null)
                    StopCoroutine(_musicFadeOutCoroutine);
                _musicFadeOutCoroutine = StartCoroutine(FadeOutAndPlayNextClip(clipId));
                return;
            }

            if (musics == null)
            {
                Debug.LogWarning("[Audio] PlayMusic: Musics is null!");
                return;
            }

            AudioClip clip = musics.clips[clipId];
            _currentMusic = musics.source;
            _currentMusic.clip = clip;
            _currentMusic.volume = musics.volume * s_musicVolume;
            _currentMusic.pitch = 1;
            _currentMusic.Play();
            Debug.Log($"[Audio] PlayMusic: Playing {clip.name}");

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
            Debug.Log($"[Audio] Fading Out: {_currentMusic.name}");
            while (elapsed <= musicFadeOutDuration)
            {
                yield return new WaitForEndOfFrame();
                elapsed += Time.deltaTime;
                _currentMusic.volume = (musicFadeOutDuration - elapsed) / musicFadeOutDuration * s_musicVolume;
            }

            _currentMusic.Stop();
            PlayMusic(nextClipId);
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
            if (!s_isAudio || Instance.sfxVolumeVar.v == 0)
                return;
            if (s == null)
            {
                Debug.LogWarning("Play sound: SoundSO is null!");
                return;
            }

            if (s.source == null)
                InitAudioSource(s);

            s.source.clip = s.clips.Length > 0 ? s.clips.GetRandom() : s.clips[0];
            s.source.volume = s.volume * (1f + Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * s_soundVolume;
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

        void OnSfxVolumeChange()
        {
            if (sfxVolumeVar.v > 0 && !s_isAudio)
            {
                Play(Sounds.AudioEnabled);
            }
            s_isAudio = sfxVolumeVar.v > 0;
            s_soundVolume = sfxVolumeVar.v * baseSfxVolume;
        }

        void OnMusicVolumeChange()
        {
            if (musicVolumeVar.v > 0 && !s_isMusic)
            {
                StartAutoPlayMusic();
            }
            s_isMusic = musicVolumeVar.v > 0;
            s_musicVolume = musicVolumeVar.v * baseMusicVolume;
            if (_currentMusic != null)
            {
                if (s_isMusic)
                    _currentMusic.volume = musics.volume * s_musicVolume;
                else
                    _currentMusic = null;
            }
        }
    }
}
