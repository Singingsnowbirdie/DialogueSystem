using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    [NodeTint("#502e0e")]
    public class PlayerResponseNodeJumper : NodeJumper
    {
        [field: SerializeField, HideInInspector] public PlayerResponseNode TargetNode { get; set; }

        public override string TargetNodeDialogueLine
        {
            get
            {
                if (TargetNode != null)
                    return TargetNode.DialogueLine;
                else
                    return "The target node has been deleted! This jumper no longer works.";
            }
        }

        internal bool TryGetPlayerResponseNode(out PlayerResponseNode relatedPlayerResponseNode)
        {
            relatedPlayerResponseNode = TargetNode;
            return TargetNode != null;
        }

        public string GetResponseOrder()
        {
            string str = "Response Order: ";

            int order = 1;

            NodePort inputPort = Inputs.ElementAt(0);

            if (inputPort.GetConnections().Count == 0)
                return "There is no connected inputs";

            NodePort previousPort = inputPort.GetConnections().ToArray()[0];

            if (previousPort.node is ConditionCheckNode checkNode)
                order = checkNode.GetResponseOrder();

            if (order == 0)
                return "There is no connected inputs";

            List<NodePort> connections = previousPort.GetConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                NodePort nodePort = connections[i];
                if (nodePort == inputPort)
                    order += i;
            }

            return str + order;
        }
    }
}