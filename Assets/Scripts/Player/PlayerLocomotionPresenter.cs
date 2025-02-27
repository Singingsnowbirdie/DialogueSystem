using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Player
{
    public class PlayerLocomotionPresenter : IStartable
    {
        [Inject] private readonly PlayerLocomotionModel _model;
        [Inject] private readonly PlayerLocomotionView _view;
        [Inject] private readonly PlayerInput _playerInput;

        public void Start()
        {
            _playerInput.actions["Look"].performed += OnLook;

            _model.LookRotation.Subscribe(_ => _view.UpdateRotation(_model.LookRotation.Value, _model.RotationX.Value));
            _model.RotationX.Subscribe(_ => _view.UpdateRotation(_model.LookRotation.Value, _model.RotationX.Value));
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _model.RotationX.Value -= input.y * _model.LookSpeedY;
            _model.RotationX.Value = Mathf.Clamp(_model.RotationX.Value, -_model.UpperLookLimit, _model.LowerLookLimit);
            _model.LookRotation.Value = Quaternion.Euler(0, input.x * _model.LookSpeedX, 0);
        }
    }
}


