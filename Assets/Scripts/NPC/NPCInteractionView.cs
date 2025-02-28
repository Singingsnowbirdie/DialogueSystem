using InteractionSystem;
using Player;
using UnityEngine;

namespace NPC
{
    public class NPCInteractionView : Interactable
    {
        [SerializeField] private string _npcName;
        [SerializeField] private string _dialogueID;

        public override void Interact(PlayerInteractionPresenter playerInteractionPresenter)
        {
            if (string.IsNullOrEmpty(_npcName))
            {
                Debug.Log("NPC name not assigned. Unable to start dialogue!");
                return;
            }

            if (string.IsNullOrEmpty(_npcName))
            {
                Debug.Log("Dialogue ID not specified. Unable to start dialogue!");
                return;
            }

            base.Interact(playerInteractionPresenter);

            Debug.Log("Interact");
        }
    }
}
