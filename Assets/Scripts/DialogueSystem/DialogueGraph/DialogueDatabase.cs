using DialogueSystem.DialogueEditor;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    [CreateAssetMenu(fileName = "DialogueDatabase", menuName = "Database/Dialogue Database")]
    public class DialogueDatabase : ScriptableObject
    {
        [SerializeField] private DialogueGraph[] _dialogueGraphs;

        private Dictionary<string, DialogueGraph> _dialogueDictionary;

        private void InitializeDictionary()
        {
            _dialogueDictionary = new Dictionary<string, DialogueGraph>();

            foreach (var graph in _dialogueGraphs)
            {
                if (graph.StartNode != null && !string.IsNullOrEmpty(graph.StartNode.Key))
                {
                    _dialogueDictionary.Add(graph.StartNode.Key, graph);
                }
                else
                {
                    Debug.LogWarning($"Graph {graph.name} does not have a GuidLabel or start node.");
                }
            }
        }

        internal bool TryGetDialogueGraph(string dialogueID, out DialogueGraph graph)
        {
            if (_dialogueDictionary == null)
            {
                InitializeDictionary();
            }

            if (_dialogueDictionary.TryGetValue(dialogueID, out graph))
            {
                return true;
            }

            Debug.LogWarning($"DialogueGraph with Guid {dialogueID} not found.");
            graph = null;
            return false;
        }
    }
}
