using System;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogueSystem.DialogueEditor
{
    [CustomNodeGraphEditor(typeof(DialogueGraph))]
    public class DialogueGraphEditor : NodeGraphEditor
    {
        private DialogueGraph _dialogueGraph;

        public override Node CopyNode(Node original)
        {
            if (original.GetType() == typeof(StartNode))
                return null;

            return base.CopyNode(original);
        }

        public override Node CreateNode(Type type, Vector2 position)
        {
            if (_dialogueGraph == null)
                _dialogueGraph = target as DialogueGraph;

            if (type == typeof(StartNode) && _dialogueGraph.StartNode != null)
                return null;

            return base.CreateNode(type, position);
        }

        /// <summary>
        /// Overridden to show the user only those nodes that are allowed in this graph.
        /// </summary>
        public override string GetNodeMenuName(Type type)
        {
            //Check if type has the CreateNodeMenuAttribute
            Node.CreateNodeMenuAttribute attrib;
            if (NodeEditorUtilities.GetAttrib(type, out attrib)) // Return custom path
            {
                string str = attrib.menuName;
                string[] subs = str.Split("/");
                if (subs[0] != "Dialogue Node")
                    return null;
                else
                    return attrib.menuName;
            }
            else
                return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            // Create a start node when creating a graph
            CreateNode(typeof(StartNode), new Vector2(0,0));
        }

        public override void RemoveNode(Node node)
        {
            if (node.GetType() == typeof(StartNode))
            {
                Debug.LogWarning("Cannot delete start node!");
                return;
            }


            base.RemoveNode(node);
        }
    }
}