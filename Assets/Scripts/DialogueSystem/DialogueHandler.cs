using DialogueSystem.DialogueEditor;

namespace DialogueSystem
{
    public class DialogueHandler
    {
        private readonly DialogueModel _dialogueModel;
        private readonly DialoguePresenter _dialoguePresenter;

        public DialogueHandler(DialogueModel dialogueModel, DialoguePresenter dialoguePresenter)
        {
            _dialogueModel = dialogueModel;
            _dialoguePresenter = dialoguePresenter;
        }

        public void HandleStartNode(StartNode startNode)
        {
            if (startNode.TryGetConnectedNode(out DialogueNode node))
            {
                if (node is SpeakerNode speakerNode)
                    _dialogueModel.CurrentNode.Value = speakerNode;
                else if (node is ConditionCheckNode conditionCheckNode)
                    HandleSpeakerNodeCondition(conditionCheckNode);
                else
                    _dialoguePresenter.EndDialogue();
            }
            else
                _dialoguePresenter.EndDialogue();
        }

        public void HandleSpeakerNodeCondition(ConditionCheckNode conditionCheckNode)
        {
            switch (conditionCheckNode.Condition)
            {
                case EDialogueCondition.HasMet:
                    break;
                case EDialogueCondition.IsReputationAmount:
                    break;
                case EDialogueCondition.IsGender:
                    break;
                case EDialogueCondition.IsRace:
                    break;
                case EDialogueCondition.IsQuestState:
                    break;
                case EDialogueCondition.HasEnoughItems:
                    break;
                case EDialogueCondition.HasEnoughCoins:
                    break;
                case EDialogueCondition.IsDialogueVariable:
                    break;
            }
        }
    }
}
