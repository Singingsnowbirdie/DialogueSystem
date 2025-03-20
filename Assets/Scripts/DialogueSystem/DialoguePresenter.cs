using Characters;
using Database;
using DialogueSystem.DialogueEditor;
using InventorySystem;
using NUnit.Framework;
using Player;
using QuestSystem;
using System.Collections.Generic;
using UI.DialogueUI;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using XNode;

namespace DialogueSystem
{
    public class DialoguePresenter : IInitializable
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

        private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

        public void Initialize()
        {
            _dialogueHandler = new DialogueHandler(_dialogueModel, _charactersModel, _playerModel, _journalModel, _inventoryModel, this);
            _eventsHandler = new EventsHandler(_charactersModel, _inventoryModel);
            _dialogueLocalizationHandler = new DialogueLocalizationHandler(_dialogueModel);

            _dialogueModel.CurrentNode
                .Subscribe(val => OnCurrentNodeUpdated(val))
                .AddTo(_compositeDisposables);

            _dialogueModel.DialogueUIModel
                .Subscribe(val => _view.OnDialogueUIModelUpdated(val))
                .AddTo(_compositeDisposables);

            _dialogueModel.TryStartDialogue
                .Subscribe(data => TryToStartDialogue(data.SpeakerName, data.DialogueID, data.FocusPoint, data.NPC_ID))
                .AddTo(_compositeDisposables);

            _dialogueModel.DialogueVariablesRepository.LoadData();
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
                if (_dialogueModel.IsDialogueStarted == false)
                {
                    _dialogueModel.DialogueUIModel.Value = new DialogueUIModel();
                    _dialogueModel.DialogueUIModel.Value.SpeakerName.Value = _dialogueModel.SpeakerName;
                    _dialogueModel.IsDialogueStarted = true;
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
            if (_dialogueModel.IsDialogueStarted == true)
            {
                // stop interaction for Player
                _dialogueModel.IsDialogueStarted = false;
            }
        }
    }
}
