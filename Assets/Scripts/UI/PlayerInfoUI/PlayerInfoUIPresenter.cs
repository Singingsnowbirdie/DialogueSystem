using DataSystem;
using InventorySystem;
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
        [Inject] private readonly InventoryModel _inventoryModel;

        public void Start()
        {
            _view.OnSetModel(_uiModel);

            _uiModel.PlayerGender.Value = $"Player Gender: {_playerModel.PlayerGender.Value}";
            _uiModel.PlayerName.Value = $"Player Name: {_playerModel.PlayerName.Value}";
            _uiModel.PlayerRace.Value = $"Player Race: {_playerModel.PlayerRace.Value}";

            int coinsAmount = 0;

            if (_inventoryModel.InventoryRepository.TryGetItemByID("Coins", out ItemData itemData))
            {
                coinsAmount = itemData.Quantity;
            }

            _uiModel.CoinsAmount.Value = $"Coins: {coinsAmount}";

            _playerModel.Reputation_OfficialAuthorities
                .Subscribe(val => OnOfficialAuthoritiesReputationAmountUpdated(val))
                .AddTo(_view);

            _playerModel.Reputation_Civilian
                .Subscribe(val => OnCivilianReputationAmountUpdated(val))
                .AddTo(_view);

            _playerModel.Reputation_Bandits
                .Subscribe(val => OnBanditsReputationAmountUpdated(val))
                .AddTo(_view);

        }

        private void OnBanditsReputationAmountUpdated(int val)
        {
            _uiModel.Reputation_Bandit.Value = $"Bandits reputation: {val}";
        }

        private void OnCivilianReputationAmountUpdated(int val)
        {
            _uiModel.Reputation_Civilian.Value = $"Civilian reputation: {val}";
        }

        private void OnOfficialAuthoritiesReputationAmountUpdated(int val)
        {
            _uiModel.Reputation_OfficialAuthorities.Value = $"Official Authorities reputation: {val}";
        }
    }
}


