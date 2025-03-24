using UnityEngine;
using VContainer;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [Inject] private readonly Camera _playerCamera;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _playerTransform;

        public Transform CameraTransform => _playerCamera.transform;

        public bool IsGrounded => _characterController.isGrounded;

        internal void UpdateRotation(Quaternion rot, float rotationX)
        {
            _playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            _playerTransform.rotation *= rot;
        }

        public void Move(Vector3 movement)
        {
            _characterController.Move(movement);
        }

        //private void OnDrawGizmos()
        //{
        //    if (_playerCamera == null)
        //        return;

        //    Gizmos.color = Color.green; 
        //    Vector3 rayOrigin = _playerCamera.transform.position;
        //    Vector3 rayDirection = _playerCamera.transform.forward * 3f;
        //    Gizmos.DrawRay(rayOrigin, rayDirection);
        //}
    }
   
}


