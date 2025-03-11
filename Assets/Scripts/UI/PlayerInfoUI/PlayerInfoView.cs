using Gameplay.UI.ReactiveViews;
using UnityEngine;

namespace UI
{
    public class PlayerInfoView : UIView
    {
        [SerializeField] private TextMeshProReactiveStringView _reputationTF;
        [SerializeField] private TextMeshProReactiveStringView _playerGenderTF;
        [SerializeField] private TextMeshProReactiveStringView _playerRaceTF;
        [SerializeField] private TextMeshProReactiveStringView _playerNameTF;

        public override void OnSetModel(UIModel uiModel)
        {
            base.OnSetModel(uiModel);

            PlayerInfoUIModel playerInfoUIModel = uiModel as PlayerInfoUIModel;

            _reputationTF.SetUIModel(playerInfoUIModel.ReputationAmount);
            _playerGenderTF.SetUIModel(playerInfoUIModel.PlayerGender);
            _playerRaceTF.SetUIModel(playerInfoUIModel.PlayerRace);
            _playerNameTF.SetUIModel(playerInfoUIModel.PlayerName);
        }
    }
}


