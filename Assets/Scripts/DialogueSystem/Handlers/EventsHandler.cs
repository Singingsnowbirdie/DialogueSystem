using Characters;
using DialogueSystem.DialogueEditor;
using InventorySystem;
using Player;
using QuestSystem;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogueSystem
{
    public class EventsHandler
    {
        private readonly CharactersModel _charactersModel;
        private readonly InventoryModel _inventoryModel;
        private readonly JournalModel _journalModel;
        private readonly PlayerModel _playerModel;
        private readonly DialoguePresenter _dialoguePresenter;
        private readonly DialogueModel _dialogueModel;
        private DialogueHandler _dialogueHandler => _dialoguePresenter.DialogueHandler;

        private string _speakerID = "";

        public EventsHandler(DialogueModel dialogueModel, CharactersModel charactersModel,
            InventoryModel inventoryModel, JournalModel journalModel, PlayerModel playerModel, DialoguePresenter dialoguePresenter)
        {
            _dialogueModel = dialogueModel;
            _charactersModel = charactersModel;
            _inventoryModel = inventoryModel;
            _journalModel = journalModel;
            _playerModel = playerModel;
            _dialoguePresenter = dialoguePresenter;
        }

        internal void HandleEvents(List<Node> events, string speakerID)
        {
            _speakerID = speakerID;

            foreach (Node node in events)
            {
                if (node is DialogueEventNode eventNode)
                {
                    HandleEvent(eventNode);
                }
                else if (node is ConditionCheckNode conditionCheckNode)
                {
                    _dialogueHandler.HandleConditionCheck(conditionCheckNode);
                }
            }
        }

        // HANDLE EVENT
        public void HandleEvent(DialogueEventNode eventNode)
        {
            switch (eventNode.EventType)
            {
                case EDialogueEventType.HasMetEvent:
                    HandleHasMetEvent(eventNode);
                    break;
                case EDialogueEventType.AddFriendship:
                    HandleAddFriendshipEvent(eventNode);
                    break;
                case EDialogueEventType.GiveTakeItemEvent:
                    HandleGiveTakeItemEvent(eventNode);
                    break;
                case EDialogueEventType.GiveTakeCoinsEvent:
                    HandleGiveTakeCoinsEvent(eventNode);
                    break;
                case EDialogueEventType.SetQuestState:
                    HandleSetQuestStateEvent(eventNode);
                    break;
                case EDialogueEventType.StartTrading:
                    HandleStartTradingEvent();
                    break;
                case EDialogueEventType.StartFighting:
                    HandleStartFightingEvent();
                    break;
                case EDialogueEventType.SetDialogueVariable:
                    HandleSetDialogueVariableEvent(eventNode);
                    break;
                case EDialogueEventType.AddReputation:
                    HandleAddReputationEvent(eventNode);
                    break;
                case EDialogueEventType.PlayAnimation:
                    HandlePlayAnimationEvent(eventNode);
                    break;
                case EDialogueEventType.PlaySound:
                    HandlePlaySoundEvent(eventNode);
                    break;
            }
        }

        private void HandleAddReputationEvent(DialogueEventNode eventNode)
        {
            ReputationData reputationData = new ReputationData(eventNode.Faction, eventNode.Amount);
            _playerModel.AddReputation.OnNext(reputationData);
        }

        private void HandleSetDialogueVariableEvent(DialogueEventNode eventNode)
        {
            SetVariableData data = new SetVariableData(eventNode.ID, eventNode.IsTrue, eventNode.Amount);
            _dialogueModel.SetVariableValues.OnNext(data);
        }

        private void HandlePlaySoundEvent(DialogueEventNode eventNode)
        {
            // Add logic to play the corresponding audio while displaying the text spoken by the NPC here.
        }

        private void HandlePlayAnimationEvent(DialogueEventNode eventNode)
        {
            // This event is designed to control NPC animations during dialogue. Add logic to call animations here.
        }

        private void HandleSetQuestStateEvent(DialogueEventNode eventNode)
        {
            QuestStateData data = new QuestStateData(eventNode.ID, eventNode.QuestState);
            _journalModel.SetQuestState.OnNext(data);
        }

        private void HandleStartTradingEvent()
        {
            _dialogueModel.TryStartTrading.OnNext(_speakerID);
        }

        private void HandleStartFightingEvent()
        {
            _dialogueModel.TryStartFighting.OnNext(_speakerID);
        }

        private void HandleGiveTakeCoinsEvent(DialogueEventNode eventNode)
        {
            string itemID = "Coins";
            GiveTakeItem(eventNode, itemID);
        }

        private void HandleGiveTakeItemEvent(DialogueEventNode eventNode)
        {
            string itemID = eventNode.ID;
            GiveTakeItem(eventNode, itemID);
        }

        private void HandleHasMetEvent(DialogueEventNode eventNode)
        {
            string characterID = GetSpeakerID(eventNode);
            bool hasMet = eventNode.IsTrue;

            HasMetData hasMetData = new(characterID, hasMet);

            _charactersModel.SetHasMet.OnNext(hasMetData);
        }

        private void HandleAddFriendshipEvent(DialogueEventNode eventNode)
        {
            string characterID = GetSpeakerID(eventNode);
            int friendshipAmount = eventNode.Amount;

            FriendshipData friendshipData = new(characterID, friendshipAmount);

            _charactersModel.AddFriendship.OnNext(friendshipData);
        }

        // OTHER METHODS

        private string GetSpeakerID(DialogueEventNode eventNode)
        {
            if (!eventNode.IsThisNPC)
                return eventNode.ID;

            return _speakerID;
        }

        private void GiveTakeItem(DialogueEventNode eventNode, string itemID)
        {
            int amount = eventNode.Amount;
            EGiveTakeEventType eventType = eventNode.GiveTakeEventType;

            GiveTakeItemData data = new(itemID, amount, eventType);

            _inventoryModel.GiveTakeItem.OnNext(data);
        }
    }
}
