using Characters;
using DialogueSystem.DialogueEditor;
using InventorySystem;
using System;
using System.Collections.Generic;
using XNode;

namespace DialogueSystem
{
    public class EventsHandler
    {
        private readonly CharactersModel _charactersModel;
        private readonly InventoryModel _inventoryModel;
        private readonly DialogueModel _dialogueModel;

        private string _speakerID = "";

        public EventsHandler(DialogueModel dialogueModel, CharactersModel charactersModel, 
            InventoryModel inventoryModel)
        {
            _dialogueModel = dialogueModel;
            _charactersModel = charactersModel;
            _inventoryModel = inventoryModel;
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
                    HandleCondition(conditionCheckNode);
                }
            }
        }

        // HANDLE CONDITION
        private void HandleCondition(ConditionCheckNode conditionCheckNode)
        {
            throw new NotImplementedException();
        }

        // HANDLE EVENT
        private void HandleEvent(DialogueEventNode eventNode)
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
                    break;
                case EDialogueEventType.StartTrading:
                    HandleStartTradingEvent();
                    break;
                case EDialogueEventType.StartFighting:
                    HandleStartFightingEvent();
                    break;
                case EDialogueEventType.SetDialogueVariable:
                    break;
                case EDialogueEventType.AddReputation:
                    break;
                case EDialogueEventType.PlayAnimation:
                    break;
                case EDialogueEventType.PlaySound:
                    break;
            }
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
