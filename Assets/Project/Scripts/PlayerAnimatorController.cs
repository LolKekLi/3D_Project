using Project.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private class AnimatorKeys
        {
            public static readonly int IsWalking = Animator.StringToHash("IsWalking");
            public static readonly int WithWeapon = Animator.StringToHash("WithWeapon");
        }

        [SerializeField] private Animator _animator;
        [SerializeField] private RagDoll _ragDoll;

        private JoystickController _joystickController;

        private bool _withWeapon;

        [Inject]
        private void Construct(JoystickController joystickController)
        {
            _joystickController = joystickController;
        }

        private void Update()
        {
            _animator.SetBool(AnimatorKeys.IsWalking, _joystickController.InputDirection != Vector2.zero);

            if (Input.GetKeyDown(KeyCode.F))
            {
                _withWeapon = !_withWeapon;
                _animator.SetBool(AnimatorKeys.WithWeapon, _withWeapon);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                var animatorEnabled = _animator.enabled;
                _animator.enabled = !animatorEnabled;
                
                if (_animator.enabled)
                {
                    _ragDoll.Disable();
                }
                else
                {
                    _ragDoll.Enable();
                    _ragDoll.AddForce(transform.forward * 100);
                }
               
            }
        }
    }
}