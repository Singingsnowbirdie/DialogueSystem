using Player;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace UI
{
    public class PlayerInfoUIPresenter : IStartable
    {
        [Inject] private readonly PlayerInfoUIModel _uiModel;
        [Inject] private readonly PlayerInfoView _view;
        [Inject] private readonly PlayerModel _playerModel;

        public void Start()
        {
            _view.OnSetModel(_uiModel);

            _uiModel.PlayerGender.Value = $"Player Gender: {_playerModel.PlayerGender.Value}";
            _uiModel.PlayerName.Value = $"Player Name: {_playerModel.PlayerName.Value}";
            _uiModel.PlayerRace.Value = $"Player Race: {_playerModel.PlayerRace.Value}";

            _playerModel.ReputationAmount
                .Subscribe(val => OnReputationAmountUpdated(val))
                .AddTo(_view);

        }
        private void OnReputationAmountUpdated(int val)
        {
            _uiModel.ReputationAmount.Value = $"Player reputation: {val}";
        }
    }
}


