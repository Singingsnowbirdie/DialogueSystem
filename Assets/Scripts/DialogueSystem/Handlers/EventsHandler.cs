using Characters;
using DialogueSystem.DialogueEditor;
using System;
using System.Collections.Generic;
using XNode;

namespace DialogueSystem
{
    public class EventsHandler
    {
        private CharactersModel _charactersModel;

        private string _speakerID = "";

        public EventsHandler(CharactersModel charactersModel)
        {
            _charactersModel = charactersModel;
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

        private void HandleCondition(ConditionCheckNode conditionCheckNode)
        {
            throw new NotImplementedException();
        }

        private void HandleEvent(DialogueEventNode eventNode)
        {
            switch (eventNode.EventType)
            {
                case EDialogueEventType.HasMetEvent:
                    HandleHasMetEvent(eventNode);
                    break;
                case EDialogueEventType.SetQuestState:
                    break;
                case EDialogueEventType.StartTrading:
                    break;
                case EDialogueEventType.StartFighting:
                    break;
                case EDialogueEventType.SetDialogueVariable:
                    break;
                case EDialogueEventType.GiveTakeItemEvent:
                    break;
                case EDialogueEventType.GiveTakeCoinsEvent:
                    break;
                case EDialogueEventType.AddReputation:
                    break;
                case EDialogueEventType.AddFriendship:
                    break;
                case EDialogueEventType.PlayAnimation:
                    break;
                case EDialogueEventType.PlaySound:
                    break;
            }
        }

        private void HandleHasMetEvent(DialogueEventNode eventNode)
        {
            string characterID = _speakerID;

            if (!eventNode.IsThisNPC)
                characterID = eventNode.ID;

            bool hasMet = eventNode.IsTrue;

            HasMetData hasMetData = new(characterID, hasMet);

            _charactersModel.SetHasMet.OnNext(hasMetData);
        }
    }
}
