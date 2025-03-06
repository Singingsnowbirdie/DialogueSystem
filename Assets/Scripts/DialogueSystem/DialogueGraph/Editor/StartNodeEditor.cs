using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeEditor(typeof(StartNode))]
    public class StartNodeEditor : DialogueNodeEditor
    {
        public override void OnCreate()
        {
            base.OnCreate();
            SetKey();
        }

        private void SetKey()
        {
            StartNode startNode = target as StartNode;

            startNode.Key = startNode.graph.name;
            startNode.DialogueKeyLabel = "KEY: " + startNode.Key;
        }

        public override void AddContextMenuItems(GenericMenu menu)
        {
            bool canRemove = true;
            // Actions if only one node is selected
            if (Selection.objects.Length == 1 && Selection.activeObject is Node)
            {
                Node node = Selection.activeObject as Node;
                menu.AddItem(new GUIContent("Move To Top"), false, () => NodeEditorWindow.current.MoveNodeToTop(node));
                canRemove = NodeGraphEditor.GetEditor(node.graph, NodeEditorWindow.current).CanRemove(node);
            }

            // Custom sctions if only one node is selected
            if (Selection.objects.Length == 1 && Selection.activeObject is XNode.Node)
            {
                XNode.Node node = Selection.activeObject as XNode.Node;
                menu.AddCustomContextMenuItems(node);
            }
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();

            StartNode startNode = target as StartNode;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Dialogue KEY:", startNode.Key, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Copy", GUILayout.Width(50)))
            {
                GUIUtility.systemCopyBuffer = startNode.Key;
            }
            EditorGUILayout.EndHorizontal();

            VerifyConnections();

            if (GUILayout.Button("Save to JSON"))
            {
                startNode.SaveGraphToJson();
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void VerifyConnections()
        {
            StartNode startNode = target as StartNode;

            NodePort[] ports = startNode.GetConnectedOutputs(0);

            foreach (NodePort port in ports)
            {
                Node connectedNode = port.node;

                if (!FitsToConnect(connectedNode))
                {
                    EditorUtility.DisplayDialog("Error", $"Node of type {connectedNode} cannot be attached to this port..", "OK");
                    port.ClearConnections();
                }
            }
        }

        private bool FitsToConnect(Node connectedNode)
        {
            return connectedNode is SpeakerNode
                || connectedNode is ConditionCheckNode
                || connectedNode is SpeakerNodeJumper;
        }
    }
}