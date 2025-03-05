using InteractionSystem;
using UniRx;

namespace Player
{
    public class PlayerInteractionModel
    {
        public ReactiveProperty<IInteractable> CurrentInteractable = new ReactiveProperty<IInteractable>();
        public ReactiveProperty<bool> IsInteracting = new ReactiveProperty<bool>(false); 
    }
}