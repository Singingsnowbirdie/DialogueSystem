using DialogueSystem.DialogueEditor;
using UniRx;

namespace Player
{
    public class PlayerModel
    {
        public ReactiveProperty<int> Reputation_OfficialAuthorities { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Reputation_Civilian { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Reputation_Bandits { get; } = new ReactiveProperty<int>();
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
            playerData.Reputation_OfficialAuthorities = Reputation_OfficialAuthorities.Value;
            playerData.Reputation_Civilian = Reputation_Civilian.Value;
            playerData.Reputation_Bandits = Reputation_Bandits.Value;
            PlayerRepository.SavePlayerData(playerData);
        }

        public void LoadPlayerData()
        {
            PlayerRepository.PlayerData playerData = PlayerRepository.LoadPlayerData();

            PlayerRace.Value = playerData.Race;
            PlayerGender.Value = playerData.Gender;
            PlayerName.Value = playerData.PlayerName;

            Reputation_OfficialAuthorities.Value = playerData.Reputation_OfficialAuthorities;
            Reputation_Civilian.Value = playerData.Reputation_Civilian;
            Reputation_Bandits.Value = playerData.Reputation_Bandits;
        }

        internal int GetReputation(EFaction faction)
        {
            return faction switch
            {
                EFaction.OfficialAuthorities => Reputation_OfficialAuthorities.Value,
                EFaction.Civilian => Reputation_Civilian.Value,
                EFaction.Bandits => Reputation_Bandits.Value,
                _ => Reputation_Civilian.Value,
            };
        }
    }
}


