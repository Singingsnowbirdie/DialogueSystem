using UniRx;
using UnityEngine;

namespace Player
{
    public class PlayerLocomotionModel
    {
        // General
        public bool CanMove = true;

        // Look Related
        public ReactiveProperty<Quaternion> LookRotation = new ReactiveProperty<Quaternion>();
        public ReactiveProperty<float> RotationX = new ReactiveProperty<float>();

        // Movement Related
        public ReactiveProperty<Vector2> MoveInput = new ReactiveProperty<Vector2>();
        public ReactiveProperty<float> CurrentSpeed = new ReactiveProperty<float>(5f);

        // Jump Related
        public ReactiveProperty<Vector3> Velocity = new ReactiveProperty<Vector3>();

        // Look Parameters
        public float LookSpeedX { get; } = 0.5f;
        public float LookSpeedY { get; } = 0.5f;
        public float UpperLookLimit { get; } = 80.0f;
        public float LowerLookLimit { get; } = 80.0f;

        // Jump Parameters
        public float Gravity { get; } = -9.81f;
        public float JumpHeight { get; } = 1.5f;
    }
}


