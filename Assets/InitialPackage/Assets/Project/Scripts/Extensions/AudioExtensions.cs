#region

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

#endregion

namespace Project
{
    public static class AudioExtensions
    {
        public static async UniTask SmoothChangeVolume(this AudioSource source, float smoothTime, float targetVolume,
            CancellationToken cancellationToken)
        {
            try
            {
                var startVolume = source.volume;

                await UniTaskExtensions.Lerp(time =>
                    {
                        source.volume = Mathf.Lerp(startVolume, targetVolume, time);
                    },
                    smoothTime, token: cancellationToken);
            }
            catch (OperationCanceledException e) { }
        }
    }
}