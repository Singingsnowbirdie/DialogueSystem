using DialogueSystem.DialogueEditor;
using UnityEditor;
using UnityEngine;

namespace GM_Tools
{
    [CustomEditor(typeof(QuestsTool))]
    public class QuestsToolEditor : Editor
    {
        private string _questId = ""; 
        private EQuestState _questState = EQuestState.Available; 

        public override void OnInspectorGUI()
        {
            QuestsTool questsTool = (QuestsTool)target;

            DrawDefaultInspector();

            // UPDATE QUEST VALUES
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Update Quest Values", EditorStyles.boldLabel);

            _questId = EditorGUILayout.TextField("Quest ID", _questId);

            if (string.IsNullOrEmpty(_questId))
            {
                EditorGUILayout.HelpBox("Quest ID cannot be empty.", MessageType.Error);
            }

            _questState = (EQuestState)EditorGUILayout.EnumPopup("Quest State", _questState);

            if (GUILayout.Button("Update Quest Values"))
            {
                if (!string.IsNullOrEmpty(_questId))
                {
                    questsTool.UpdateQuestValues(_questId, _questState);
                }
                else
                {
                    Debug.Log("Quest ID cannot be empty.");
                }
            }


            // RESET DATA
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reset Data", EditorStyles.boldLabel);

            if (GUILayout.Button("Reset Data"))
            {
                questsTool.ResetData();
            }

        }
    }
}

