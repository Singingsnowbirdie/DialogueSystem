using System;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeEditor(typeof(StartNode))]
    public class DialogueStartNodeEditor : BaseDialogueNodeEditor
    {
        public override void OnCreate()
        {
            base.OnCreate();
            GenerateGUID();
        }

        private void GenerateGUID()
        {
            StartNode startNode = target as StartNode;

            startNode.Guid = Guid.NewGuid().ToString();
            startNode.GuidLabel = "GUID: " + startNode.Guid;
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
            EditorGUILayout.TextField("Dialogue GUID", startNode.Guid, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Copy", GUILayout.Width(50)))
            {
                GUIUtility.systemCopyBuffer = startNode.Guid;
            }
            EditorGUILayout.EndHorizontal();

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
            return connectedNode is DialogueNode
                || connectedNode is ConditionCheckNode
                || connectedNode is DialogueNodeJumper;
        }
    }
}