#region

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Meta;
using UniRx;
using UnityEngine;
using Zenject;

#endregion

namespace Project.UI
{
    public abstract class Window : MonoBehaviour, IWindow
    {
        public static event Action<Window> Shown = delegate { };
        public static event Action<Window> Hidden = delegate { };
        
        [SerializeField]
        private SelfTweenController _onShowAnimation = null;

        [SerializeField]
        private SelfTweenController _onHideAnimation = null;

        private User _user = null;
        protected UISystem _uiSystem = null;
        
        private UniRxSubscribersContainer _subscribersContainer = new UniRxSubscribersContainer();
        private Transform _transform;

        public Transform Transform
        {
            get =>
                _transform;
        }

        public virtual bool IsPopup
        {
            get => false;
        }
        
        [Inject]
        private void Construct(User user, UISystem uiSystem)
        {
            _user = user;
            _uiSystem = uiSystem;
        }

        protected virtual void OnEnable()
        {
            _subscribersContainer.Subscribe(_user.Coins.Skip(1), User_CurrencyChanged);
        }

        protected virtual void OnDisable()
        {
            _subscribersContainer.FreeSubscribers();
        }

        protected virtual void Start()
        {
           
        }

        void IWindow.Show()
        {
            OnShow();

            Shown(this);

            AfterShown();

            Refresh();
        }

        async UniTask IWindow.Hide(bool isAnimationNeeded, CancellationToken cancellationToken)
        {
            await OnHide(isAnimationNeeded, cancellationToken);

            Hidden(this);
        }

        public virtual void Preload()
        {
            _transform = transform;
            gameObject.SetActive(false);
        }

        protected virtual void OnShow()
        {
            gameObject.SetActive(true);

            if (_onShowAnimation != null)
            {
                _onShowAnimation.Play();
            }
        }

        protected virtual void AfterShown()
        {
        }

        protected virtual void Refresh()
        {
        }

        protected virtual async UniTask OnHide(bool isAnimationNeeded, CancellationToken cancellationToken)
        {
            try
            {
                if (isAnimationNeeded)
                {
                    if (_onHideAnimation != null)
                    {
                        _onHideAnimation.Play();

                        await UniTask.Delay(TimeSpan.FromSeconds(_onHideAnimation.LongestAnimationTime),
                            cancellationToken: cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException e)
            {
            }
            
            gameObject.SetActive(false);
        }

        protected T GetDataValue<T>(string itemKey, T defaultValue = default,
            Dictionary<string, object> forcedData = null)
        {
            Dictionary<string, object> data = _uiSystem.Data;

            if (data == null || data.Count == 0)
            {
                return defaultValue;
            }

            if (!data.TryGetValue(itemKey, out object itemObject))
            {
                return defaultValue;
            }

            if (itemObject is T)
            {
                return (T)itemObject;
            }

            return defaultValue;
        }

        protected void SetDataValue<T>(string itemKey, T value)
        {
            if (!_uiSystem.Data.ContainsKey(itemKey))
            {
                _uiSystem.Data.Add(itemKey, value);
            }
            else
            {
                _uiSystem.Data[itemKey] = value;
            }
        }
        
        private void User_CurrencyChanged(int value)
        {
            Refresh();
        }
    }
}