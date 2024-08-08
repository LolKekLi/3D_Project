using System.Collections.Generic;
using Project.Scripts;
using Project.UI;
using UnityEngine;
using Zenject;

public abstract class ChooseObject : MonoBehaviour
{
    protected Dictionary<string, object> _choosePopupData;

    private UISystem _uiSystem;

    [Inject]
    private void Construct(UISystem uiSystem)
    {
        _uiSystem = uiSystem;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementController playerMovementController))
        {
            _uiSystem.ShowWindow<ChoosePopup>(GetPopupData());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementController playerMovementController))
        {
            _uiSystem.ShowWindow<GameWindow>();
        }
    }

    public abstract Dictionary<string, object> GetPopupData();
}