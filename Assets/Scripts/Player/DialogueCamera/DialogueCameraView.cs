using System;
using UniRx;
using UnityEngine;

namespace Player
{
    public class DialogueCameraView : MonoBehaviour
    {
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _focusSpeed = 5f;

        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        private IDisposable _focusingListener;

        private float PositionThreshold = 0.1f;
        private float RotationThreshold = 1f;

        internal void OnStart()
        {
            _originalPosition = _playerCamera.transform.position;
            _originalRotation = _playerCamera.transform.rotation;
        }

        public void FocusOnNPC(Transform focusPoint)
        {
            _focusingListener = Observable.EveryUpdate()
                  .Subscribe(_ => UpdateCameraFocus(focusPoint))
                  .AddTo(this);
        }

        private void UpdateCameraFocus(Transform focusPoint)
        {
            _playerCamera.transform.SetPositionAndRotation(
                Vector3.Lerp(_playerCamera.transform.position, focusPoint.position, _focusSpeed * Time.deltaTime),
                Quaternion.Slerp(_playerCamera.transform.rotation, Quaternion.LookRotation(focusPoint.forward), _focusSpeed * Time.deltaTime
            ));

            if (IsCloseEnough(focusPoint.position, Quaternion.LookRotation(focusPoint.forward)))
            {
                _focusingListener.Dispose();
            }
        }

        private bool IsCloseEnough(Vector3 targetPosition, Quaternion targetRotation)
        {
            bool isPositionClose = Vector3.Distance(_playerCamera.transform.position, targetPosition) <= PositionThreshold;
            bool isRotationClose = Quaternion.Angle(_playerCamera.transform.rotation, targetRotation) <= RotationThreshold;

            return isPositionClose && isRotationClose;
        }

        public void ResetCamera()
        {
            _focusingListener?.Dispose();
            _playerCamera.transform.SetPositionAndRotation(_originalPosition, _originalRotation);
        }
    }
}


