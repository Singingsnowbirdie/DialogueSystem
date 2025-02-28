using DialogueSystem.DialogueEditor;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    [CreateAssetMenu(fileName = "DialogueDatabase", menuName = "Database/Dialogue Database")]
    public class DialogueDatabase : ScriptableObject
    {
        [SerializeField] private DialogueGraph[] dialogueGraphs; 

        private Dictionary<string, DialogueGraph> _dialogueDictionary; 

        private void InitializeDictionary()
        {
            _dialogueDictionary = new Dictionary<string, DialogueGraph>();

            foreach (var graph in dialogueGraphs)
            {
                if (graph.StartNode != null && !string.IsNullOrEmpty(graph.StartNode.GuidLabel))
                {
                    _dialogueDictionary[graph.StartNode.GuidLabel] = graph;
                }
                else
                {
                    Debug.LogWarning($"Graph {graph.name} does not have a GuidLabel or start node.");
                }
            }
        }

        public DialogueGraph GetDialogueGraph(string guidLabel)
        {
            if (_dialogueDictionary == null)
            {
                InitializeDictionary(); 
            }

            if (_dialogueDictionary.TryGetValue(guidLabel, out var graph))
            {
                return graph;
            }

            Debug.LogWarning($"DialogueGraph with GuidLabel {guidLabel} not found.");
            return null;
        }
    }
}
