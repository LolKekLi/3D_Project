#region

using DG.Tweening;

#endregion

namespace Project
{
    public static class DoTweenExtensions
    {
        public static void Play(this DOTweenAnimation animation)
        {
            animation.tween.Rewind();
            animation.tween.Play();
        }
    }
}