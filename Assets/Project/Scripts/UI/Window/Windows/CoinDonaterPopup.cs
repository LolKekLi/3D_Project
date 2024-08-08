using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class CoinDonaterPopup : Popup
    {
        public static string GET_FREE_COINS_BUTTON_TEXT_KEY = "GET_FREE_COINS_BUTTON_KEY";
        public static string GET_FREE_COINS_BUTTON_ACTION = "GET_FREE_COINS_BUTTON_ACTION";

        [Space]
        [SerializeField] private Button _getFreeCoinsButton;
        [SerializeField] private TMP_Text _getFreeCoinsButtonText;

        private Action _onGetFreeCoinsButtonClick;

        protected override void Start()
        {
            base.Start();

            _getFreeCoinsButton.onClick.AddListener(OnGetFreeCoinsButtonClick);
        }

        protected override void OnShow()
        {
            base.OnShow();

            _getFreeCoinsButtonText.text = GetDataValue<string>(GET_FREE_COINS_BUTTON_TEXT_KEY);
            _onGetFreeCoinsButtonClick = GetDataValue<Action>(GET_FREE_COINS_BUTTON_ACTION);
        }
        
        private void OnGetFreeCoinsButtonClick()
        {
            _onGetFreeCoinsButtonClick?.Invoke();

            _uiSystem.ReturnToPreviousWindow();
        }
    }
}