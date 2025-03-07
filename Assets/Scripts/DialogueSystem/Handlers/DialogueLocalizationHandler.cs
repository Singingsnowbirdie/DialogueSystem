using System.IO;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueLocalizationHandler
    {
        private DialogueModel _dialogueModel;

        public DialogueLocalizationHandler(DialogueModel dialogueModel)
        {
            _dialogueModel = dialogueModel;
        }

        public bool TryGetDialogueLine(string dialogueKey, string nodeId, out string dialogueLine)
        {
            if (_dialogueModel.DialogueJsonData == null || _dialogueModel.DialogueJsonData.DialogueKey != dialogueKey)
            {
                _dialogueModel.DialogueJsonData = GetDialogueJsonData(dialogueKey);
            }

            foreach (DialogueNodeData item in _dialogueModel.DialogueJsonData.Items)
            {
                if (item.NodeId == nodeId)
                {
                    // TODO: Add language selection here!

                    dialogueLine = item.DialogueLine;
                    return true;
                }
            }

            Debug.LogError($"Node with NodeId {nodeId} not found in file {dialogueKey}.json");

            dialogueLine = null;
            return false;
        }

        private DialogueDataWrapper GetDialogueJsonData(string dialogueKey)
        {
            string jsonFilePath = Path.Combine(Application.dataPath, "Resources", "JSON", $"{dialogueKey}.json");

            if (!File.Exists(jsonFilePath))
            {
                Debug.LogError($"JSON file not found: {jsonFilePath}");
                return null;
            }

            string jsonContent = File.ReadAllText(jsonFilePath);

            DialogueDataWrapper jsonData = JsonUtility.FromJson<DialogueDataWrapper>(jsonContent);
            jsonData.DialogueKey = dialogueKey;

            if (jsonData == null || jsonData.Items == null || jsonData.Items.Length == 0)
            {
                Debug.LogError("The JSON file is empty or has an invalid format.");
                return null;
            }

            return jsonData;
        }
    }

    [System.Serializable]
    public class DialogueDataWrapper
    {
        public string DialogueKey { get; set; }
        public DialogueNodeData[] Items;
    }

    [System.Serializable]
    public class DialogueNodeData
    {
        public string NodeType;
        public string NodeId;
        public string DialogueLine;
    }
}
