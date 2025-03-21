using Characters;
using Database;
using DialogueSystem.DialogueEditor;
using InventorySystem;
using Player;
using QuestSystem;
using System;
using System.Collections.Generic;
using UI.DialogueUI;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using XNode;

namespace DialogueSystem
{
    public class DialoguePresenter : IInitializable, IDisposable
    {
        [Inject] private readonly DialogueDatabase _dialogueDatabase;
        [Inject] private readonly DialogueView _view;
        [Inject] private readonly DialogueModel _dialogueModel;
        [Inject] private readonly DialogueCameraModel _dialogueCameraModel;
        [Inject] private readonly PlayerModel _playerModel;
        [Inject] private readonly CharactersModel _charactersModel;
        [Inject] private readonly JournalModel _journalModel;
        [Inject] private readonly InventoryModel _inventoryModel;

        private DialogueHandler _dialogueHandler;
        private EventsHandler _eventsHandler;
        private DialogueLocalizationHandler _dialogueLocalizationHandler;

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public void Initialize()
        {
            _dialogueHandler = new DialogueHandler(_dialogueModel, _charactersModel, _playerModel, _journalModel, _inventoryModel, this);
            _eventsHandler = new EventsHandler(_dialogueModel, _charactersModel, _inventoryModel, _journalModel);
            _dialogueLocalizationHandler = new DialogueLocalizationHandler(_dialogueModel);

            _dialogueModel.CurrentNode
                .Subscribe(val => OnCurrentNodeUpdated(val))
                .AddTo(_compositeDisposable);

            _dialogueModel.DialogueUIModel
                .Subscribe(val => _view.OnDialogueUIModelUpdated(val))
                .AddTo(_compositeDisposable);

            _dialogueModel.TryStartDialogue
                .Subscribe(data => TryToStartDialogue(data.SpeakerName, data.DialogueID, data.FocusPoint, data.NPC_ID))
                .AddTo(_compositeDisposable);

            _dialogueModel.TryStartFighting
                .Subscribe(characterID => TryStartFighting(characterID))
                .AddTo(_compositeDisposable);

            _dialogueModel.SetVariableValues
                .Subscribe(data => SetVariableValues(data))
                .AddTo(_compositeDisposable);

            _dialogueModel.TryStartTrading
                .Subscribe(characterID => TryStartTrading(characterID))
                .AddTo(_compositeDisposable);

            _dialogueModel.DialogueVariablesRepository.LoadData();
        }

        private void SetVariableValues(SetVariableData data)
        {
            DialogueVariableData variable = _dialogueModel.DialogueVariablesRepository.GetDialogueVariable(data.VariableID);
            variable.IsTrue = data.IsTrue;
            variable.Amount = data.Amount;
            _dialogueModel.DialogueVariablesRepository.SaveData();
        }

        private void TryStartTrading(string characterID)
        {
            // TODO: We create the "Start Trading" event in CharactersModel and pass the NPC ID.
            // We subscribe to this event in CharactersPresenter and process it.

            Debug.Log("TryStartTrading");

            EndDialogue();
        }

        private void TryStartFighting(string characterID)
        {
            // TODO: We create the "Start Fighting" event in CharactersModel and pass the NPC ID.
            // We subscribe to this event in CharactersPresenter and process it.
            // It is possible that not only the specified NPC but also his allies should start fighting with the character.

            Debug.Log("TryStartFighting");

            EndDialogue();
        }

        internal void TryToStartDialogue(string speakerName, string dialogueID, Transform focusPoint, string npcID)
        {
            if (_dialogueDatabase.TryGetDialogueGraph(dialogueID, out DialogueGraph graph))
            {
                _dialogueModel.SpeakerName = speakerName;
                _dialogueModel.SpeakerID = npcID;
                _dialogueModel.Graph = graph;
                _dialogueModel.CurrentNode.Value = graph.StartNode;
                _dialogueCameraModel.NpcFocusPoint.Value = focusPoint;
            }
        }

        private void OnCurrentNodeUpdated(DialogueNode currentNode)
        {
            if (currentNode == null)
            {
                EndDialogue();
                return;
            }

            if (currentNode is StartNode startNode)
                _dialogueHandler.HandleStartNode(startNode, _dialogueModel.SpeakerID);
            else if (currentNode is SpeakerNode speakerNode)
            {
                if (_dialogueModel.IsDialogueOccurs.Value == false)
                {
                    _dialogueModel.DialogueUIModel.Value = new DialogueUIModel();
                    _dialogueModel.DialogueUIModel.Value.SpeakerName.Value = _dialogueModel.SpeakerName;
                    _dialogueModel.IsDialogueOccurs.Value = true;
                }

                string dialogueLine = speakerNode.DialogueLine;

                if (_dialogueLocalizationHandler.TryGetDialogueLine(_dialogueModel.Graph.StartNode.DialogueId, speakerNode.NodeId, out string line))
                {
                    dialogueLine = line;
                }

                _dialogueModel.DialogueUIModel.Value.DialogueText.Value = ReplacePlayerName(dialogueLine);

                if (speakerNode.TryGetEvents(out List<Node> events))
                {
                    _eventsHandler.HandleEvents(events, _dialogueModel.SpeakerID);
                }
            }
        }

        private string ReplacePlayerName(string dialogueLine)
        {
            const string Placeholder = "/playerName/";

            if (dialogueLine.Contains(Placeholder))
            {
                return dialogueLine.Replace(Placeholder, _playerModel.PlayerName.Value);
            }

            return dialogueLine;
        }

        public void EndDialogue()
        {
            if (_dialogueModel.IsDialogueOccurs.Value == true)
                _dialogueModel.IsDialogueOccurs.Value = false;
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
