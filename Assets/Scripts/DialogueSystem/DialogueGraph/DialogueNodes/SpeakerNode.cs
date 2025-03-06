using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    [NodeTint("#656475")]
    [CreateNodeMenu("Dialogue Node/Speaker`s Node", 0)]

    public class SpeakerNode : DialogueNode
    {
        [field: SerializeField, Input(backingValue = ShowBackingValue.Never)] public Node Input { get; set; }
        [field: SerializeField, Output(backingValue = ShowBackingValue.Never)] public Node Output { get; set; }
        [field: SerializeField, Output(backingValue = ShowBackingValue.Never), HideInInspector] public Node Events { get; set; }
        [field: SerializeField, HideInInspector] public string DialogueLine { get; set; }
        [field: SerializeField, HideInInspector] public List<SpeakerNodeJumper> ConnectedJumpers { get; set; } = new List<SpeakerNodeJumper>();
        [field: SerializeField, HideInInspector] public int SelectedIndex { get; set; }
        [field: SerializeField, HideInInspector] public int SpeakerKey { get; set; }
        [field: SerializeField, HideInInspector] public bool IsExpanded { get; set; } = true;

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

        internal bool TryGetEvents(out List<Node> events)
        {
            List<Node> nodes = new();

            NodePort[] connectedPorts = GetConnectedOutputs(1);

            foreach (NodePort item in connectedPorts)
            {
                nodes.Add(item.node);
            }
            events = nodes;
            return events.Count > 0;
        }
    }

    public static class IdGenerator
    {
        private static readonly System.Random _random = new();
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateId(int length = 8)
        {
            return new string(Enumerable.Repeat(Characters, length)
                .Select(s => s[_random.Next(s.Length)])
                .ToArray());
        }
    }
}