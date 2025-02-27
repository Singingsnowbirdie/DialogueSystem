using UnityEngine;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    public abstract class NodeJumper : BaseDialogueNode
    {
        [field: SerializeField, Input(backingValue = ShowBackingValue.Never)] public Node Input { get; set; }

        public abstract string TargetNodeDialogueLine { get; }
    }

}