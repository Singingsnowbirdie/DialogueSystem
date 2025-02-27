using UniRx;
using UnityEngine;

namespace Player
{
    public class FirstPersonModel
    {
        public ReactiveProperty<Vector3> Position = new ReactiveProperty<Vector3>();
        public ReactiveProperty<Vector2> LookRotation = new ReactiveProperty<Vector2>();
        public ReactiveProperty<bool> IsGrounded = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> IsSprinting = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> IsCrouching = new ReactiveProperty<bool>(false);
        public ReactiveProperty<float> CurrentSpeed = new ReactiveProperty<float>(5f);

        public float WalkSpeed = 5f;
        public float RunSpeed = 10f;
        public float JumpForce = 5f;
        public float CrouchSpeed = 2.5f;
    }
}


