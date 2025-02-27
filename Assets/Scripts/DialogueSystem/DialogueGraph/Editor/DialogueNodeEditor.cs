using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeEditor(typeof(DialogueNode))]
    public class DialogueNodeEditor : BaseDialogueNodeEditor
    {
        private DialogueNode _dialogueNode;
        private DialogueGraph _dialogueGraph;

        public override void OnBodyGUI()
        {
            if (_dialogueNode == null)
                _dialogueNode = target as DialogueNode;

            if (_dialogueGraph == null)
                _dialogueGraph = (DialogueGraph)_dialogueNode.graph;

            base.OnBodyGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Text Line");
            _dialogueNode.DialogueLine = EditorGUILayout.TextArea(_dialogueNode.DialogueLine, EditorStyles.textArea, GUILayout.Height(100));

            EditorGUILayout.Space();
            NodeEditorGUILayout.PortField(_dialogueNode.Outputs.ElementAt(1));

            VerifyConnections();

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void VerifyConnections()
        {
            VerifyNextNodePort();
            VerifyEventsPort();
        }

        private void VerifyEventsPort()
        {
            NodePort[] ports = _dialogueNode.GetConnectedOutputs(1);

            foreach (NodePort port in ports)
            {
                Node connectedNode = port.node;

                if (!FitsToEventsPort(connectedNode))
                    port.ClearConnections();
            }
        }

        private void VerifyNextNodePort()
        {
            NodePort[] ports = _dialogueNode.GetConnectedOutputs(0);

            foreach (NodePort port in ports)
            {
                Node connectedNode = port.node;

                if (!FitsToNodePort(connectedNode))
                    port.ClearConnections();

                if (IncorrectMultipleAttachment(ports, connectedNode))
                {
                    port.ClearConnections();
                    EditorUtility.DisplayDialog("Error", "You cannot attach both a player response node and a dialogue node at the same time. Please select one node type.", "OK");
                    break;
                }
            }
        }

        private bool IncorrectMultipleAttachment(NodePort[] ports, Node connectedNode)
        {
            if (connectedNode is PlayerResponseNode || connectedNode is PlayerResponseNodeJumper)
            {
                foreach (NodePort port in ports)
                {
                    if (port.node is DialogueNode || port.node is DialogueNodeJumper)
                    {
                        return true;
                    }
                }
            }
            else if (connectedNode is DialogueNode || connectedNode is DialogueNodeJumper)
            {
                foreach (NodePort port in ports)
                {
                    if (port.node is PlayerResponseNode || port.node is PlayerResponseNodeJumper)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool FitsToNodePort(Node connectedNode)
        {
            return connectedNode is DialogueNode
                || connectedNode is PlayerResponseNode
                || connectedNode is PlayerResponseNodeJumper
                || connectedNode is DialogueNodeJumper
                || connectedNode is ConditionCheckNode;
        }

        private bool FitsToEventsPort(Node connectedNode)
        {
            return connectedNode is ConditionCheckNode
            || connectedNode is DialogueEventNode;
        }

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

        private void CreateNewJumper(Node node)
        {
            DialogueNodeJumper jumper = (DialogueNodeJumper)NodeGraphEditor.GetEditor(node.graph, NodeEditorWindow.current).
                CreateNode(typeof(DialogueNodeJumper), node.position + new Vector2(30, 30));
            jumper.name = "Dialogue Jumper";

            DialogueNode dialogueNode = (DialogueNode)node;

            jumper.TargetNode = dialogueNode;
            dialogueNode.ConnectedJumpers.Add(jumper);
        }
    }
}