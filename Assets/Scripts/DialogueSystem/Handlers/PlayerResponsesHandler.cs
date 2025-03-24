using DialogueSystem.DialogueEditor;
using System.Collections.Generic;

namespace DialogueSystem
{
    public class PlayerResponsesHandler
    {
        private DialoguePresenter _dialoguePresenter;
        private DialogueHandler _dialogueHandler => _dialoguePresenter.DialogueHandler;

        public PlayerResponsesHandler(DialoguePresenter dialoguePresenter)
        {
            _dialoguePresenter = dialoguePresenter;
        }

        internal bool HasPlayerResponses(List<DialogueNode> dialogueNodes, out List<PlayerResponseNode> responses)
        {
            responses = new List<PlayerResponseNode>();

            foreach (DialogueNode node in dialogueNodes)
            {
                if (node is PlayerResponseNode responseNode)
                {
                    responses.Add(responseNode);
                }
                else if (node is ConditionCheckNode conditionCheckNode)
                {
                    if (_dialogueHandler.TryGetNextResponseNode(conditionCheckNode, out List<PlayerResponseNode> results))
                    {
                        foreach (PlayerResponseNode response in results)
                        {
                            responses.Add(response);
                        }
                    }
                }
            }

            return responses.Count > 0;
        }
    }
}
