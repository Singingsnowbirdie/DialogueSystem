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
        [Inject] private readonly PlayerView _view;
        [Inject] private readonly PlayerInput _playerInput;

        public void Start()
        {
            _playerInput.actions["Look"].performed += OnLook;
            _playerInput.actions["Move"].performed += OnMove;
            _playerInput.actions["Move"].canceled += OnMoveCanceled;
            _playerInput.actions["Jump"].performed += OnJump;

            _model.LookRotation
                .Subscribe(_ => _view.UpdateRotation(_model.LookRotation.Value, _model.RotationX.Value))
                .AddTo(_view);

            _model.RotationX
                .Subscribe(_ => _view.UpdateRotation(_model.LookRotation.Value, _model.RotationX.Value))
                .AddTo(_view);

            Observable.EveryUpdate()
                .Subscribe(_ => UpdateMovement())
                .AddTo(_view);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            _model.MoveInput.Value = context.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _model.MoveInput.Value = Vector2.zero;
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (_view.IsGrounded)
                _model.Velocity.Value = new Vector3(_model.Velocity.Value.x, Mathf.Sqrt(_model.JumpHeight * -2f * _model.Gravity), _model.Velocity.Value.z);
        }

        private void UpdateMovement()
        {
            if (_view.IsGrounded && _model.Velocity.Value.y < 0)
                _model.Velocity.Value = new Vector3(_model.Velocity.Value.x, -2f, _model.Velocity.Value.z);
            else
                _model.Velocity.Value += Vector3.up * _model.Gravity * Time.deltaTime;

            Vector3 moveDirection = CalculateMovement(_model.MoveInput.Value);
            _view.Move((moveDirection + _model.Velocity.Value) * Time.deltaTime);
        }

        private Vector3 CalculateMovement(Vector2 input)
        {
            Vector3 forward = _view.CameraTransform.forward;
            Vector3 right = _view.CameraTransform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            return (forward * input.y + right * input.x) * _model.CurrentSpeed.Value;
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


