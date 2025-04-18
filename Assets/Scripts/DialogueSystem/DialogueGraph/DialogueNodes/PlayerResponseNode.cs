using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    [NodeTint("#502e0e")]
    [CreateNodeMenu("Dialogue Node/Player Response", 0)]
    public class PlayerResponseNode : DialogueNode, ISerializationCallbackReceiver
    {
        [field: SerializeField, Input(backingValue = ShowBackingValue.Never)] public Node Input { get; set; }
        [field: SerializeField, Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)] public Node Output { get; set; }
        [field: SerializeField, Output(backingValue = ShowBackingValue.Never), HideInInspector] public Node Events { get; set; }
        [field: SerializeField, TextArea(5, 10)] public string DialogueLine { get; set; } = "";
        [field: SerializeField, HideInInspector] public List<PlayerResponseNodeJumper> ConnectedJumpers { get; set; } = new List<PlayerResponseNodeJumper>();

        [SerializeField, HideInInspector] private string _nodeId;

        public string NodeId
        {
            get
            {
                if (string.IsNullOrEmpty(_nodeId))
                {
                    _nodeId = IdGenerator.GenerateId();
                }
                return _nodeId;
            }
            private set => _nodeId = value;
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

        internal bool TryGetEvents(out List<Node> events)
        {
            events = EventPortConnections;
            return EventPortConnections.Count > 0;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _nodeId = IdGenerator.GenerateId();
        }

        private List<Node> EventPortConnections
        {
            get
            {
                List<Node> nodes = new();
                NodePort[] connectedPorts = GetConnectedOutputs(1);

                foreach (NodePort item in connectedPorts)
                {
                    nodes.Add(item.node);
                }

                return nodes;
            }
        }
    }
}