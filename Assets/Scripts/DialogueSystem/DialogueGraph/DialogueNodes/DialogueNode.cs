using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    [NodeTint("#656475")]
    [CreateNodeMenu("Dialogue Node/Dialogue", 0)]

    public class DialogueNode : BaseDialogueNode
    {
        [field: SerializeField, Input(backingValue = ShowBackingValue.Never)] public Node Input { get; set; }
        [field: SerializeField, Output(backingValue = ShowBackingValue.Never)] public Node Output { get; set; }
        [field: SerializeField, Output(backingValue = ShowBackingValue.Never), HideInInspector] public Node Events { get; set; }
        [field: SerializeField, HideInInspector] public string DialogueLine { get; set; }
        [field: SerializeField, HideInInspector] public List<DialogueNodeJumper> ConnectedJumpers { get; set; } = new List<DialogueNodeJumper>();
        [field: SerializeField, HideInInspector] public int SelectedIndex { get; set; }
        [field: SerializeField, HideInInspector] public int SpeakerKey { get; set; }

        public void RemoveFromJumpers()
        {
            foreach (var item in ConnectedJumpers)
            {
                item.TargetNode = null;
            }
        }

        public void RemoveJumper(DialogueNodeJumper jumper)
        {
            foreach (var item in ConnectedJumpers)
            {
                if (item == jumper)
                {
                    ConnectedJumpers.Remove(item);
                    break;
                }
            }
        }

        internal bool HasEvents(out List<Node> events)
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
}