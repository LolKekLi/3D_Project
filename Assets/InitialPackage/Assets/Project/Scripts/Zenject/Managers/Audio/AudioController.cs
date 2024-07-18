#region

using Project.Settings;
using UnityEngine;

#endregion

namespace Project
{
    public class AudioController
    {
        private readonly AudioSource AudioSource;
        private readonly PoolManager PoolManager;

        public AudioController(AudioSource audioSource, PoolManager poolManager)
        {
            PoolManager = poolManager;
            AudioSource = audioSource;
        }

        public void Play(SoundSettings.SoundSetup soundSetup)
        {
            AudioSource.volume = soundSetup.Volume;
            AudioSource.PlayOneShot(soundSetup.Clips.RandomElement());
        }

        public void Play(SoundSettings.SoundSetup soundSetup, Vector3 position)
        {
            var pooledAudio =
                PoolManager.Get<PooledAudio>(PoolManager.PoolSettings.PooledAudio, position, Quaternion.identity);
            pooledAudio.Setup(soundSetup.Clips.RandomElement(), soundSetup.Volume);
        }
    }
}