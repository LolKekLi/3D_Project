#region

using System.Collections.Generic;
using Project.Settings;
using UnityEngine;

#endregion

namespace Project
{
    public class LoopedAudioController
    {
       
        
        private readonly PoolManager PoolManager = null;
        private readonly Dictionary<SoundType, LoopPoledAudio> LoopPoledAudios
            = new Dictionary<SoundType, LoopPoledAudio>();

        private bool _isMuted;

        public LoopedAudioController(PoolManager poolManager, bool isMuted)
        {
            _isMuted = isMuted;
            PoolManager = poolManager;
        }

        public void Play(SoundSettings.SoundSetup soundSetup, float smoothTime)
        {
            LoopPoledAudio loopPoledAudio = null;

            if (!LoopPoledAudios.ContainsKey(soundSetup.Type))
            {
                LoopPoledAudios.Add(soundSetup.Type, null);
            }

            if (LoopPoledAudios[soundSetup.Type] != null)
            {
                loopPoledAudio = LoopPoledAudios[soundSetup.Type];
            }
            else
            {
                loopPoledAudio = PoolManager.Get<LoopPoledAudio>(PoolManager.PoolSettings.LoopPoledAudio,
                    Vector3.zero, Quaternion.identity);
                loopPoledAudio.Setup(soundSetup.Clips.RandomElement(), soundSetup.Volume);
            }

            loopPoledAudio.Play(smoothTime, _isMuted);
            
            LoopPoledAudios[soundSetup.Type] = loopPoledAudio;
        }

        public void Stop(SoundType type, float smoothTime)
        {
            if (!LoopPoledAudios.ContainsKey(type) || LoopPoledAudios[type] == null)
            {
                return;
            }

            LoopPoledAudios[type].Stop(smoothTime, () =>
            {
                LoopPoledAudios[type].Free();
                LoopPoledAudios[type] = null;
            }).Forget();
        }
        
        public void DisableSound()
        {
            _isMuted = true;

            ToggleSound();
        }
        
        public void EnableSound()
        {
            _isMuted = false;
            
            ToggleSound();
        }
        
        private void ToggleSound()
        {
            foreach (var loopPoledAudio in LoopPoledAudios)
            {
                if (loopPoledAudio.Value)
                {
                    loopPoledAudio.Value.ToggleSound(AudioManager.ToggleSoundSmoothTime, _isMuted);
                }
            }
        }
    }
}