#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

#endregion

namespace Project.UI
{
    public class UISystem : MonoBehaviour
    {
        private class ShowData
        {
            public IWindow Window { get; }

            public Dictionary<string, object> Data { get; }

            public ShowData(IWindow window, Dictionary<string, object> data)
            {
                Window = window;
                Data = data;
            }
        }

        [SerializeField] private GameObject _windowsContainer = null;

        [SerializeField] private Camera _camera = null;

        private IWindow[] _windows = null;
        private IWindow _current = null;
        private readonly Stack<IWindow> _stack = new Stack<IWindow>();
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
        private readonly Queue<ShowData> _showWindowQueue = new Queue<ShowData>();

        private CancellationTokenSource _dequeueWindowToken;
        private CancellationTokenSource _returnToPreviousWindowToken;

        public IWindow CurrentWindow
        {
            get => _current;
        }

        public Camera Camera
        {
            get => _camera;
        }

        public Dictionary<string, object> Data
        {
            get => _data;
        }

        private void Awake()
        {
            _windows = _windowsContainer.GetComponentsInChildren<IWindow>(true);
            _windows.Do(wind => wind.Preload());

            DontDestroyOnLoad(gameObject);
        }

        private async void DequeueWindowAsync(CancellationToken cancellationToken)
        {
            while (_showWindowQueue.Count > 0)
            {
                var window = _showWindowQueue.Dequeue();

                await ShowWindow(window, cancellationToken);
            }

            _dequeueWindowToken = null;
        }

        public void ShowWindow<T>(Dictionary<string, object> data = null)
            where T : Window
        {
            var window = GetWindow<T>();

            _showWindowQueue.Enqueue(new ShowData(window, data));

            if (_showWindowQueue.Count > 0 && _dequeueWindowToken == null)
            {
                DequeueWindowAsync(UniTaskUtil.RefreshToken(ref _dequeueWindowToken));
            }
        }

        private async UniTask ShowWindow(ShowData showData, CancellationToken cancellationToken)
        {
            if (_current == showData.Window)
            {
                return;
            }

            SetData(showData.Data);

            if (!showData.Window.IsPopup)
            {
                foreach (var wnd in _stack)
                {
                    await wnd.Hide(false, cancellationToken);
                }

                _stack.Clear();
            }
            else
            {
                showData.Window.Transform.SetAsLastSibling();
            }

            _stack.Push(showData.Window);

            showData.Window.Show();

            _current = showData.Window;
        }

        public async void ReturnToPreviousWindow()
        {
            if (_stack.Count > 0)
            {
                var window = _stack.Pop();
                _current = _stack.Peek();
    
                await window.Hide(true, UniTaskUtil.RefreshToken(ref _returnToPreviousWindowToken));
            }
        }

        private IWindow GetWindow<T>()
            where T : IWindow
        {
            var window = _windows.FirstOrDefault(w => w is T);

            if (window == null)
            {
                DebugSafe.LogException(new Exception($"{typeof(T)} not found!"));
            }

            return window;
        }

        private void SetData(Dictionary<string, object> data)
        {
            if (data != null)
            {
                foreach (var record in data)
                {
                    if (_data.ContainsKey(record.Key))
                    {
                        _data[record.Key] = record.Value;
                    }
                    else
                    {
                        _data.Add(record.Key, record.Value);
                    }
                }
            }
        }

        [Conditional("FORCE_DEBUG")]
        public void ToggleUI(bool isActive)
        {
            foreach (var wnd in _windows)
            {
                if (!wnd.Transform.TryGetComponent(out CanvasGroup canvasGroup))
                {
                    canvasGroup = wnd.Transform.gameObject.AddComponent<CanvasGroup>();
                }

                canvasGroup.alpha = isActive ? 1 : 0;
            }
        }
    }
}