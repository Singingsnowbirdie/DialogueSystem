using UnityEngine;

namespace Player
{
    public class FirstPersonView : MonoBehaviour
    {
        public Camera PlayerCamera;
        public CharacterController CharacterController;

        public void UpdatePosition(Vector3 position)
        {
            CharacterController.Move(position - transform.position);
        }

        public void UpdateRotation(Vector2 rotation)
        {
            transform.Rotate(Vector3.up, rotation.x);
            PlayerCamera.transform.Rotate(Vector3.right, -rotation.y);
        }
    }
}


