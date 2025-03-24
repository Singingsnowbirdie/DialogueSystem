using InteractionSystem;
using Player;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace UI
{
    public class InteractionPromptPresenter : IStartable
    {
        [Inject] private readonly InteractionPromptView _view;
        [Inject] private readonly InteractionPromptUIModel _model;
        [Inject] private readonly PlayerInteractionModel _playerInteractionModel;

        private readonly string _availableInteractionNotification = "Press \"E\" to interact";

        public void Start()
        {
            _view.OnSetModel(_model);

            _playerInteractionModel.CurrentInteractable
                .Subscribe(val => OnCurrentInteractableUpdated(val))
                .AddTo(_view);

            _playerInteractionModel.IsInteracting
                .Subscribe(val => OnInteracting(val))
                .AddTo(_view);
        }

        private void OnInteracting(bool val)
        {
            if (val)
            {
                _model.PromptText.Value = "";
            }
        }

        private void OnCurrentInteractableUpdated(IInteractable val)
        {
            if (val != null)
                _model.PromptText.Value = _availableInteractionNotification;
            else
                _model.PromptText.Value = "";
        }
    }
}
