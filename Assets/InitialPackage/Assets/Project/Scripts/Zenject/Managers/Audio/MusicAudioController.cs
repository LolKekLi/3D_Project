#region

using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Settings;
using UnityEngine;

#endregion

namespace Project
{
    public class MusicAudioController
    {
        private float _targetVolume;
        
        private AudioSource _musicSource;
        private CancellationTokenSource _smoothVolumeToken;
        
        public MusicAudioController(AudioSource musicSource)
        {
            _musicSource = musicSource;
        }

        public async UniTaskVoid Play(SoundSettings.MusicSetup musicSetup, float smoothTime)
        {
            if (_musicSource.clip)
            {
                await Stop(AudioManager.ToggleSoundSmoothTime);
            }

            _musicSource.clip = musicSetup.Clip;
            _targetVolume = musicSetup.Volume;

            EnableSound(smoothTime);
        }
        
        public async UniTask Stop(float smoothTime)
        {
            await DisableSound(smoothTime);
            
            _musicSource.clip = null;
        }

        public async UniTask DisableSound(float smoothTime)
        {
            if (!_musicSource.clip)
            { 
                return;
            }
            
            if (smoothTime > 0)
            {
                await _musicSource.SmoothChangeVolume(smoothTime, 0,
                    UniTaskUtil.RefreshToken(ref _smoothVolumeToken));
            }
            
            _musicSource.Stop();
        }

        public void EnableSound(float smoothTime)
        {
            if (smoothTime <= 0)
            {
                _musicSource.volume = _targetVolume;
            }
            else
            {
                _musicSource.volume = 0;
                _musicSource.SmoothChangeVolume(smoothTime, _targetVolume,
                    UniTaskUtil.RefreshToken(ref _smoothVolumeToken)).Forget();
            }

            _musicSource.Play();
        }
    }
}