using Gameplay.UI.ReactiveViews;
using UnityEngine;

namespace UI
{
    public class PlayerInfoView : UIView
    {
        [SerializeField] private TextMeshProReactiveStringView _playerGenderTF;
        [SerializeField] private TextMeshProReactiveStringView _playerRaceTF;
        [SerializeField] private TextMeshProReactiveStringView _playerNameTF;

        [Header("REPUTATION")]
        [SerializeField] private TextMeshProReactiveStringView _reputation_OA_TF;
        [SerializeField] private TextMeshProReactiveStringView _reputation_C_TF;
        [SerializeField] private TextMeshProReactiveStringView _reputation_B_TF;

        public override void OnSetModel(UIModel uiModel)
        {
            base.OnSetModel(uiModel);

            PlayerInfoUIModel playerInfoUIModel = uiModel as PlayerInfoUIModel;

            _reputation_OA_TF.SetUIModel(playerInfoUIModel.Reputation_OfficialAuthorities);
            _reputation_C_TF.SetUIModel(playerInfoUIModel.Reputation_Civilian);
            _reputation_B_TF.SetUIModel(playerInfoUIModel.Reputation_Bandit);
            _playerGenderTF.SetUIModel(playerInfoUIModel.PlayerGender);
            _playerRaceTF.SetUIModel(playerInfoUIModel.PlayerRace);
            _playerNameTF.SetUIModel(playerInfoUIModel.PlayerName);
        }
    }
}


