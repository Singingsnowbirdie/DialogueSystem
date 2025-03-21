using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    [NodeTint("#131959"), NodeWidth(300)]
    [CreateNodeMenu("Dialogue Node/Event", 0)]

    public partial class DialogueEventNode : DialogueNode
    {
        [HideInInspector] public List<int> InvolvedNpcs = new();

        [field: SerializeField, Input(backingValue = ShowBackingValue.Never)] public Node Input { get; set; }
        [field: SerializeField, HideInInspector] public EDialogueEventType EventType { get; set; }
        [field: SerializeField, HideInInspector] public string Notes { get; set; }
        [field: SerializeField, HideInInspector] public int Amount { get; set; } = 1;
        [field: SerializeField, HideInInspector] public string ID { get; set; }
        [field: SerializeField, HideInInspector] public EGiveTakeEventType GiveTakeEventType { get; set; }
        [field: SerializeField, HideInInspector] public EQuestState QuestState { get; set; }
        [field: SerializeField, HideInInspector] public EDialogueVariableType DialogueVariableType { get; set; }
        [field: SerializeField, HideInInspector] public EFaction Faction { get; set; }
        [field: SerializeField, HideInInspector] public bool IsTrue { get; set; }
        [field: SerializeField, HideInInspector] public bool IsThisNPC { get; set; } = true;
    }
}