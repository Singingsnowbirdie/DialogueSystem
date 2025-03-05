using Database;
using DialogueSystem.DialogueEditor;
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

        private DialogueHandler _dialogueHandler;
        private readonly CompositeDisposable _compositeDisposables = new CompositeDisposable();

        public void Initialize()
        {
            _dialogueHandler = new DialogueHandler(_dialogueModel, this);

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
                Debug.Log("Current Node is SpeakerNode");

                if (_dialogueModel.IsDialogueStarted == false)
                {
                    _dialogueModel.DialogueUIModel.Value = new DialogueUIModel();
                    _dialogueModel.DialogueUIModel.Value.SpeakerName.Value = _dialogueModel.SpeakerName;
                    _dialogueModel.IsDialogueStarted = true;
                }

                _dialogueModel.DialogueUIModel.Value.DialogueText.Value = ReplacePlayerName(speakerNode.DialogueLine);
            }
        }

        private string ReplacePlayerName(string dialogueLine)
        {
            const string placeholder = "/playerName/";

            if (dialogueLine.Contains(placeholder))
            {
                return dialogueLine.Replace(placeholder, _playerModel.PlayerName);
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
