using InteractionSystem;
using Player;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace UI
{
    public class InteractionPromptPresenter : IStartable
    {
        // UI
        [Inject] private InteractionPromptView _view;
        [Inject] private InteractionPromptUIModel _model;

        // Player
        [Inject] private PlayerInteractionModel _playerInteractionModel;

        private readonly string _availableInteractionNotification = "Press \"E\" to interact";

        public void Start()
        {
            _view.OnSetModel(_model);

            _playerInteractionModel.CurrentInteractable
                .Subscribe(val => OnCurrentInteractableUpdated(val))
                .AddTo(_view);
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
