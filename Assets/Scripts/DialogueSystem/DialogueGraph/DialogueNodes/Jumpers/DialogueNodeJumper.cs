﻿using UnityEngine;

namespace DialogueSystem.DialogueEditor
{
    [NodeTint("#302f3b")]
    public class DialogueNodeJumper : NodeJumper
    {
        [field: SerializeField, HideInInspector] public DialogueNode TargetNode { get; set; }

        public override string TargetNodeDialogueLine
        {
            get
            {
                if (TargetNode != null)
                    return TargetNode.DialogueLine;
                else return "The target node has been deleted! This jumper no longer works.";
            }
        }
    }
}