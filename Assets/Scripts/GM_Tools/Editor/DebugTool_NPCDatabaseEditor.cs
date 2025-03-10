using UnityEditor;
using UnityEngine;

namespace GM_Tools
{
    [CustomEditor(typeof(DebugTool_NPCDatabase))]
    public class DebugTool_NPCDatabaseEditor : Editor
    {
        private string _npcId = ""; 
        private bool _hasMetPlayer = false; 

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DebugTool_NPCDatabase debugTool = (DebugTool_NPCDatabase)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Caution!", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("After pressing this button, all NPCs will \"forget\" that they met the player.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.Space();

            if (GUILayout.Button("Reset All Data"))
            {
                debugTool.ResetData();
            }

            EditorGUILayout.Space();

            _npcId = EditorGUILayout.TextField("NPC ID", _npcId);
            _hasMetPlayer = EditorGUILayout.Toggle("Has Met Player", _hasMetPlayer);

            if (GUILayout.Button("Apply new value"))
            {
                debugTool.SetHasMetPlayer(_npcId, _hasMetPlayer);
            }
        }
    }
}

