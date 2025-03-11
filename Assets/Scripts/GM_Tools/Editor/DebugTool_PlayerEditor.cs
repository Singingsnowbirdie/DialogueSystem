using DialogueSystem.DialogueEditor;
using UnityEditor;
using UnityEngine;

namespace GM_Tools
{
    [CustomEditor(typeof(DebugTool_Player))]
    public class DebugTool_PlayerEditor : Editor
    {
        private int _reputationAmount = 0;
        private string _playerName = "John Doe";
        private EGender _playerGender = EGender.Male;
        private ERace _playerRace = ERace.Human;

        public override void OnInspectorGUI()
        {

            DebugTool_Player debugTool = (DebugTool_Player)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Reset Player Data"))
            {
                debugTool.ResetPlayerData();
            }

            EditorGUILayout.Space();
            _reputationAmount = EditorGUILayout.IntField("Reputation Amount", _reputationAmount);

            EditorGUILayout.Space();
            _playerName = EditorGUILayout.TextField("Player Name", _playerName);

            EditorGUILayout.Space();
            _playerGender = (EGender)EditorGUILayout.EnumPopup("Player Gender", _playerGender);

            EditorGUILayout.Space();
            _playerRace = (ERace)EditorGUILayout.EnumPopup("Player Race", _playerRace);

            if (GUILayout.Button("Update Player Data"))
            {
                debugTool.SetPlayerData(_playerName, _playerGender, _playerRace, _reputationAmount);
            }
        }
    }
}

