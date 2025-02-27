using System.Collections.Generic;
using System.Linq;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    public abstract class BaseDialogueNode : Node
    {
        public List<Node> GetConnectedNodes()
        {
            NodePort[] connectedPorts = GetConnectedOutputs(0);

            List<Node> nodes = new();

            foreach (NodePort item in connectedPorts)
            {
                Node connectedNode = item.node;
                nodes.Add(connectedNode);
            }

            return nodes;
        }

        public NodePort[] GetConnectedOutputs(int index)
        {
            NodePort outputPort = Outputs.ElementAt(index);
            return outputPort.GetConnections().ToArray();
        }
    }
}