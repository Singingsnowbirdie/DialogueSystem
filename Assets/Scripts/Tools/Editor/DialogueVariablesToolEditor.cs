using UnityEditor;
using UnityEngine;

namespace GM_Tools
{
    [CustomEditor(typeof(DialogueVariablesTool))]
    public class DialogueVariablesToolEditor : Editor
    {
        private string _variableId = ""; 
        private bool _isTrue = false;    
        private int _amount = 0;         

        public override void OnInspectorGUI()
        {
            DialogueVariablesTool tool = (DialogueVariablesTool)target;

            DrawDefaultInspector();

            // UPDATE DIALOGUE VARIABLE
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Update Dialogue Variable", EditorStyles.boldLabel);

            _variableId = EditorGUILayout.TextField("Variable ID", _variableId);

            _isTrue = EditorGUILayout.Toggle("Is True", _isTrue);

            _amount = EditorGUILayout.IntField("Amount", _amount);

            if (GUILayout.Button("Update Variable"))
            {
                tool.UpdateDialogueVariable(_variableId, _isTrue, _amount);
            }

            // RESET DATA
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reset Data", EditorStyles.boldLabel);

            if (GUILayout.Button("Reset Data"))
            {
                tool.ResetData();
            }
        }
    }
}

