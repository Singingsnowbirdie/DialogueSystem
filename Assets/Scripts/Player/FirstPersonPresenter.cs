using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Player
{
    public class FirstPersonPresenter : IStartable
    {
        private readonly FirstPersonModel _model;
        private readonly FirstPersonView _view;
        private readonly PlayerInput _playerInput;

        public FirstPersonPresenter(FirstPersonModel model, FirstPersonView view, PlayerInput playerInput)
        {
            _model = model;
            _view = view;
            _playerInput = playerInput;
        }

        public void Start()
        {
            _playerInput.actions["Move"].performed += OnMove;
            _playerInput.actions["Look"].performed += OnLook;
            _playerInput.actions["Jump"].performed += OnJump;
            _playerInput.actions["Sprint"].performed += OnSprint;
            _playerInput.actions["Crouch"].performed += OnCrouch;

            _model.Position.Subscribe(pos => _view.UpdatePosition(pos));
            _model.LookRotation.Subscribe(rot => _view.UpdateRotation(rot));

            _model.IsSprinting.Subscribe(sprinting =>
            {
                _model.CurrentSpeed.Value = sprinting ? _model.RunSpeed : _model.WalkSpeed;
            });

            _model.IsCrouching.Subscribe(crouching =>
            {
                _model.CurrentSpeed.Value = crouching ? _model.CrouchSpeed : _model.WalkSpeed;
            });
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(input.x, 0, input.y) * _model.CurrentSpeed.Value * Time.deltaTime;
            _model.Position.Value += moveDirection;
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _model.LookRotation.Value = new Vector2(input.x, input.y);
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (_model.IsGrounded.Value)
            {
                _model.Position.Value += Vector3.up * _model.JumpForce;
            }
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            _model.IsSprinting.Value = context.ReadValueAsButton();
        }

        private void OnCrouch(InputAction.CallbackContext context)
        {
            _model.IsCrouching.Value = context.ReadValueAsButton();
        }
    }
}


