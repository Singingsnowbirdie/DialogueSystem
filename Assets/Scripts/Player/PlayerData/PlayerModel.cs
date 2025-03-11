using DialogueSystem.DialogueEditor;
using UniRx;
using UnityEngine;

namespace Player
{
    public class PlayerModel
    {
        public ReactiveProperty<int> ReputationAmount { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<ERace> PlayerRace { get; } = new ReactiveProperty<ERace>();
        public ReactiveProperty<EGender> PlayerGender { get; } = new ReactiveProperty<EGender>();
        public ReactiveProperty<string> PlayerName { get; } = new ReactiveProperty<string>();

        private PlayerRepository _playerRepository;

        public PlayerRepository PlayerRepository
        {
            get
            {
                _playerRepository ??= new PlayerRepository();
                return _playerRepository;
            }
        }

        public void SavePlayerData()
        {
            PlayerRepository.PlayerData playerData = PlayerRepository.LoadPlayerData();
            playerData.Reputation = ReputationAmount.Value;
            PlayerRepository.SavePlayerData(playerData);
        }

        public void LoadPlayerData()
        {
            PlayerRepository.PlayerData playerData = PlayerRepository.LoadPlayerData();

            ReputationAmount.Value = playerData.Reputation;
            PlayerRace.Value = playerData.Race;
            PlayerGender.Value = playerData.Gender;
            PlayerName.Value = playerData.PlayerName;
        }
    }
}


