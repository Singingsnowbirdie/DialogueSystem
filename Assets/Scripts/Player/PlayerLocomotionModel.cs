using UniRx;
using UnityEngine;

namespace Player
{
    public class PlayerLocomotionModel
    {
        public ReactiveProperty<Quaternion> LookRotation = new ReactiveProperty<Quaternion>();
        public ReactiveProperty<float> RotationX = new ReactiveProperty<float>();

        [Header("Look Parameters")]
        public float LookSpeedX { get; } = 2.0f;
        public float LookSpeedY { get; } = 2.0f;
        public float UpperLookLimit { get; } = 80.0f;
        public float LowerLookLimit { get; } = 80.0f;
    }
}


