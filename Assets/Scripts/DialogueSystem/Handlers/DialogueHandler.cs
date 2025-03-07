using DialogueSystem.DialogueEditor;
using NPC;

namespace DialogueSystem
{
    public class DialogueHandler
    {
        private readonly DialogueModel _dialogueModel;
        private readonly NPCManagerModel _npcManagerModel;
        private readonly DialoguePresenter _dialoguePresenter;

        public DialogueHandler(DialogueModel dialogueModel, NPCManagerModel npcManagerModel, DialoguePresenter dialoguePresenter)
        {
            _dialogueModel = dialogueModel;
            _npcManagerModel = npcManagerModel;
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
                    HandleHasMetCondition(conditionCheckNode.NpcID);
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

        private void HandleHasMetCondition(string npcID)
        {

        }
    }
}
