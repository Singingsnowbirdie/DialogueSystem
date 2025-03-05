using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Player
{
    public class DialogueCameraPresenter : IStartable
    {
        [Inject] private readonly DialogueCameraModel _model;
        [Inject] private readonly DialogueCameraView _view;

        public void Start()
        {
            _view.OnStart();

            _model.NpcFocusPoint
                .Where(focusPoint => focusPoint != null)
                .Subscribe(focusPoint => OnCameraFocus(focusPoint))
                .AddTo(_view);

            _model.IsCameraFocused
                .Where(isFocused => !isFocused)
                .Subscribe(_ => _view.ResetCamera())
                .AddTo(_view);
        }

        private void OnCameraFocus(Transform focusPoint)
        {
            if (focusPoint)
            {
                _view.FocusOnNPC(focusPoint);
                _model.IsCameraFocused.Value = true;
            }
            else
            {
                _model.IsCameraFocused.Value = false;
            }
        }
    }
}


