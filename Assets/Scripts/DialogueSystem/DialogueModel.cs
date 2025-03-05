using DialogueSystem.DialogueEditor;
using UI.DialogueUI;
using UniRx;

namespace DialogueSystem
{
    public class DialogueModel
    {
        public bool IsDialogueStarted { get; set; }
        public string SpeakerName { get; set; }
        public DialogueGraph Graph { get; set; }
        public ReactiveProperty<DialogueNode> CurrentNode { get; } = new ReactiveProperty<DialogueNode>();
        public ReactiveProperty<DialogueUIModel> DialogueUIModel { get; set; } = new ReactiveProperty<DialogueUIModel>();
        public ISubject<DialogueData> TryStartDialogue { get; private set; } = new Subject<DialogueData>();
    }

    public struct DialogueData
    {
        public string SpeakerName;
        public string DialogueID;

        public DialogueData(string speakerName, string message)
        {
            SpeakerName = speakerName;
            DialogueID = message;
        }
    }
}
