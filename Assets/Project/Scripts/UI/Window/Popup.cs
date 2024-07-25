#region

using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Project.UI
{
    public abstract class Popup : Window
    {
        [SerializeField]
        private Button _backButton = null;
        
        public override bool IsPopup
        {
            get => true;
        }

        protected override void Start()
        {
            base.Start();
            
            if (_backButton != null)
            {
                _backButton.onClick.AddListener(OnBackButtonClicked);
            }
        }
        
        protected virtual void OnBackButtonClicked()
        {
            _uiSystem.ReturnToPreviousWindow();
        }
    }
}