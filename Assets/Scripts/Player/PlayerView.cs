using DialogueSystem.DialogueEditor;
using UnityEngine;
using VContainer;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [Inject] private readonly Camera _playerCamera;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _playerTransform;

        [Header("PLAYER CONFIG")]
        [field: SerializeField] public string PlayerName { get; private set; } = "Player Name";
        [field: SerializeField] public EGender PlayerGender { get; private set; } = EGender.Male;
        [field: SerializeField] public ERace PlayerRace { get; private set; } = ERace.Human;

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
    }
}


