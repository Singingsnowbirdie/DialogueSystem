using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeEditor(typeof(PlayerResponseNode))]
    public class PlayerResponseNodeEditor : DialogueNodeEditor
    {
        private PlayerResponseNode _playerResponseNode;

        public override void AddContextMenuItems(GenericMenu menu)
        {
            bool canRemove = true;
            // Actions if only one node is selected
            if (Selection.objects.Length == 1 && Selection.activeObject is Node)
            {
                Node node = Selection.activeObject as Node;
                menu.AddItem(new GUIContent("Move To Top"), false, () => NodeEditorWindow.current.MoveNodeToTop(node));
                menu.AddItem(new GUIContent("Duplicate As Jumper"), false, () => CreateNewJumper(node));

                canRemove = NodeGraphEditor.GetEditor(node.graph, NodeEditorWindow.current).CanRemove(node);
            }

            // Add actions to any number of selected nodes
            menu.AddItem(new GUIContent("Copy"), false, NodeEditorWindow.current.CopySelectedNodes);
            menu.AddItem(new GUIContent("Duplicate"), false, NodeEditorWindow.current.DuplicateSelectedNodes);

            if (canRemove)
                menu.AddItem(new GUIContent("Remove"), false, NodeEditorWindow.current.RemoveSelectedNodes);
            else
                menu.AddItem(new GUIContent("Remove"), false, null);

            // Custom sctions if only one node is selected
            if (Selection.objects.Length == 1 && Selection.activeObject is XNode.Node)
            {
                XNode.Node node = Selection.activeObject as XNode.Node;
                menu.AddCustomContextMenuItems(node);
            }
        }

        public override void OnBodyGUI()
        {
            _playerResponseNode = target as PlayerResponseNode;
            ShowLabel();

            EditorGUILayout.LabelField("Node ID:", _playerResponseNode.NodeId);

            if (GUILayout.Button("Copy Node ID"))
            {
                GUIUtility.systemCopyBuffer = _playerResponseNode.NodeId;
                Debug.Log($"Node ID copied: {_playerResponseNode.NodeId}");
            }

            base.OnBodyGUI();

            EditorGUILayout.Space();
            NodeEditorGUILayout.PortField(_playerResponseNode.Outputs.ElementAt(1));

            VerifyConnections();

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void ShowLabel()
        {
            GUILayout.Label("", GUILayout.Height(18));
            GUIStyle textStyle = EditorStyles.label;
            EditorGUI.LabelField(new Rect(20, 30, 170, 18), _playerResponseNode.GetResponseOrder(), textStyle);
        }

        private void CreateNewJumper(Node node)
        {
            PlayerResponseNodeJumper jumper = (PlayerResponseNodeJumper)NodeGraphEditor.GetEditor(node.graph, NodeEditorWindow.current).
                CreateNode(typeof(PlayerResponseNodeJumper), node.position + new Vector2(30, 30));
            jumper.name = "Player Response Jumper";

            PlayerResponseNode playerResponseNode = (PlayerResponseNode)node;

            jumper.TargetNode = playerResponseNode;
            playerResponseNode.ConnectedJumpers.Add(jumper);
        }

        private void VerifyConnections()
        {
            VerifyNextNodePort();
            VerifyEventsPort();


        }

        private void VerifyEventsPort()
        {
            NodePort[] ports = _playerResponseNode.GetConnectedOutputs(1);

            foreach (NodePort port in ports)
            {
                Node connectedNode = port.node;

                if (!FitsToEventsPort(connectedNode))
                    port.ClearConnections();
            }
        }

        private void VerifyNextNodePort()
        {
            NodePort[] ports = _playerResponseNode.GetConnectedOutputs(0);

            foreach (NodePort port in ports)
            {
                Node connectedNode = port.node;

                if (!FitsToNodePort(connectedNode))
                    port.ClearConnections();
            }
        }

        private bool FitsToNodePort(Node connectedNode)
        {
            return connectedNode is SpeakerNode
                || connectedNode is ConditionCheckNode
                || connectedNode is SpeakerNodeJumper;
        }

        private bool FitsToEventsPort(Node connectedNode)
        {
            return connectedNode is DialogueEventNode
                || connectedNode is ConditionCheckNode;
        }
    }
}