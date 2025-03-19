using DialogueSystem.DialogueEditor;
using UnityEditor;
using UnityEngine;

namespace GM_Tools
{
    [CustomEditor(typeof(PlayerTool))]
    public class PlayerToolEditor : Editor
    {
        private string _playerName = "John Doe";
        private EGender _playerGender = EGender.Male;
        private ERace _playerRace = ERace.Human;

        private int _reputation_OfficialAuthorities = 0;
        private int _reputation_Civilian = 0;
        private int _reputation_Bandits = 0;

        public override void OnInspectorGUI()
        {

            PlayerTool debugTool = (PlayerTool)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Reset Player Data"))
            {
                debugTool.ResetPlayerData();
            }

            // GENERAL
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("General:");

            _playerName = EditorGUILayout.TextField("Player Name", _playerName);
            _playerGender = (EGender)EditorGUILayout.EnumPopup("Player Gender", _playerGender);
            _playerRace = (ERace)EditorGUILayout.EnumPopup("Player Race", _playerRace);

            // REPUTATION
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reputation:");

            _reputation_OfficialAuthorities = EditorGUILayout.IntField("Official Authorities", _reputation_OfficialAuthorities);
            _reputation_Civilian = EditorGUILayout.IntField("Civilian", _reputation_Civilian);
            _reputation_Bandits = EditorGUILayout.IntField("Bandits", _reputation_Bandits);

            if (GUILayout.Button("Update Player Data"))
            {
                debugTool.SetPlayerData(_playerName, _playerGender, _playerRace, 
                    _reputation_OfficialAuthorities, _reputation_Civilian, _reputation_Bandits);
            }
        }
    }
}

