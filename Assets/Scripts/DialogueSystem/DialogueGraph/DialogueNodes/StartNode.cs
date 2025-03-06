using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XNode;

namespace DialogueSystem.DialogueEditor
{
    [NodeTint("#800000"), NodeWidth(450)]
    [CreateNodeMenu("Dialogue Node/Start", 0)]
    public class StartNode : DialogueNode
    {
        [field: SerializeField, Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public Node Output { get; set; }

        [SerializeField, HideInInspector] public string Key;

        [NonSerialized] public string DialogueKeyLabel;

        public void SaveGraphToJson()
        {
            NodeGraph graph = this.graph;

            SaveGraphData(graph);
        }

        private void SaveGraphData(NodeGraph graph)
        {
            string filePath = Path.Combine(Application.dataPath, $"Resources/{Key}.json");
            string resourcesFolder = Path.Combine(Application.dataPath, "Resources");

            if (!Directory.Exists(resourcesFolder))
            {
                Directory.CreateDirectory(resourcesFolder);
            }

            List<NodeData> existingData = new List<NodeData>();
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                existingData = JsonUtility.FromJson<SerializationWrapper<NodeData>>(json).Items;
            }

            var existingDataDict = new Dictionary<string, NodeData>();
            foreach (var data in existingData)
            {
                existingDataDict[data.NodeId] = data;
            }

            List<NodeData> nodeDataList = new List<NodeData>();

            foreach (var node in graph.nodes)
            {
                if (node is SpeakerNode speakerNode)
                {
                    ProcessNode(speakerNode.NodeId, "SpeakerNode", speakerNode.DialogueLine, existingDataDict, nodeDataList);
                }
                else if (node is PlayerResponseNode playerResponseNode)
                {
                    ProcessNode(playerResponseNode.NodeId, "PlayerResponseNode", playerResponseNode.DialogueLine, existingDataDict, nodeDataList);
                }
            }

            string jsonData = JsonUtility.ToJson(new SerializationWrapper<NodeData>(nodeDataList), true);
            File.WriteAllText(filePath, jsonData);

            Debug.Log($"The graph is saved to {filePath}");
        }

        private void ProcessNode(string nodeId, string nodeType, string dialogueLine, Dictionary<string, NodeData> existingDataDict, List<NodeData> nodeDataList)
        {
            if (existingDataDict.TryGetValue(nodeId, out var existingData))
            {
                if (existingData.DialogueLine != dialogueLine)
                {
                    existingData.DialogueLine = dialogueLine;
                    Debug.Log($"Node data {nodeId} updated");
                }
                nodeDataList.Add(existingData);
            }
            else
            {
                nodeDataList.Add(new NodeData
                {
                    NodeType = nodeType,
                    NodeId = nodeId,
                    DialogueLine = dialogueLine
                });
                Debug.Log($"Added new node: {nodeId}");
            }
        }

    }

    [Serializable]
    public class NodeData
    {
        public string NodeType;
        public string NodeId;
        public string DialogueLine;
    }

    [System.Serializable]
    public class SerializationWrapper<T>
    {
        public List<T> Items;
        public SerializationWrapper(List<T> list) => Items = list;
    }
}