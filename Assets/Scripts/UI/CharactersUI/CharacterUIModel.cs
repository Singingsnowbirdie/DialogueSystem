using UniRx;

namespace UI
{
    public class CharacterUIModel : UIModel
    {
        public ReactiveProperty<string> CharacterName = new ReactiveProperty<string>();
        public ReactiveProperty<string> CharacterID = new ReactiveProperty<string>();
        public ReactiveProperty<string> DialogueID = new ReactiveProperty<string>();
        public ReactiveProperty<string> FriendshipAmount = new ReactiveProperty<string>();
    }
}


