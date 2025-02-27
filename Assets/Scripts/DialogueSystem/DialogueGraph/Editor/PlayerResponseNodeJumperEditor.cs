using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeEditor(typeof(PlayerResponseNodeJumper))]
    public class PlayerResponseNodeJumperEditor : NodeJumperEditor
    {
        private PlayerResponseNodeJumper _nodeJumper => (PlayerResponseNodeJumper)nodeJumper;

        public override void OnBodyGUI()
        {
            ShowLabel();

            base.OnBodyGUI();
        }

        protected override void ShowConnectedNode()
        {
            if (GUILayout.Button("Show Connected Node"))
            {
                Selection.objects = new Object[1] { _nodeJumper.TargetNode };
                NodeEditorWindow w = NodeEditorWindow.Open(_nodeJumper.TargetNode.graph);
                w.Home(); // Focus selected node
            }
        }

        private void ShowLabel()
        {
            GUILayout.Label("", GUILayout.Height(18));
            GUIStyle textStyle = EditorStyles.label;
            EditorGUI.LabelField(new Rect(20, 30, 170, 18), _nodeJumper.GetResponseOrder(), textStyle);
        }
    }



}