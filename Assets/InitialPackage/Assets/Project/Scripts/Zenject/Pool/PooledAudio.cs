#region

using UnityEngine;

#endregion

namespace Project
{
    [RequireComponent(typeof(AudioSource))]
    public class PooledAudio : PooledBehaviour
    {
        [SerializeField]
        protected AudioSource _source = null;

        public AudioSource Source
        {
            get =>
                _source;
        }

        public virtual void Setup(AudioClip clip, float volume)
        {
            _source.clip = clip;
           
            FreeTimeout = clip.length;
            
            _source.volume = volume;
            _source.Play();
        }
    }
}