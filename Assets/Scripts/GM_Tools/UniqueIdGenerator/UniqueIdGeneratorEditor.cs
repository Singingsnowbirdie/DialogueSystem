using UnityEditor;
using UnityEngine;

namespace GM_Tools
{
    [CustomEditor(typeof(UniqueIdGenerator))]
    public class UniqueIdGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UniqueIdGenerator generator = (UniqueIdGenerator)target;

            if (GUILayout.Button("Generate Unique ID"))
            {
                generator.GenerateId();
                Debug.Log("Generated ID: " + generator.UniqueId);
            }

            if (GUILayout.Button("Reset Unique ID"))
            {
                generator.ResetId();
                Debug.Log("ID reset");
            }

            if (GUILayout.Button("Copy Unique ID to Clipboard"))
            {
                generator.CopyIdToClipboard();
            }
        }
    }
}

