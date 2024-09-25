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
        [SerializeField] [Required] FloatVar audioVolumeVar;
        [SerializeField] [Required] FloatVar musicVolumeVar;
        [SerializeField] SoundSO musics;
        [SerializeField] AudioMixerGroup musicMixerGroup;
        [SerializeField] AudioMixerGroup sfxMixerGroup;

        [SerializeField] [Range(0.0f, 1.0f)] float baseSoundVolume = .2f;
        [SerializeField] [Range(0.0f, 1.0f)] float baseMusicVolume = .2f;

        [SerializeField] bool musicAutoPlayStart;
        [SerializeField] bool musicAutoPlayRandomClip;
        [SerializeField] bool musicAutoPlayNext;
        [SerializeField] float musicFadeOutDuration;

        AudioSource _currentMusic;
        float _soundVolume;
        float _musicVolume;
        bool _isAudio;
        bool _isMusic;

        protected override void OnAwake()
        {
            InitAudioSource(musics, true);
            AutoPlayMusic();
        }

        void OnEnable()
        {
            audioVolumeVar.AddOnChange(SetAudio);
            musicVolumeVar.AddOnChange(SetMusic);
        }

        void AutoPlayMusic()
        {
            if (musicAutoPlayStart && (musics != null))
                PlayMusic(musicAutoPlayRandomClip ? Random.Range(0, musics.clips.Length) : 0);
        }

        void PlayMusic(int clipId = 0)
        {
            if (audioVolumeVar.v == 0)
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

            var clip = musics.clips[clipId];
            _currentMusic = musics.source;
            _currentMusic.clip = clip;
            _currentMusic.volume = musics.volume * _musicVolume;
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
                _currentMusic.volume = (musicFadeOutDuration - elapsed) / musicFadeOutDuration * _musicVolume;
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
            if (!Instance._isAudio)
                return;
            if (s == null)
            {
                Debug.LogWarning("Play sound: SoundSO is null!");
                return;
            }

            if (s.source == null)
                InitAudioSource(s);

            s.source.clip = s.clips.Length > 0 ? s.clips.GetRandom() : s.clips[0];
            s.source.volume = s.volume * (1f + Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * Instance._soundVolume;
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
            if (audioVolumeVar == null)
            {
                Debug.LogWarning("SetAudio: AudioVolumeVar is null!", this);
                return;
            }
            if ((audioVolumeVar.v > 0) && !_isAudio)
            {
                Play(SOSounds.AudioEnabled);
            }
            _isAudio = audioVolumeVar.v > 0;
            Debug.Log($"is audio:{_isAudio}, volume:{audioVolumeVar.v}");
            _soundVolume = audioVolumeVar.v * baseSoundVolume;
        }

        void SetMusic()
        {
            if (musicVolumeVar == null)
            {
                Debug.LogWarning("SetMusic: MusicVolumeVar is null!", this);
                return;
            }
            if ((musicVolumeVar.v > 0) && !_isMusic)
            {
                AutoPlayMusic();
            }
            _isMusic = musicVolumeVar.v > 0;
            Debug.Log($"is music:{_isMusic}, volume:{musicVolumeVar.v}");
            _musicVolume = musicVolumeVar.v * baseMusicVolume;
        }
    }
}
