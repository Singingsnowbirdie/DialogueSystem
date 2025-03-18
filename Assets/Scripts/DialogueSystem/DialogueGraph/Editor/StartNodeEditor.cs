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

            EditorGUILayout.LabelField("Dialogue ID:", startNode.DialogueId);

            if (GUILayout.Button("Copy Dialogue ID"))
            {
                GUIUtility.systemCopyBuffer = startNode.DialogueId;
                Debug.Log($"Dialogue ID copied: {startNode.DialogueId}");
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Save to JSON"))
            {
                startNode.SaveGraphToJson();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Save to XML"))
            {
                startNode.SaveGraphToXML();
            }

            VerifyConnections();

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