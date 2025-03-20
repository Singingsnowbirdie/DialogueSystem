using UnityEditor;
using UnityEngine;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeEditor(typeof(ConditionCheckNode))]
    public class ConditionCheckNodeEditor : DialogueNodeEditor
    {
        private ConditionCheckNode _conditionCheckNode;
        private DialogueGraph _dialogueGraph;
        private bool _isNotesFoldout;

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            if (_conditionCheckNode == null)
                _conditionCheckNode = target as ConditionCheckNode;

            if (_dialogueGraph == null)
                _dialogueGraph = (DialogueGraph)_conditionCheckNode.graph;

            EditorGUILayout.LabelField("Condition Type");
            _conditionCheckNode.Condition = (EDialogueCondition)EditorGUILayout.EnumPopup(_conditionCheckNode.Condition);

            switch (_conditionCheckNode.Condition)
            {
                case EDialogueCondition.IsGender:
                    ShowGenderOptions();
                    break;
                case EDialogueCondition.IsRace:
                    ShowRaceOptions();
                    break;
                case EDialogueCondition.HasMet:
                    ShowNPCOptions();
                    break;
                case EDialogueCondition.IsReputationAmount:
                    ShowReputationOptions();
                    break;
                case EDialogueCondition.IsQuestState:
                    ShowQuestStateOptions();
                    break;
                case EDialogueCondition.HasEnoughItems:
                    ShowItemsOptions();
                    break;
                case EDialogueCondition.HasEnoughCoins:
                    ShowCoinsOptions();
                    break;
                case EDialogueCondition.IsDialogueVariable:
                    ShowDialogueVariableOptions();
                    break;
                case EDialogueCondition.IsFriendshipAmount:
                    ShowFriendshipOptions();
                    break;
                case EDialogueCondition.InfluenceAttempt:
                    ShowInfluenceOptions();
                    break;
            }

            ShowNotesFoldout();
            VerifyConnections();

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        // OPTIONS
        private void ShowInfluenceOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Select Influence Type");
            _conditionCheckNode.InfluenceType = (EInfluenceType)EditorGUILayout.EnumPopup(_conditionCheckNode.InfluenceType);
        }

        private void ShowGenderOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Is Player`s Gender");
            _conditionCheckNode.PlayerGender = (EGender)EditorGUILayout.EnumPopup(_conditionCheckNode.PlayerGender);
        }

        private void ShowRaceOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Is Player`s Race");
            _conditionCheckNode.PlayerRace = (ERace)EditorGUILayout.EnumPopup(_conditionCheckNode.PlayerRace);
        }

        private void ShowDialogueVariableOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Variable Type");
            _conditionCheckNode.DialogueVariableType = (EDialogueVariableType)EditorGUILayout.EnumPopup(_conditionCheckNode.DialogueVariableType);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Variable ID");
            _conditionCheckNode.ID = EditorGUILayout.TextField(_conditionCheckNode.ID);

            if (_conditionCheckNode.DialogueVariableType == EDialogueVariableType.Amount)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Comparison Type");
                _conditionCheckNode.ComparisonType = (EComparisonTypes)EditorGUILayout.EnumPopup(_conditionCheckNode.ComparisonType);

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Comparison Amount");
                _conditionCheckNode.Amount = EditorGUILayout.IntField(_conditionCheckNode.Amount);
            }
        }

        private void ShowItemsOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Item ID");
            _conditionCheckNode.ID = EditorGUILayout.TextField(_conditionCheckNode.ID);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Items Amount");
            _conditionCheckNode.Amount = EditorGUILayout.IntField(_conditionCheckNode.Amount);
        }

        private void ShowNPCOptions()
        {
            EditorGUILayout.Space();
            _conditionCheckNode.IsThisNPC = EditorGUILayout.Toggle("This NPC", _conditionCheckNode.IsThisNPC);

            if (!_conditionCheckNode.IsThisNPC)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("NPC ID");
                _conditionCheckNode.ID = EditorGUILayout.TextField(_conditionCheckNode.ID);
            }
        }

        private void ShowFriendshipOptions()
        {
            ShowNPCOptions();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Comparison Type");
            _conditionCheckNode.ComparisonType = (EComparisonTypes)EditorGUILayout.EnumPopup(_conditionCheckNode.ComparisonType);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Friendship Amount");
            _conditionCheckNode.Amount = EditorGUILayout.IntField(_conditionCheckNode.Amount);
        }

        private void ShowReputationOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Compare To Faction");
            _conditionCheckNode.Faction = (EFaction)EditorGUILayout.EnumPopup(_conditionCheckNode.Faction);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Comparison Type");
            _conditionCheckNode.ComparisonType = (EComparisonTypes)EditorGUILayout.EnumPopup(_conditionCheckNode.ComparisonType);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reputation Amount");
            _conditionCheckNode.Amount = EditorGUILayout.IntField(_conditionCheckNode.Amount);
        }

        private void ShowQuestStateOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Quest ID");
            _conditionCheckNode.ID = EditorGUILayout.TextField(_conditionCheckNode.ID);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Quest State To Compare");
            _conditionCheckNode.QuestState = (EQuestState)EditorGUILayout.EnumPopup(_conditionCheckNode.QuestState);
        }

        private void ShowCoinsOptions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Coins Amount");
            _conditionCheckNode.Amount = EditorGUILayout.IntField(_conditionCheckNode.Amount);
        }

        // VERIFY CONNECTIONS

        private void VerifyConnections()
        {
            for (int i = 0; i < 2; i++)
            {
                VerifyPortConnections(_conditionCheckNode.GetConnectedOutputs(i));
            }
        }

        private void VerifyPortConnections(NodePort[] ports)
        {
            foreach (NodePort port in ports)
            {
                Node connectedNode = port.node;

                if (!FitsToConnect(connectedNode))
                    port.ClearConnections();
            }
        }

        private bool FitsToConnect(Node connectedNode)
        {
            return connectedNode is ConditionCheckNode
                || connectedNode is SpeakerNodeJumper
                || connectedNode is PlayerResponseNode
                || connectedNode is PlayerResponseNodeJumper
                || connectedNode is DialogueEventNode
                || connectedNode is SpeakerNode;
        }

        // OTHER
        private void ShowNotesFoldout()
        {
            EditorGUILayout.Space();

            _isNotesFoldout = EditorGUILayout.Foldout(_isNotesFoldout, "GD Notes");

            if (_isNotesFoldout)
            {
                _conditionCheckNode.Notes = EditorGUILayout.TextArea(_conditionCheckNode.Notes, EditorStyles.textArea, GUILayout.Height(100));
            }
        }
    }
}