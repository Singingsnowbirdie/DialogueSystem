using UniRx;

namespace UI.DialogueUI
{
    public class DialogueUIModel : UIModel
    {
        public ReactiveProperty<string> SpeakerName = new ReactiveProperty<string>();
        public ReactiveProperty<string> DialogueText = new ReactiveProperty<string>();
        public ReactiveCollection<PlayerResponseModel> PlayerResponses = new();
    }
}