using Characters;
using DialogueSystem.DialogueEditor;
using Player;
using QuestSystem;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogueSystem
{
    public class DialogueHandler
    {
        private readonly DialogueModel _dialogueModel;
        private readonly CharactersModel _npcManagerModel;
        private readonly PlayerModel _playerModel;
        private readonly DialoguePresenter _dialoguePresenter;
        private readonly JournalModel _journalModel;

        private string _speakerID = "";

        public DialogueHandler(DialogueModel dialogueModel, CharactersModel npcManagerModel, 
            PlayerModel playerModel, JournalModel journalModel, 
            DialoguePresenter dialoguePresenter)
        {
            _dialogueModel = dialogueModel;
            _npcManagerModel = npcManagerModel;
            _playerModel = playerModel;
            _dialoguePresenter = dialoguePresenter;
            _journalModel = journalModel;
        }

        public void HandleStartNode(StartNode startNode, string speakerID)
        {
            _speakerID = speakerID;

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
                case EDialogueCondition.IsReputationAmount:
                    HandleReputationAmount(conditionCheckNode);
                    break;
                case EDialogueCondition.IsGender:
                    HandleIsGenderConditionForSpeakerNode(conditionCheckNode);
                    break;
                case EDialogueCondition.IsRace:
                    HandleRaceConditionForSpeakerNode(conditionCheckNode);
                    break;
                case EDialogueCondition.HasMet:
                    HandleHasMetConditionForSpeakerNode(conditionCheckNode);
                    break;
                case EDialogueCondition.IsFriendshipAmount:
                    HandleFriendshipAmountConditionForSpeakerNode(conditionCheckNode);
                    break;
                case EDialogueCondition.IsQuestState:
                    HandleQuestStateConditionForSpeakerNode(conditionCheckNode);
                    break;
                case EDialogueCondition.HasEnoughItems:
                    break;
                case EDialogueCondition.HasEnoughCoins:
                    break;
                case EDialogueCondition.IsDialogueVariable:
                    break;
                case EDialogueCondition.None:
                    break;
            }
        }

        private void HandleQuestStateConditionForSpeakerNode(ConditionCheckNode conditionCheckNode)
        {
            bool metsCondition = false;

            if (string.IsNullOrEmpty(conditionCheckNode.QuestKey))
            {
                Debug.Log("Quest ID for checking status is not specified!");
            }
            else
            {
                QuestData questData = _journalModel.QuestsRepository.GetQuestByID(conditionCheckNode.QuestKey);

                metsCondition = questData.QuestState == conditionCheckNode.QuestState;
            }

            List<Node> connectedNodes = conditionCheckNode.GetBoolConnections(metsCondition);
            HandleNextNodeType(connectedNodes[0]);
        }

        private void HandleReputationAmount(ConditionCheckNode conditionCheckNode)
        {
            int playersAmount = _playerModel.GetReputation(conditionCheckNode.Faction);
            int comparisonAmount = conditionCheckNode.Amount;

            bool metsCondition = Compare(playersAmount, comparisonAmount, conditionCheckNode.ComparisonType);

            List<Node> connectedNodes = conditionCheckNode.GetBoolConnections(metsCondition);
            HandleNextNodeType(connectedNodes[0]);
        }

        private void HandleFriendshipAmountConditionForSpeakerNode(ConditionCheckNode conditionCheckNode)
        {
            CharacterData npcData = GetNpcData(conditionCheckNode);
            int npcAmount = npcData.FriendshipAmount;
            int comparisonAmount = conditionCheckNode.Amount;

            bool metsCondition = Compare(npcAmount, comparisonAmount, conditionCheckNode.ComparisonType);
            List<Node> connectedNodes = conditionCheckNode.GetBoolConnections(metsCondition);
            HandleNextNodeType(connectedNodes[0]);
        }

        private bool Compare(int playersAmount, int comparisonAmount, EComparisonTypes comparisonType)
        {
            return comparisonType switch
            {
                EComparisonTypes.Equals => playersAmount == comparisonAmount,
                EComparisonTypes.More => playersAmount > comparisonAmount,
                EComparisonTypes.Less => playersAmount < comparisonAmount,
                EComparisonTypes.EqualsOrMore => playersAmount >= comparisonAmount,
                EComparisonTypes.EqualsOrLess => playersAmount <= comparisonAmount,
                _ => playersAmount == comparisonAmount,
            };
        }

        private void HandleRaceConditionForSpeakerNode(ConditionCheckNode conditionCheckNode)
        {
            bool metsCondition = false;

            if (_playerModel.PlayerRace.Value == conditionCheckNode.PlayerRace)
                metsCondition = true;

            List<Node> connectedNodes = conditionCheckNode.GetBoolConnections(metsCondition);
            HandleNextNodeType(connectedNodes[0]);
        }

        private void HandleIsGenderConditionForSpeakerNode(ConditionCheckNode conditionCheckNode)
        {
            bool metsCondition = false;

            if (_playerModel.PlayerGender.Value == conditionCheckNode.PlayerGender)
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
            CharacterData npcData = GetNpcData(conditionCheckNode);
            List<Node> connectedNodes = conditionCheckNode.GetBoolConnections(npcData.HasMetPlayer);
            HandleNextNodeType(connectedNodes[0]);
        }

        private CharacterData GetNpcData(ConditionCheckNode conditionCheckNode)
        {
            CharacterData npcData;
            if (conditionCheckNode.IsThisNPC)
                npcData = _npcManagerModel.CharactersRepository.GetCharacterByID(_speakerID);
            else
                npcData = _npcManagerModel.CharactersRepository.GetCharacterByID(conditionCheckNode.NpcID);
            return npcData;
        }
    }
}
