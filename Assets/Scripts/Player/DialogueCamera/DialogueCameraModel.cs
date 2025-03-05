using UniRx;
using UnityEngine;

namespace Player
{
    public class DialogueCameraModel
    {
        public ReactiveProperty<Transform> NpcFocusPoint { get; private set; } = new ReactiveProperty<Transform>();
        public ReactiveProperty<bool> IsCameraFocused { get; private set; } = new ReactiveProperty<bool>(false);
    }
}


