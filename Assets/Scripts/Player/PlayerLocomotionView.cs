using UnityEngine;

namespace Player
{
    public class PlayerLocomotionView : MonoBehaviour
    {
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _playerTransform;

        internal void UpdateRotation(Quaternion rot, float rotationX)
        {
            _playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            _playerTransform.rotation *= rot;
        }
    }
}


