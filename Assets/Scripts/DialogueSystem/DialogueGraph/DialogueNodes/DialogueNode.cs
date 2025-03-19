using System.Collections.Generic;
using System.Linq;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    public abstract class DialogueNode : Node
    {
        public bool TryGetConnectedNode(out DialogueNode node)
        {
            if (TryGetConnectedNodes(out List<DialogueNode> nodes))
            {
                node = nodes[0];
                return true;
            }

            node = null;
            return false;
        }

        public bool TryGetConnectedNodes(out List<DialogueNode> dialogueNodes)
        {
            NodePort[] connectedPorts = GetConnectedOutputs(0);

            List<DialogueNode> nodes = new();

            foreach (NodePort item in connectedPorts)
            {
                if (item.node is DialogueNode dialogueNode)
                    nodes.Add(dialogueNode);
            }

            dialogueNodes = nodes;

            if (dialogueNodes.Count > 0)
                return true;

            return false;
        }

        public NodePort[] GetConnectedOutputs(int index)
        {
            NodePort outputPort = Outputs.ElementAt(index);
            return outputPort.GetConnections().ToArray();
        }
    }
}