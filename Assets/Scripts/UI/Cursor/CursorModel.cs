using UniRx;
using UnityEngine;

namespace UI
{
    public class CursorModel
    {
        public ReactiveProperty<bool> IsCursorActive { get; } = new ReactiveProperty<bool>(false);

        public void SetCursorState(bool isActive)
        {
            IsCursorActive.Value = isActive;
            UpdateCursorState();
        }

        private void UpdateCursorState()
        {
            Cursor.visible = IsCursorActive.Value;
            Cursor.lockState = IsCursorActive.Value ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}

