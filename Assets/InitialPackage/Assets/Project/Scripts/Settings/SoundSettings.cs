#region

using System;
using System.Linq;
using UnityEngine;

#endregion

namespace Project.Settings
{
    [CreateAssetMenu(menuName = "Scriptable/SoundSettings", fileName = "SoundSettings", order = 0)]
    public class SoundSettings : ScriptableObject
    {
        [Serializable]
        public class SoundSetup
        {
            [field: SerializeField]
            public SoundType Type
            {
                get;
                private set;
            }

            [field: SerializeField, Range(0, 1)]
            public float Volume
            {
                get;
                private set;
            }

            [field: SerializeField]
            public AudioClip[] Clips
            {
                get;
                private set;
            }
        }

        [Serializable]
        public class MusicSetup
        {
            [field: SerializeField]
            public MusicType Type
            {
                get;
                private set;
            }

            [field: SerializeField, Range(0, 1)]
            public float Volume
            {
                get;
                private set;
            }

            [field: SerializeField]
            public AudioClip Clip
            {
                get;
                private set;
            }
        }

        [SerializeField]
        private SoundSetup[] _soundSetups = null;

        [SerializeField]
        private MusicSetup[] _musicSetups = null;

        public SoundSetup GetSoundSetup(SoundType type)
        {
            var soundSetup = _soundSetups.FirstOrDefault(x => x.Type == type);

            if (soundSetup == null)
            {
                DebugSafe.LogError($"{type} sound setups not found");
            }

            return soundSetup;
        }

        public MusicSetup GetMusicSetup(MusicType type)
        {
            var soundSetup = _musicSetups.FirstOrDefault(x => x.Type == type);

            if (soundSetup == null)
            {
                DebugSafe.LogError($"{type} music setups not found");
            }

            return soundSetup;
        }
    }
}