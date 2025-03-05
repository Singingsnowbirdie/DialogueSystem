using UniRx;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace Player
{
    public class PlayerLocomotionPresenter : IStartable
    {
        [Inject] private readonly PlayerLocomotionModel _locomotionModel;
        [Inject] private readonly PlayerInteractionModel _interactionModel;
        [Inject] private readonly PlayerView _view;
        [Inject] private readonly PlayerInput _playerInput;

        public void Start()
        {
            _playerInput.actions["Look"].performed += OnLook;
            _playerInput.actions["Move"].performed += OnMove;
            _playerInput.actions["Move"].canceled += OnMoveCanceled;
            _playerInput.actions["Jump"].started += OnJump;

            _locomotionModel.LookRotation
                .Subscribe(_ => _view.UpdateRotation(_locomotionModel.LookRotation.Value, _locomotionModel.RotationX.Value))
                .AddTo(_view);

            _locomotionModel.RotationX
                .Subscribe(_ => _view.UpdateRotation(_locomotionModel.LookRotation.Value, _locomotionModel.RotationX.Value))
                .AddTo(_view);

            Observable.EveryUpdate()
                .Subscribe(_ => UpdateMovement())
                .AddTo(_view);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _interactionModel.IsInteracting
                .Subscribe(val => OnInteracting(val))
                .AddTo(_view);
        }

        private void OnInteracting(bool val)
        {
            _locomotionModel.CanMove = !val;

            if (!_locomotionModel.CanMove)
                _locomotionModel.MoveInput.Value = Vector2.zero;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (_locomotionModel.CanMove)
                _locomotionModel.MoveInput.Value = context.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _locomotionModel.MoveInput.Value = Vector2.zero;
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (_locomotionModel.CanMove && _view.IsGrounded)
                _locomotionModel.Velocity.Value = new Vector3(_locomotionModel.Velocity.Value.x, Mathf.Sqrt(_locomotionModel.JumpHeight * -2f * _locomotionModel.Gravity), _locomotionModel.Velocity.Value.z);
        }

        private void UpdateMovement()
        {
            if (_view.IsGrounded && _locomotionModel.Velocity.Value.y < 0)
                _locomotionModel.Velocity.Value = new Vector3(_locomotionModel.Velocity.Value.x, -2f, _locomotionModel.Velocity.Value.z);
            else
                _locomotionModel.Velocity.Value += Vector3.up * _locomotionModel.Gravity * Time.deltaTime;

            Vector3 moveDirection = CalculateMovement(_locomotionModel.MoveInput.Value);
            _view.Move((moveDirection + _locomotionModel.Velocity.Value) * Time.deltaTime);
        }

        private Vector3 CalculateMovement(Vector2 input)
        {
            Vector3 forward = _view.CameraTransform.forward;
            Vector3 right = _view.CameraTransform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            return (forward * input.y + right * input.x) * _locomotionModel.CurrentSpeed.Value;
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            if (_locomotionModel.CanMove)
            {
                Vector2 input = context.ReadValue<Vector2>();
                _locomotionModel.RotationX.Value -= input.y * _locomotionModel.LookSpeedY;
                _locomotionModel.RotationX.Value = Mathf.Clamp(_locomotionModel.RotationX.Value, -_locomotionModel.UpperLookLimit, _locomotionModel.LowerLookLimit);
                _locomotionModel.LookRotation.Value = Quaternion.Euler(0, input.x * _locomotionModel.LookSpeedX, 0);
            }
        }
    }
}


