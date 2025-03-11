using UniRx;

namespace UI
{
    public class PlayerInfoUIModel : UIModel
    {
        public ReactiveProperty<string> PlayerRace = new ReactiveProperty<string>();
        public ReactiveProperty<string> PlayerGender = new ReactiveProperty<string>();
        public ReactiveProperty<string> PlayerName = new ReactiveProperty<string>();

        // REPUTATION
        public ReactiveProperty<string> Reputation_OfficialAuthorities = new ReactiveProperty<string>();
        public ReactiveProperty<string> Reputation_Bandit = new ReactiveProperty<string>();
        public ReactiveProperty<string> Reputation_Civilian = new ReactiveProperty<string>();
    }
}


