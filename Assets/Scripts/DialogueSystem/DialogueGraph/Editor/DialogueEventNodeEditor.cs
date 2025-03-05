using UnityEditor;
using UnityEngine;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeEditor(typeof(DialogueEventNode))]
    public class DialogueEventNodeEditor : DialogueNodeEditor
    {
        private DialogueEventNode _eventNode;
        private DialogueGraph _dialogueGraph;

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            if (_eventNode == null)
                _eventNode = target as DialogueEventNode;

            if (_dialogueGraph == null)
                _dialogueGraph = (DialogueGraph)_eventNode.graph;

            EditorGUILayout.LabelField("Event Type");
            _eventNode.EventType = (EDialogueEventType)EditorGUILayout.EnumPopup(_eventNode.EventType);

            switch (_eventNode.EventType)
            {
                case EDialogueEventType.None:
                    // No options
                    break;
                case EDialogueEventType.StartTrading:
                    // No options
                    break;
                case EDialogueEventType.StartFighting:
                    // No options
                    break;
                case EDialogueEventType.HasMetEvent:
                    ShowMetNPCOptions();
                    break;
                case EDialogueEventType.SetQuestState:
                    ShowQuestOptions();
                    break;
                case EDialogueEventType.SetDialogueVariable:
                    ShowDialogueVariableOptions();
                    break;
                case EDialogueEventType.GiveTakeItemEvent:
                    ShowItemOptions();
                    break;
                case EDialogueEventType.GiveTakeCoinsEvent:
                    ShowCoinsOptions();
                    break;
                default:
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("GD Notes");
            _eventNode.Notes = EditorGUILayout.TextArea(_eventNode.Notes, EditorStyles.textArea, GUILayout.Height(50));

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void ShowCoinsOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Gide or Take Coins");
            _eventNode.GiveTakeEventType = (EGiveTakeEventType)EditorGUILayout.EnumPopup(_eventNode.GiveTakeEventType);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Coins Amount");
            _eventNode.Amount = EditorGUILayout.IntField(_eventNode.Amount);
        }

        private void ShowItemOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Gide or Take Item");
            _eventNode.GiveTakeEventType = (EGiveTakeEventType)EditorGUILayout.EnumPopup(_eventNode.GiveTakeEventType);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Item Config Key");
            _eventNode.ItemConfigKey = EditorGUILayout.IntField(_eventNode.ItemConfigKey);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Item Amount");
            _eventNode.Amount = EditorGUILayout.IntField(_eventNode.Amount);
        }

        private void ShowDialogueVariableOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Variable Type");
            _eventNode.DialogueVariableType = (EDialogueVariableType)EditorGUILayout.EnumPopup(_eventNode.DialogueVariableType);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Variable Key");
            _eventNode.DialogueVariableKey = EditorGUILayout.IntField(_eventNode.DialogueVariableKey);

            if (_eventNode.DialogueVariableType == EDialogueVariableType.Amount)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Comparison Amount");
                _eventNode.Amount = EditorGUILayout.IntField(_eventNode.Amount);
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Set Value");
                _eventNode.IsTrue = EditorGUILayout.Toggle(_eventNode.IsTrue);
            }
        }

        private void ShowQuestOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Quest Key");
            _eventNode.QuestConfigKey = EditorGUILayout.IntField(_eventNode.QuestConfigKey);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("New Quest State");
            _eventNode.QuestState = (EQuestState)EditorGUILayout.EnumPopup(_eventNode.QuestState);
        }

        void ShowMetNPCOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("NPC Key");
            _eventNode.NPCKey = EditorGUILayout.IntField(_eventNode.NPCKey);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Set Value");
            _eventNode.IsTrue = EditorGUILayout.Toggle(_eventNode.IsTrue);
        }
    }
}