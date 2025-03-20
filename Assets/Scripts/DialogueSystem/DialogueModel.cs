using DialogueSystem.DialogueEditor;
using UI.DialogueUI;
using UniRx;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueModel
    {
        public bool IsDialogueStarted { get; set; }
        public string SpeakerName { get; set; }
        public string SpeakerID { get; set; }
        public DialogueGraph Graph { get; set; }
        public DialogueDataWrapper DialogueJsonData { get; set; }
        public ReactiveProperty<DialogueNode> CurrentNode { get; } = new ReactiveProperty<DialogueNode>();
        public ReactiveProperty<DialogueUIModel> DialogueUIModel { get; } = new ReactiveProperty<DialogueUIModel>();
        public ISubject<DialogueData> TryStartDialogue { get; } = new Subject<DialogueData>();
        public ISubject<string> TryStartFighting { get; } = new Subject<string>();
        public ISubject<string> TryStartTrading { get; } = new Subject<string>();

        private DialogueVariablesRepository _dialogueVariablesRepository;

        public DialogueVariablesRepository DialogueVariablesRepository
        {
            get
            {
                _dialogueVariablesRepository ??= new DialogueVariablesRepository();
                return _dialogueVariablesRepository;
            }
        }
    }

    public readonly struct DialogueData
    {
        public Transform FocusPoint { get; }
        public string NPC_ID { get; }
        public string DialogueID { get; }
        public string SpeakerName { get; }

        public DialogueData(string speakerName, string message, Transform focusPoint, string npcId)
        {
            SpeakerName = speakerName;
            DialogueID = message;
            FocusPoint = focusPoint;
            NPC_ID = npcId;
        }
    }
}
