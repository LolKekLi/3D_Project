#region

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Meta;
using Project.Settings;
using Project.UI;
using UnityEngine.SceneManagement;
using Zenject;

#endregion

namespace Project
{
    public class LevelFlowController
    {
        public event Action Loaded = null;
        public event Action Preloaded = null;
        public event Action Started = null;
        public event Action<bool> Finished = null;

        [InjectOptional] private UISystem _uiSystem = null;

        [InjectOptional] private ILevelData _levelData;

        private LevelSettings _levelSettings;

        [Inject]
        public void Construct(LevelSettings levelSettings)
        {
            _levelSettings = levelSettings;
        }

        public void Start(Action callback = null)
        {
            Started?.Invoke();

            callback?.Invoke();

            _uiSystem.ShowWindow<GameWindow>();
        }

        public async void Complete(Dictionary<string, object> data = null, Action callback = null)
        {
            Finished?.Invoke(true);

            _levelData.LevelIndex++;

            callback?.Invoke();

            await UniTask.Delay(TimeSpan.FromSeconds(_levelSettings.ResultDelay));

            _uiSystem.ShowWindow<ResultWindow>(data);
        }

        public async void Fail(Action callback = null)
        {
            Finished?.Invoke(false);

            callback?.Invoke();

            await UniTask.Delay(TimeSpan.FromSeconds(_levelSettings.FailDelay));

            _uiSystem.ShowWindow<FailWindow>();
        }

        public async UniTask Load(Action callback = null)
        {
            Preloaded?.Invoke();

            await SceneManager.LoadSceneAsync(_levelSettings.GetScene);

            Loaded?.Invoke();

            callback?.Invoke();

            _uiSystem.ShowWindow<MainWindow>();
        }
    }
}