using DialogueSystem.DialogueEditor;
using NPC;
using Player;
using System.Collections.Generic;
using XNode;

namespace DialogueSystem
{
    public class DialogueHandler
    {
        private readonly DialogueModel _dialogueModel;
        private readonly NPCManagerModel _npcManagerModel;
        private readonly PlayerModel _playerModel;
        private readonly DialoguePresenter _dialoguePresenter;

        public DialogueHandler(DialogueModel dialogueModel, NPCManagerModel npcManagerModel, PlayerModel playerModel, DialoguePresenter dialoguePresenter)
        {
            _dialogueModel = dialogueModel;
            _npcManagerModel = npcManagerModel;
            _playerModel = playerModel;
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
                    HandleHasMetConditionForSpeakerNode(conditionCheckNode);
                    break;
                case EDialogueCondition.IsReputationAmount:
                    break;
                case EDialogueCondition.IsGender:
                    HandleIsGenderConditionForSpeakerNode(conditionCheckNode);
                    break;
                case EDialogueCondition.IsRace:
                    HandleRaceConditionForSpeakerNode(conditionCheckNode);
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

        private void HandleRaceConditionForSpeakerNode(ConditionCheckNode conditionCheckNode)
        {
            bool metsCondition = false;

            if (_playerModel.PlayerRace == conditionCheckNode.PlayerRace)
                metsCondition = true;

            List<Node> connectedNodes = conditionCheckNode.GetBoolConnections(metsCondition);
            HandleNextNodeType(connectedNodes[0]);
        }

        private void HandleIsGenderConditionForSpeakerNode(ConditionCheckNode conditionCheckNode)
        {
            bool metsCondition = false;

            if (_playerModel.PlayerGender == conditionCheckNode.PlayerGender)
                metsCondition = true;

            List<Node> connectedNodes = conditionCheckNode.GetBoolConnections(metsCondition);
            HandleNextNodeType(connectedNodes[0]);
        }

        private void HandleNextNodeType(Node node)
        {
            if (node is SpeakerNode speakerNode)
                _dialogueModel.CurrentNode.Value = speakerNode;
            else if (node is ConditionCheckNode nextConditionCheckNode)
                HandleSpeakerNodeCondition(nextConditionCheckNode);
        }

        private void HandleHasMetConditionForSpeakerNode(ConditionCheckNode conditionCheckNode)
        {
            NPCData npcData = _npcManagerModel.NpcDatabase.GetNPCByID(conditionCheckNode.NpcID);
            List<Node> connectedNodes = conditionCheckNode.GetBoolConnections(npcData.HasMetPlayer);
            HandleNextNodeType(connectedNodes[0]);
        }
    }
}
