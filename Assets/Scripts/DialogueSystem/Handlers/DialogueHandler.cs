using Characters;
using DataSystem;
using DialogueSystem.DialogueEditor;
using InventorySystem;
using Player;
using QuestSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        private readonly InventoryModel _inventoryModel;
        private EventsHandler _eventsHandler => _dialoguePresenter.EventsHandler;
        private string _speakerID = "";

        public DialogueHandler(DialogueModel dialogueModel, CharactersModel npcManagerModel,
            PlayerModel playerModel, JournalModel journalModel,
            InventoryModel inventoryModel, DialoguePresenter dialoguePresenter)
        {
            _dialogueModel = dialogueModel;
            _npcManagerModel = npcManagerModel;
            _playerModel = playerModel;
            _dialoguePresenter = dialoguePresenter;
            _journalModel = journalModel;
            _inventoryModel = inventoryModel;
        }

        public void HandleStartNode(StartNode startNode, string speakerID)
        {
            _speakerID = speakerID;

            if (startNode.TryGetConnectedNode(out DialogueNode node))
            {
                if (node is SpeakerNode speakerNode)
                    _dialogueModel.CurrentNode.Value = speakerNode;
                else if (node is ConditionCheckNode conditionCheckNode)
                    HandleConditionCheck(conditionCheckNode);
                else
                    _dialoguePresenter.EndDialogue();
            }
            else
                _dialoguePresenter.EndDialogue();
        }

        public void HandleConditionCheck(ConditionCheckNode conditionCheckNode)
        {
            bool metsCondition = MetsCondition(conditionCheckNode);
            List<Node> connectedNodes = conditionCheckNode.GetBoolConnections(metsCondition);
            HandleNextNodeType(connectedNodes[0]);
        }

        private bool MetsCondition(ConditionCheckNode conditionCheckNode)
        {
            return conditionCheckNode.Condition switch
            {
                EDialogueCondition.IsReputationAmount => IsReputationAmount(conditionCheckNode),
                EDialogueCondition.IsGender => IsGender(conditionCheckNode),
                EDialogueCondition.IsRace => IsRace(conditionCheckNode),
                EDialogueCondition.HasMet => HasMet(conditionCheckNode),
                EDialogueCondition.IsFriendshipAmount => IsFriendshipAmount(conditionCheckNode),
                EDialogueCondition.IsQuestState => IsQuestState(conditionCheckNode),
                EDialogueCondition.HasEnoughItems => HasEnoughItems(conditionCheckNode),
                EDialogueCondition.HasEnoughCoins => HasEnoughCoins(conditionCheckNode),
                EDialogueCondition.IsDialogueVariable => IsDialogueVariable(conditionCheckNode),
                EDialogueCondition.InfluenceAttempt => IsInfluenceSuccessful(conditionCheckNode),
                _ => false,
            };
        }

        private bool IsInfluenceSuccessful(ConditionCheckNode conditionCheckNode)
        {
            if (conditionCheckNode.InfluenceType == EInfluenceType.Persuasion)
                return IsPersuasionSuccessful(_speakerID);
            else
                return IsThreatSuccessful(_speakerID);
        }

        private bool IsThreatSuccessful(string speakerID)
        {
            // TODO: Perform calculations taking into account the player's perks,
            // buffs and the NPC's characteristics.

            return Random.value < 0.5f;
        }

        private bool IsPersuasionSuccessful(string speakerID)
        {
            // TODO: Perform calculations taking into account the player's perks,
            // buffs and the NPC's characteristics ( and his attitude towards the player).

            return Random.value < 0.5f;
        }

        private bool IsDialogueVariable(ConditionCheckNode conditionCheckNode)
        {
            if (string.IsNullOrEmpty(conditionCheckNode.ID))
            {
                Debug.Log("Dialogue Variable ID for checking status is not specified!");
            }
            else if (_dialogueModel.DialogueVariablesRepository.TryGetDialogueVariable(conditionCheckNode.ID, out DialogueVariableData dialogueVariable))
            {
                Debug.Log($"dialogueVariable = {dialogueVariable}; IsTrue = {dialogueVariable.IsTrue}; DialogueVariableType + {conditionCheckNode.DialogueVariableType}");

                if (conditionCheckNode.DialogueVariableType == EDialogueVariableType.Bool)
                    return dialogueVariable.IsTrue;
                else
                {
                    int playersAmount = dialogueVariable.Amount;
                    int comparisonAmount = conditionCheckNode.Amount;

                    return Compare(playersAmount, comparisonAmount, conditionCheckNode.ComparisonType);
                }
            }

            return false;
        }

        private bool IsQuestState(ConditionCheckNode conditionCheckNode)
        {
            bool metsCondition = false;

            if (string.IsNullOrEmpty(conditionCheckNode.ID))
            {
                Debug.Log("Quest ID for checking status is not specified!");
            }
            else
            {
                QuestData questData = _journalModel.QuestsRepository.GetQuestByID(conditionCheckNode.ID);
                metsCondition = questData.QuestState == conditionCheckNode.QuestState;
            }

            return metsCondition;
        }

        private bool IsReputationAmount(ConditionCheckNode conditionCheckNode)
        {
            int playersAmount = _playerModel.GetReputation(conditionCheckNode.Faction);
            int comparisonAmount = conditionCheckNode.Amount;

            return Compare(playersAmount, comparisonAmount, conditionCheckNode.ComparisonType);
        }

        private bool HasEnoughItems(ConditionCheckNode conditionCheckNode)
        {
            bool metsCondition = false;

            if (string.IsNullOrEmpty(conditionCheckNode.ID))
            {
                Debug.Log("Item ID for checking status is not specified!");
            }
            else
            {
                if (_inventoryModel.InventoryRepository.TryGetItemByID(conditionCheckNode.ID, out ItemData item))
                {
                    metsCondition = Compare(item.Quantity, conditionCheckNode.Amount, conditionCheckNode.ComparisonType);
                }
            }

            return metsCondition;
        }

        private bool HasEnoughCoins(ConditionCheckNode conditionCheckNode)
        {
            if (_inventoryModel.InventoryRepository.TryGetItemByID("Coins", out ItemData item))
            {
                return Compare(item.Quantity, conditionCheckNode.Amount, conditionCheckNode.ComparisonType);
            }

            return false;
        }

        private bool IsFriendshipAmount(ConditionCheckNode conditionCheckNode)
        {
            CharacterData npcData = GetNpcData(conditionCheckNode);
            int npcAmount = npcData.FriendshipAmount;
            int comparisonAmount = conditionCheckNode.Amount;
            return Compare(npcAmount, comparisonAmount, conditionCheckNode.ComparisonType);
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

        private bool IsRace(ConditionCheckNode conditionCheckNode)
        {
            bool metsCondition = false;

            if (_playerModel.PlayerRace.Value == conditionCheckNode.PlayerRace)
                metsCondition = true;

            return metsCondition;
        }

        private bool IsGender(ConditionCheckNode conditionCheckNode)
        {
            bool metsCondition = false;

            if (_playerModel.PlayerGender.Value == conditionCheckNode.PlayerGender)
                metsCondition = true;

            return metsCondition;
        }

        private void HandleNextNodeType(Node node)
        {
            if (node is SpeakerNode speakerNode)
                _dialogueModel.CurrentNode.Value = speakerNode;
            else if (node is SpeakerNodeJumper speakerNodeJumper)
            {
                if (speakerNodeJumper.TryGetSpeakerNode(out SpeakerNode nextSpeakerNode))
                {
                    _dialogueModel.CurrentNode.Value = nextSpeakerNode;
                }
            }
            else if (node is ConditionCheckNode nextConditionCheckNode)
                HandleConditionCheck(nextConditionCheckNode);
            else if (node is DialogueEventNode eventNode)
                _eventsHandler.HandleEvent(eventNode);
        }

        private bool HasMet(ConditionCheckNode conditionCheckNode)
        {
            CharacterData npcData = GetNpcData(conditionCheckNode);
            return npcData.HasMetPlayer;
        }

        private CharacterData GetNpcData(ConditionCheckNode conditionCheckNode)
        {
            CharacterData npcData;
            if (conditionCheckNode.IsThisNPC)
                npcData = _npcManagerModel.CharactersRepository.GetCharacterByID(_speakerID);
            else
                npcData = _npcManagerModel.CharactersRepository.GetCharacterByID(conditionCheckNode.ID);
            return npcData;
        }

        internal bool TryGetNextResponses(ConditionCheckNode conditionCheckNode, out List<PlayerResponseNode> resultResponses)
        {
            List<PlayerResponseNode> list = new List<PlayerResponseNode>();
            bool metsCondition = MetsCondition(conditionCheckNode);
            List<Node> connectedNodes = conditionCheckNode.GetBoolConnections(metsCondition);

            foreach (Node node in connectedNodes)
            {
                if (node is PlayerResponseNode responseNode)
                {
                    list.Add(responseNode);
                }

                else if (node is ConditionCheckNode oneMoreConditionCheckNode)
                {
                    if (TryGetNextResponses(conditionCheckNode, out List<PlayerResponseNode> results))
                    {
                        list.AddRange(results);
                    }
                }
            }

            resultResponses = list;
            return list.Count > 0;
        }
    }
}
