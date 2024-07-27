using System;
using Project.UI;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    private JoystickController _joystickController;

    [Inject]
    private void Construct(JoystickController joystickController)
    {
        _joystickController = joystickController;
    }

    private void Awake()
    {
        Debug.LogError(_joystickController);
    }
}