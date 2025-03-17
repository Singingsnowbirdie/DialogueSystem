using DialogueSystem;
using InteractionSystem;
using Player;
using System;
using UI;
using UnityEngine;

namespace NPC
{
    public class CharacterView : Interactable
    {
        [SerializeField] private Transform _focusPoint;
        [SerializeField, Space] private NPC_Config _npcConfig;
        [SerializeField, Space] private CharacterUIView _characterUIView;

        public string UniqueId => _npcConfig.NPC_ID;
        public string DialogueKey => _npcConfig.DialogueKey;
        public string CharacterName => _npcConfig.NPC_Name;
        public CharacterModel Model { private get; set; }

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

            if (string.IsNullOrEmpty(_npcConfig.NPC_ID))
            {
                Debug.Log("Unique NPC ID not specified. Unable to start dialogue!");
                return;
            }

            base.Interact(playerInteractionPresenter);

            DialogueData dialogueData = new(_npcConfig.NPC_Name, _npcConfig.DialogueKey, _focusPoint, _npcConfig.NPC_ID);
            Model.TryStartDialogue.OnNext(dialogueData);
        }

        internal void OnSetUIModel(CharacterUIModel characterUIModel)
        {
            _characterUIView.OnSetModel(characterUIModel);
        }

        internal void LookAtCamera(Vector3 cameraPosition)
        {
            _characterUIView.LookAtCamera(cameraPosition);
        }
    }

    [Serializable]
    public class NPC_Config
    {
        [field: SerializeField] public string NPC_ID { get; private set; }
        [field: SerializeField] public string NPC_Name { get; private set; }
        [field: SerializeField] public string DialogueKey { get; private set; }
    }
}

