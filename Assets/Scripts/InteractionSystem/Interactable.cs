using Player;
using UnityEngine;

namespace InteractionSystem
{
    public class Interactable : MonoBehaviour, IInteractable
    {
       private PlayerInteractionPresenter _playerInteractionPresenter;

        public virtual void Interact(PlayerInteractionPresenter playerInteractionPresenter)
        {
            _playerInteractionPresenter = playerInteractionPresenter;
        }

        public void OnInteractionCompleted()
        {
            _playerInteractionPresenter.OnInteractionCompleted();
        }
    }
}
