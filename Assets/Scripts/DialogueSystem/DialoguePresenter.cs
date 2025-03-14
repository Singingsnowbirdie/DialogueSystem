using Characters;
using Database;
using DialogueSystem.DialogueEditor;
using Player;
using QuestSystem;
using UI.DialogueUI;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DialogueSystem
{
    public class DialoguePresenter : IInitializable
    {
        [Inject] private readonly DialogueDatabase _dialogueDatabase;
        [Inject] private readonly DialogueView _view;
        [Inject] private readonly DialogueModel _dialogueModel;
        [Inject] private readonly DialogueCameraModel _dialogueCameraModel;
        [Inject] private readonly PlayerModel _playerModel;
        [Inject] private readonly CharactersModel _nPCManagerModel;
        [Inject] private readonly JournalModel _journalModel;

        private DialogueHandler _dialogueHandler;
        private DialogueLocalizationHandler _dialogueLocalizationHandler;

        private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

        public void Initialize()
        {
            _dialogueHandler = new DialogueHandler(_dialogueModel, _nPCManagerModel, _playerModel, _journalModel, this);
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

            _dialogueModel.LoadVariables();
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

                if (_dialogueLocalizationHandler.TryGetDialogueLine(_dialogueModel.Graph.StartNode.Key, speakerNode.NodeId, out string line))
                {
                    dialogueLine = line;
                }

                _dialogueModel.DialogueUIModel.Value.DialogueText.Value = ReplacePlayerName(dialogueLine);

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
