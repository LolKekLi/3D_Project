﻿#region

using UnityEngine;
using UnityEngine.UI;
using Zenject;

#endregion

namespace Project.UI
{
    public class ResultWindow : Window
    {
        private static readonly string ReceivedCoinsKey = "ReceivedCoinsKey";

        [SerializeField]
        private Button _continueButton = null;

        [Inject]
        private LevelFlowController _levelFlowController = null;
        
        protected override void Start()
        {
            base.Start();
            
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }
        
        protected override void OnShow()
        {
            base.OnShow();

            int receivedCoins = GetDataValue<int>(ReceivedCoinsKey, 0);   
        }
        
        private void OnContinueButtonClicked()
        {
            _levelFlowController.Load();
        }
    }
}