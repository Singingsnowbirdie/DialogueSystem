using System;
using UnityEngine;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    [NodeTint("#800000"), NodeWidth(450)]
    [CreateNodeMenu("Dialogue Node/Start", 0)]
    public class StartNode : DialogueNode
    {
        [field: SerializeField, Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public Node Output { get; set; }

        [SerializeField, HideInInspector] public string Key;

        [NonSerialized] public string DialogueKeyLabel;
    }

}