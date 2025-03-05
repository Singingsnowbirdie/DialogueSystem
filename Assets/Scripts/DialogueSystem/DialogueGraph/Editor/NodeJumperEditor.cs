using UnityEditor;
using UnityEngine;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeEditor(typeof(NodeJumper))]
    public abstract class NodeJumperEditor : DialogueNodeEditor
    {
        protected NodeJumper nodeJumper => target as NodeJumper;

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            ShowConnectedNode();
            ShowLabel();

            serializedObject.Update();
        }

        private void ShowLabel()
        {
            GUILayout.Label("", GUILayout.Height(100));
            GUIStyle textStyle = EditorStyles.label;
            textStyle.wordWrap = true;
            string newStr = nodeJumper.TargetNodeDialogueLine;

            if (newStr.Length > 100)
                newStr = newStr[..100] + "...";

            EditorGUI.LabelField(new Rect(20, 30, 170, 180), newStr, textStyle);
            textStyle.wordWrap = false;
        }

        protected abstract void ShowConnectedNode();
    }



}