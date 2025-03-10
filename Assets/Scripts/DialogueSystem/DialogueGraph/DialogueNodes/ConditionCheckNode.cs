using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;
using static XNode.Node;

namespace DialogueSystem.DialogueEditor
{
    [NodeTint("#194d2d")]
    [CreateNodeMenu("Dialogue Node/Condition Check", 0)]
    public partial class ConditionCheckNode : DialogueNode
    {
        // General
        [field: SerializeField, Input(backingValue = ShowBackingValue.Never)] public Node Input { get; set; }
        [field: SerializeField, Output(backingValue = ShowBackingValue.Never)] public bool True { get; set; }
        [field: SerializeField, Output(backingValue = ShowBackingValue.Never)] public int False { get; set; }
        [field: SerializeField, HideInInspector] public EDialogueCondition Condition { get; set; }
        [field: SerializeField, HideInInspector] public string Notes { get; set; }
        [field: SerializeField, HideInInspector] public EComparisonTypes ComparisonType { get; set; }
        [field: SerializeField, HideInInspector] public int Amount { get; set; }

        // Reputation Options
        [field: SerializeField, HideInInspector] public EReputationComparisonTarget ReputationComparisonTarget { get; set; }
        [field: SerializeField, HideInInspector] public EFaction Faction { get; set; }

        // Player`s Gender Options
        [field: SerializeField, HideInInspector] public EGender PlayerGender { get; set; }

        // Player`s Race Options
        [field: SerializeField, HideInInspector] public ERace PlayerRace { get; set; }

        // Item Options
        [field: SerializeField, HideInInspector] public int ItemConfigKey { get; set; }

        // Quest Options
        [field: SerializeField, HideInInspector] public int QuestKey { get; set; }
        [field: SerializeField, HideInInspector] public EQuestState QuestState { get; set; }

        //NPC Options
        [field: SerializeField, HideInInspector] public string NpcID { get; set; }

        // Dialogue Variables Options
        [field: SerializeField, HideInInspector] public EDialogueVariableType DialogueVariableType { get; set; }
        [field: SerializeField, HideInInspector] public int DialogueVariableKey { get; set; }
        [field: SerializeField, HideInInspector] public int SelectedIndex { get; set; }

        internal List<Node> GetBoolConnections(bool isTrue)
        {
            IEnumerable<NodePort> outputs = Outputs;

            NodePort port = null;

            foreach (NodePort item in outputs)
            {
                if (item.fieldName.Contains("True") && isTrue)
                {
                    port = item;
                    break;
                }
                else if (item.fieldName.Contains("False") && !isTrue)
                {
                    port = item;
                    break;
                }
            }

            if (port != null)
            {
                List<NodePort> connections = port.GetConnections();

                if (connections.Count > 0)
                {
                    List<Node> connectedNodes = new();

                    foreach (NodePort node in connections)
                    {
                        connectedNodes.Add(node.node);
                    }

                    return connectedNodes;
                }
            }


            return null;
        }

        internal int GetResponseOrder()
        {
            NodePort inputPort = Inputs.ElementAt(0);

            if (inputPort.GetConnections().Count == 0)
                return 0;

            NodePort previousPort = inputPort.GetConnections().ToArray()[0];

            if (previousPort.node is ConditionCheckNode checkNode)
                return checkNode.GetResponseOrder();

            List<NodePort> connections = previousPort.GetConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                NodePort nodePort = connections[i];
                if (nodePort == inputPort)
                    return i + 1;
            }

            return 0;
        }

        public override void OnCreateConnection(NodePort fromPort, NodePort toPort)
        {
            if (!ValidateConnection(fromPort, toPort))
            {
                fromPort.Disconnect(toPort);
            }
            else
            {
                base.OnCreateConnection(fromPort, toPort);
            }
        }

        public bool ValidateConnection(NodePort fromPort, NodePort toPort)
        {
            Node connectingNode = toPort.node;

            if (fromPort.fieldName.Contains("True"))
            {
                return ValidateOutputConnection(fromPort, connectingNode);
            }
            else if (fromPort.fieldName.Contains("False"))
            {
                return ValidateOutputConnection(fromPort, connectingNode);
            }

            return true;
        }

        private bool ValidateOutputConnection(NodePort outputPort, Node connectingNode)
        {
            List<Node> connectedNodes = outputPort.GetConnections().Select(port => port.node).ToList();

            Debug.Log($"connectedNodes = {connectedNodes.Count}");

            bool hasSpeakerNode = connectedNodes.Any(node => node is SpeakerNode || node is SpeakerNodeJumper);

            if (connectingNode is SpeakerNode || connectingNode is SpeakerNodeJumper)
            {
                if (connectedNodes.Count > 1)
                {
                    EditorUtility.DisplayDialog("Connection Error", $"Cannot connect SpeakerNode or SpeakerNodeJumper to {outputPort.fieldName}: Other nodes are already connected.", "OK");
                    return false;
                }
            }
            else if (connectingNode is PlayerResponseNode || connectingNode is PlayerResponseNodeJumper)
            {
                if (hasSpeakerNode)
                {
                    EditorUtility.DisplayDialog("Connection Error", $"Cannot connect PlayerResponseNode or PlayerResponseNodeJumper to {outputPort.fieldName}: SpeakerNode or SpeakerNodeJumper is already connected.", "OK");
                    return false;
                }
            }

            return true;
        }
    }
}