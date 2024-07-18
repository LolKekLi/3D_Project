#region

using UnityEngine.Events;

#endregion

namespace Project.UI
{
    public static class ButtonExtensions
    {
        private static AudioManager _audioManager = null;

        public static void SetManagerInstance(AudioManager manager)
        {
            _audioManager = manager;
        }

        public static void AddListener(this UnityEvent uEvent, UnityAction call, SoundType soundType)
        {
            uEvent.AddListener(call);
            
            uEvent.AddListener(() => _audioManager.PlaySound(soundType));
        }
    }
}