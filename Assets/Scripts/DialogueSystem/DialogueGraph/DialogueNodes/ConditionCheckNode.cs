using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

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
                    List<Node> connectedNodes = new List<Node>();

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
    }
}