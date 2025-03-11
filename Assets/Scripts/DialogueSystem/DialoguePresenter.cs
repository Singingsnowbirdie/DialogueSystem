using Database;
using DialogueSystem.DialogueEditor;
using NPC;
using Player;
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
        [Inject] private readonly NPCManagerModel _nPCManagerModel;

        private DialogueHandler _dialogueHandler;
        private DialogueLocalizationHandler _dialogueLocalizationHandler;

        private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

        public void Initialize()
        {
            _dialogueHandler = new DialogueHandler(_dialogueModel, _nPCManagerModel, _playerModel, this);
            _dialogueLocalizationHandler = new DialogueLocalizationHandler(_dialogueModel);

            _dialogueModel.CurrentNode
                .Subscribe(val => OnCurrentNodeUpdated(val))
                .AddTo(_compositeDisposables);

            _dialogueModel.DialogueUIModel
                .Subscribe(val => _view.OnDialogueUIModelUpdated(val))
                .AddTo(_compositeDisposables);

            _dialogueModel.TryStartDialogue
                .Subscribe(data => TryToStartDialogue(data.SpeakerName, data.DialogueID, data.FocusPoint))
                .AddTo(_compositeDisposables);
        }

        internal void TryToStartDialogue(string speakerName, string dialogueID, Transform focusPoint)
        {
            if (_dialogueDatabase.TryGetDialogueGraph(dialogueID, out DialogueGraph graph))
            {
                _dialogueModel.SpeakerName = speakerName;
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
                _dialogueHandler.HandleStartNode(startNode);
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
