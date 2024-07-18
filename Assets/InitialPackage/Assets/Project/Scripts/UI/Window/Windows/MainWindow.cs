#region

using UnityEngine;
using UnityEngine.UI;
using Zenject;

#endregion

namespace Project.UI
{
    public class MainWindow : Window
    {
        [SerializeField]
        private Button _startButton = null;
        
        [Inject]
        private LevelFlowController _levelFlowController = null;
        
        
        protected override void Start()
        {
            base.Start();
            
            _startButton.onClick.AddListener(OnStartButtonClicked);
        }
        
        private void OnStartButtonClicked()
        {
            _levelFlowController.Start();
        }
    }
}