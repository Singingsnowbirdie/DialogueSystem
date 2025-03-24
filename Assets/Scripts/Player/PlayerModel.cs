using DialogueSystem.DialogueEditor;
using UniRx;

namespace Player
{
    public class PlayerModel
    {
        // MAIN
        public ReactiveProperty<ERace> PlayerRace { get; } = new ReactiveProperty<ERace>();
        public ReactiveProperty<EGender> PlayerGender { get; } = new ReactiveProperty<EGender>();
        public ReactiveProperty<string> PlayerName { get; } = new ReactiveProperty<string>();

        // REPUTATION
        public ReactiveProperty<int> Reputation_OfficialAuthorities { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Reputation_Civilian { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Reputation_Bandits { get; } = new ReactiveProperty<int>();
        public ISubject<ReputationData> AddReputation { get; } = new Subject<ReputationData>();

        // OTHER
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
            PlayerRepository.PlayerData.Reputation_OfficialAuthorities = Reputation_OfficialAuthorities.Value;
            PlayerRepository.PlayerData.Reputation_Civilian = Reputation_Civilian.Value;
            PlayerRepository.PlayerData.Reputation_Bandits = Reputation_Bandits.Value;
            PlayerRepository.SavePlayerData(PlayerRepository.PlayerData);
        }

        public void LoadPlayerData()
        {
            PlayerRepository.LoadData();

            PlayerRace.Value = PlayerRepository.PlayerData.Race;
            PlayerGender.Value = PlayerRepository.PlayerData.Gender;
            PlayerName.Value = PlayerRepository.PlayerData.PlayerName;

            Reputation_OfficialAuthorities.Value = PlayerRepository.PlayerData.Reputation_OfficialAuthorities;
            Reputation_Civilian.Value = PlayerRepository.PlayerData.Reputation_Civilian;
            Reputation_Bandits.Value = PlayerRepository.PlayerData.Reputation_Bandits;
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

    public readonly struct ReputationData
    {
        public ReputationData(EFaction faction, int amount)
        {
            Faction = faction;
            Amount = amount;
        }

        public EFaction Faction { get; }
        public int Amount { get; }
    }
}


