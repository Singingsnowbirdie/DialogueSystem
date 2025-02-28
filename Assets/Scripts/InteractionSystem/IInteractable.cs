using Player;

namespace InteractionSystem
{
    public interface IInteractable
    {
        void Interact(PlayerInteractionPresenter playerInteractionPresenter);

        void OnInteractionCompleted();
    }
}
