using ECM.Controllers;
using Project.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class PlayerMovementController : BaseCharacterController
    {
        private JoystickController _joystickController;

        [Inject]
        private void Construct(JoystickController joystickController)
        {
            _joystickController = joystickController;
        }

        protected override void HandleInput()
        {
            if (_joystickController.InputDirection == Vector2.zero)
            {
                base.HandleInput();
            }
            else
            {
                moveDirection = new Vector3
                {
                    x = _joystickController.InputDirection.x,
                    y = 0.0f,
                    z = _joystickController.InputDirection.y
                };
            }
        }
    }
}