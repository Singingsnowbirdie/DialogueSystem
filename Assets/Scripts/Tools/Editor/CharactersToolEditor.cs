using UnityEditor;
using UnityEngine;

namespace GM_Tools
{
    [CustomEditor(typeof(CharactersTool))]
    public class CharactersToolEditor : Editor
    {
        private string _characterId = ""; 
        private bool _hasMetPlayer = false;
        private int _friendshipAmount = 0; 

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CharactersTool debugTool = (CharactersTool)target;

            // UPDATE CHARACTER VALUES
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Apply Character Values", EditorStyles.boldLabel);

            _characterId = EditorGUILayout.TextField("Character ID", _characterId);

            if (string.IsNullOrEmpty(_characterId))
            {
                EditorGUILayout.HelpBox("Character ID cannot be empty.", MessageType.Error);
            }

            _hasMetPlayer = EditorGUILayout.Toggle("Has Met Player", _hasMetPlayer);
            _friendshipAmount = EditorGUILayout.IntField("Friendship Amount", _friendshipAmount);


            if (GUILayout.Button("Apply new value"))
            {
                debugTool.UpdateCharacterValues(_characterId, _hasMetPlayer, _friendshipAmount);
            }

            // RESET DATA
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reset Data", EditorStyles.boldLabel);

            if (GUILayout.Button("Reset All Data"))
            {
                debugTool.ResetData();
            }
        }
    }


}

