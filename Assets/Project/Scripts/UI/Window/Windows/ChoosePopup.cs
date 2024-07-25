using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class ChoosePopup : Popup
    {
        public static string CHOOSE_BUTTON_TEXT_KEY = "CHOOSE_BUTTON_TEXT_KEY";
        public static string CHOOSE_BUTTON_ACTION = "CHOOSE_BUTTON_ACTION";

        [Space]
        [SerializeField] private Button _chooseButton;
        [SerializeField] private TMP_Text _chooseButtonText;

        private Action _onChooseButtonClick;

        protected override void Start()
        {
            base.Start();

            _chooseButton.onClick.AddListener(OnChooseButtonClick);
        }

        protected override void OnShow()
        {
            base.OnShow();

            _chooseButtonText.text = GetDataValue<string>(CHOOSE_BUTTON_TEXT_KEY);
            _onChooseButtonClick = GetDataValue<Action>(CHOOSE_BUTTON_ACTION);
        }

        private void OnChooseButtonClick()
        {
            _onChooseButtonClick?.Invoke();
            
            _uiSystem.ReturnToPreviousWindow();
        }
    }
}