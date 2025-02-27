using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeEditor(typeof(DialogueNodeJumper))]
    public class DialogueNodeJumperEditor : NodeJumperEditor
    {
        private DialogueNodeJumper _nodeJumper => (DialogueNodeJumper)nodeJumper;

        protected override void ShowConnectedNode()
        {
            if (GUILayout.Button("Move To Connected Node"))
            {
                Selection.objects = new Object[1] { _nodeJumper.TargetNode };
                NodeEditorWindow w = NodeEditorWindow.Open(_nodeJumper.TargetNode.graph);
                w.Home(); // Focus selected node
            }
        }
    }
}