using DialogueSystem;
using InteractionSystem;
using Player;
using System;
using UnityEngine;

namespace NPC
{
    public class NPCView : Interactable
    {
        [SerializeField] private Transform _focusPoint;
        [SerializeField, Space] private NPC_Config _npcConfig;

        private DialogueModel _dialogueModel;

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
            if (string.IsNullOrEmpty(_npcConfig.NPC_Name))
            {
                Debug.Log("NPC name not assigned. Unable to start dialogue!");
                return;
            }

            if (string.IsNullOrEmpty(_npcConfig.DialogueKey))
            {
                Debug.Log("Dialogue Key not specified. Unable to start dialogue!");
                return;
            }

            if (string.IsNullOrEmpty(_npcConfig.UniqueId))
            {
                Debug.Log("Unique NPC ID not specified. Unable to start dialogue!");
                return;
            }

            base.Interact(playerInteractionPresenter);

            DialogueData dialogueData = new DialogueData(_npcConfig.NPC_Name, _npcConfig.DialogueKey, _focusPoint, _npcConfig.UniqueId);
            DialogueModel.TryStartDialogue.OnNext(dialogueData);
        }

    }

    [Serializable]
    public class NPC_Config
    {
        [field: SerializeField] public string UniqueId { get; private set; }
        [field: SerializeField] public string NPC_Name { get; private set; }
        [field: SerializeField] public string DialogueKey { get; private set; }
    }
}

