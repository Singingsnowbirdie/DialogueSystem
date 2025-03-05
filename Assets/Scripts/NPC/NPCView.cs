using DialogueSystem;
using InteractionSystem;
using Player;
using UnityEngine;

namespace NPC
{
    public class NPCView : Interactable
    {
        [SerializeField] private string _npcName;
        [SerializeField] private string _dialogueKey;
        [SerializeField] private Transform _focusPoint;
        [SerializeField] private string _uniqueId;

        private DialogueModel _dialogueModel;

        public string UniqueId => _uniqueId;

        public DialogueModel DialogueModel
        {
            get
            {
                if (_dialogueModel == null)
                {
                    NPCCollectionView npcCollectionView = GetComponentInParent<NPCCollectionView>();
                    _dialogueModel = npcCollectionView.DialogueModel;
                }

                return _dialogueModel;
            }
        }

        public override void Interact(PlayerInteractionPresenter playerInteractionPresenter)
        {
            if (string.IsNullOrEmpty(_npcName))
            {
                Debug.Log("NPC name not assigned. Unable to start dialogue!");
                return;
            }

            if (string.IsNullOrEmpty(_dialogueKey))
            {
                Debug.Log("Dialogue Key not specified. Unable to start dialogue!");
                return;
            }

            base.Interact(playerInteractionPresenter);

            DialogueData dialogueData = new DialogueData(_npcName, _dialogueKey, _focusPoint);
            DialogueModel.TryStartDialogue.OnNext(dialogueData);
        }
    }
}
