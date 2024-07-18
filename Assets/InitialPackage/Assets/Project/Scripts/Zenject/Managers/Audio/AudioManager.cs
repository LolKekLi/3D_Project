#region

using Cysharp.Threading.Tasks;
using Project.Settings;
using Project.UI;
using UnityEngine;
using Zenject;

#endregion

namespace Project
{
    public class AudioManager : ZenjectManager<AudioManager>
    {
        public static float ToggleSoundSmoothTime = 0.1f;
        
        [SerializeField]
        private AudioSource _oneShotDudioSource = null;
        [SerializeField]
        private AudioSource _musicAudioSource = null;

        [Inject]
        private PoolManager _poolManager = null;

        [Inject]
        private SoundSettings _soundSettings;
        
        private AudioController _audioController;
        private LoopedAudioController _loopedAudioController;
        private MusicAudioController _musicAudioController;

        public bool IsSoundMuted
        {
            get => LocalConfig.IsSoundMuted;
            set
            {
                if (value)
                {
                    _loopedAudioController.DisableSound();
                }
                else
                {
                    _loopedAudioController.EnableSound();
                }

                LocalConfig.IsSoundMuted = value;
            }
        }

        public bool IsMusicMuted
        {
            get =>
                LocalConfig.IsMusicMuted;
            set
            {
                {
                    if (value)
                    {
                        _musicAudioController.DisableSound(ToggleSoundSmoothTime).Forget();
                    }
                    else
                    {
                        _musicAudioController.EnableSound(ToggleSoundSmoothTime);
                    }

                    LocalConfig.IsMusicMuted = value;
                }
            }
        }

        protected override void Init()
        {
            base.Init();
            
            _audioController = new AudioController(_oneShotDudioSource, _poolManager);
            _loopedAudioController = new LoopedAudioController(_poolManager, IsSoundMuted);
            _musicAudioController = new MusicAudioController(_musicAudioSource);

            ButtonExtensions.SetManagerInstance(this);
        }

        public void PlayMusic(MusicType type, float smoothTime = 0)
        {
            var musicSetup = _soundSettings.GetMusicSetup(type);
            _musicAudioController.Play(musicSetup, smoothTime).Forget();
        }

        public void StopMusic(float smoothTime = 0)
        {
            _musicAudioController.Stop(smoothTime);
        }

        public void PlaySound(SoundType type)
        {
            if (IsSoundMuted)
            {
                return;
            }
            
            var soundSetup = _soundSettings.GetSoundSetup(type);
            _audioController.Play(soundSetup);
        }

        public void PlaySound(SoundType type, Vector3 position)
        {
            if (IsSoundMuted)
            {
                return;
            }
            
            var soundSetup = _soundSettings.GetSoundSetup(type);
            _audioController.Play(soundSetup, position);
        }

        public void PlayLoopSound(SoundType type, float smoothTime = 0)
        {
            if (IsSoundMuted)
            {
                var soundSetup = _soundSettings.GetSoundSetup(type);
                _loopedAudioController.Play(soundSetup, smoothTime);
            }
        }

        public void StopLoopSound(SoundType type, float smoothTime = 0)
        {
            if (IsSoundMuted)
            {
                _loopedAudioController.Stop(type, smoothTime);
            }
        }
    }
}