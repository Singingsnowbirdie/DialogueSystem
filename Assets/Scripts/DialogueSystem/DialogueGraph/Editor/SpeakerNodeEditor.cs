using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeEditor(typeof(SpeakerNode))]
    public class SpeakerNodeEditor : DialogueNodeEditor
    {
        private SpeakerNode _dialogueNode;
        private DialogueGraph _dialogueGraph;

        public override void OnBodyGUI()
        {
            if (_dialogueNode == null)
                _dialogueNode = target as SpeakerNode;

            if (_dialogueGraph == null)
                _dialogueGraph = (DialogueGraph)_dialogueNode.graph;

            EditorGUILayout.LabelField("Node ID:", _dialogueNode.NodeId);

            if (GUILayout.Button("Copy Node ID"))
            {
                GUIUtility.systemCopyBuffer = _dialogueNode.NodeId;
                Debug.Log($"Node ID copied: {_dialogueNode.NodeId}");
            }

            base.OnBodyGUI();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                _dialogueNode.TextAreaHeight += 20;
                _dialogueNode.TextAreaHeight = Mathf.Min(_dialogueNode.TextAreaHeight, 200);
            }
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                _dialogueNode.TextAreaHeight -= 20;
                _dialogueNode.TextAreaHeight = Mathf.Max(_dialogueNode.TextAreaHeight, 60);
            }
            EditorGUILayout.LabelField("Text Line");
            EditorGUILayout.EndHorizontal();
            _dialogueNode.DialogueLine = EditorGUILayout.TextArea(_dialogueNode.DialogueLine, EditorStyles.textArea, GUILayout.Height(_dialogueNode.TextAreaHeight));

            EditorGUILayout.Space();
            _dialogueNode.IsExpanded = EditorGUILayout.Foldout(_dialogueNode.IsExpanded, "Tags hint");

            if (_dialogueNode.IsExpanded)
            {
                EditorGUILayout.LabelField("To insert a player's name into ");
                EditorGUILayout.LabelField("the text, use this tag:");

                EditorGUILayout.BeginHorizontal();
                string playerNameTag = "/playerName/";
                EditorGUILayout.TextField("", playerNameTag, GUILayout.Width(85));
                if (GUILayout.Button("Copy", GUILayout.Width(50)))
                {
                    GUIUtility.systemCopyBuffer = playerNameTag;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField("");
            }

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
                    if (port.node is SpeakerNode || port.node is SpeakerNodeJumper)
                    {
                        return true;
                    }
                }
            }
            else if (connectedNode is SpeakerNode || connectedNode is SpeakerNodeJumper)
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
            return connectedNode is SpeakerNode
                || connectedNode is PlayerResponseNode
                || connectedNode is PlayerResponseNodeJumper
                || connectedNode is SpeakerNodeJumper
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
            SpeakerNodeJumper jumper = (SpeakerNodeJumper)NodeGraphEditor.GetEditor(node.graph, NodeEditorWindow.current).
                CreateNode(typeof(SpeakerNodeJumper), node.position + new Vector2(30, 30));
            jumper.name = "Dialogue Jumper";

            SpeakerNode dialogueNode = (SpeakerNode)node;

            jumper.TargetNode = dialogueNode;
            dialogueNode.ConnectedJumpers.Add(jumper);
        }
    }
}