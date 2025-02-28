using InteractionSystem;
using Player;
using UnityEngine;

namespace NPC
{
    public class NPCInteractionView : Interactable
    {
        public override void Interact(PlayerInteractionPresenter playerInteractionPresenter)
        {
            base.Interact(playerInteractionPresenter);

            Debug.Log("Interact");
        }
    }
}
