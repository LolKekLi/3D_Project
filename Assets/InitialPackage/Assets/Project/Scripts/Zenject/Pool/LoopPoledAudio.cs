#region

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

#endregion

namespace Project
{
    public class LoopPoledAudio : PooledAudio
    {
        private CancellationTokenSource _smoothVolumeToken;
        private float _targetVolume;

        public override void Prepare(PooledObjectType pooledType, Transform parent)
        {
            base.Prepare(pooledType, parent);
            
            _source.loop = true;
        }

        protected override void BeforeReturnToPool()
        {
            base.BeforeReturnToPool();
            
            UniTaskUtil.CancelToken(ref _smoothVolumeToken);
        }

        public override void Setup(AudioClip clip, float targetVolume)
        {
            _source.clip = clip;
            _targetVolume = targetVolume;
        }

        public void Play(float smoothTime, bool isMuted)
        {
            if (smoothTime > 0)
            {
                try
                {
                    var cancellationToken = UniTaskUtil.RefreshToken(ref _smoothVolumeToken);
                    _source.SmoothChangeVolume(smoothTime, 
                        isMuted ? 0 : _targetVolume, cancellationToken).Forget();
                }
                catch (OperationCanceledException e)
                {
                }
            }
            else
            {
                _source.volume = _targetVolume;
            }

            _source.Play();
        }

        public async UniTaskVoid Stop(float smoothTime, Action callBack)
        {
            if (smoothTime > 0)
            {
                try
                {
                    var cancellationToken = UniTaskUtil.RefreshToken(ref _smoothVolumeToken);
                    await _source.SmoothChangeVolume(smoothTime, 0, cancellationToken);

                    callBack.Invoke();
                }
                catch (OperationCanceledException e) { }
            }
            else
            {
                _source.volume = 0;

                callBack.Invoke();
            }
        }

        public void ToggleSound(float smoothTime, bool isMuted)
        {
            var cancellationToken = UniTaskUtil.RefreshToken(ref _smoothVolumeToken);
            _source.SmoothChangeVolume(smoothTime, isMuted ? 0 : _targetVolume, cancellationToken).Forget();
        }
        
    }
}