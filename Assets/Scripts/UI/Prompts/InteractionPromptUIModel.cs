using UniRx;

namespace UI
{
    public class InteractionPromptUIModel: UIModel
    {
        public ReactiveProperty<string> PromptText = new ReactiveProperty<string>();
    }
}
