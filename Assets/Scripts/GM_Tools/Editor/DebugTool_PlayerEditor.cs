using DialogueSystem.DialogueEditor;
using UnityEditor;
using UnityEngine;

namespace GM_Tools
{
    [CustomEditor(typeof(DebugTool_Player))]
    public class DebugTool_PlayerEditor : Editor
    {
        private string _playerName = "John Doe";
        private EGender _playerGender = EGender.Male;
        private ERace _playerRace = ERace.Human;

        private int _reputation_OfficialAuthorities = 0;
        private int _reputation_Civilian = 0;
        private int _reputation_Bandits = 0;

        public override void OnInspectorGUI()
        {

            DebugTool_Player debugTool = (DebugTool_Player)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Reset Player Data"))
            {
                debugTool.ResetPlayerData();
            }

            EditorGUILayout.Space();
            _playerName = EditorGUILayout.TextField("Player Name", _playerName);

            EditorGUILayout.Space();
            _playerGender = (EGender)EditorGUILayout.EnumPopup("Player Gender", _playerGender);

            EditorGUILayout.Space();
            _playerRace = (ERace)EditorGUILayout.EnumPopup("Player Race", _playerRace);

            EditorGUILayout.Space();
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter,
            };

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reputation:", headerStyle);

            EditorGUILayout.Space();
            _reputation_OfficialAuthorities = EditorGUILayout.IntField("Official Authorities", _reputation_OfficialAuthorities);
            
            EditorGUILayout.Space();
            _reputation_Civilian = EditorGUILayout.IntField("Civilian", _reputation_Civilian);
            
            EditorGUILayout.Space();
            _reputation_Bandits = EditorGUILayout.IntField("Bandits", _reputation_Bandits);

            if (GUILayout.Button("Update Player Data"))
            {
                debugTool.SetPlayerData(_playerName, _playerGender, _playerRace, 
                    _reputation_OfficialAuthorities, _reputation_Civilian, _reputation_Bandits);
            }
        }
    }
}

