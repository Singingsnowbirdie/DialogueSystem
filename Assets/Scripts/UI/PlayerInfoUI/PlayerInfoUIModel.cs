using UniRx;

namespace UI
{
    public class PlayerInfoUIModel : UIModel
    {
        public ReactiveProperty<string> ReputationAmount = new ReactiveProperty<string>();
        public ReactiveProperty<string> PlayerRace = new ReactiveProperty<string>();
        public ReactiveProperty<string> PlayerGender = new ReactiveProperty<string>();
        public ReactiveProperty<string> PlayerName = new ReactiveProperty<string>();
    }
}


