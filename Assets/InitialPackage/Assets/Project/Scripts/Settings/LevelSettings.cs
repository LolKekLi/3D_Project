#region

using System;
using Project.Meta;
using UnityEngine;
using Zenject;

#endregion

namespace Project.Settings
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Scriptable/LevelSettings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        
#if UNITY_EDITOR
        [SerializeField, Header("Test Group")]
        private bool _isTestSceneEnabled = false;

        [SerializeField, EnabledIf(nameof(_isTestSceneEnabled), true, EnabledIfAttribute.HideMode.Invisible)]
        private string _testSceneName = string.Empty;
#endif
        
        [SerializeField, Header("Main Group")]
        private string _tutorialSceneName = string.Empty;

        [SerializeField]
        private string[] _levels = null;
        
        [SerializeField]
        private string[] _loopedLevels = null;
        
        
        [field: SerializeField]
        public float ResultDelay
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public float FailDelay
        {
            get;
            private set;
        }

        [InjectOptional]
        private ILevelData _levelData;
        
        public string GetScene
        {
            get
            {
#if UNITY_EDITOR
                if (_isTestSceneEnabled)
                {
                    return _testSceneName;
                }
#endif
                int levelIndex = _levelData.LevelIndex;

                if (levelIndex == 0)
                {
                    return _tutorialSceneName;
                }
                else
                {
                    // NOTE: учитываем туториал
                    levelIndex -= 1;
                }

                if (levelIndex < _levels.Length)
                {
                    return _levels[levelIndex % _levels.Length];
                }
                else
                {
                    levelIndex -= _levels.Length;

                    return _loopedLevels[levelIndex % _loopedLevels.Length];
                }
            }
        }
    }
}