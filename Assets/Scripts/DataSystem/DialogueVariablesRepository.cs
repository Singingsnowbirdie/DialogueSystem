using DataSystem;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueVariablesRepository : IRepository
    {
        private List<DialogueVariableData> _dialogueVariables;

        private string _jsonFilePath;

        public string JsonFilePath
        {
            get
            {
                _jsonFilePath ??= Path.Combine(Application.persistentDataPath, "DialogueVariables.json");
                return _jsonFilePath;
            }
        }

        public void LoadData()
        {
            _dialogueVariables = LoadDialogueVariables();
        }

        public List<DialogueVariableData> LoadDialogueVariables()
        {
            List<DialogueVariableData> dialogueVariables = new List<DialogueVariableData>();

            if (File.Exists(JsonFilePath))
            {
                string json = File.ReadAllText(JsonFilePath);
                dialogueVariables = JsonUtility.FromJson<DialogueVariablesWrapper>(json).Variables;
            }

            return dialogueVariables;
        }

        internal void SaveData()
        {
            SaveData(_dialogueVariables);
        }

        public void SaveData(List<DialogueVariableData> dialogueVariables)
        {
            DialogueVariablesWrapper wrapper = new DialogueVariablesWrapper { Variables = dialogueVariables };
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(JsonFilePath, json);
        }

        public void ResetData()
        {
            if (File.Exists(JsonFilePath))
            {
                File.Delete(JsonFilePath);
                Debug.Log("Dialogue variables data has been reset.");
            }
        }

        internal bool TryGetDialogueVariable(string id, out DialogueVariableData variableData)
        {
            variableData = _dialogueVariables.Find(variable => variable.VariableID == id);
            return variableData != null;
        }

        public void AddDialogueVariable(string variableID, bool isTrue, int amount)
        {
            DialogueVariableData variableData = new DialogueVariableData(variableID, isTrue, amount);
            _dialogueVariables.Add(variableData);
        }

        [System.Serializable]
        private class DialogueVariablesWrapper
        {
            public List<DialogueVariableData> Variables;
        }
    }

    [System.Serializable]
    public class DialogueVariableData
    {
        public string VariableID { get; }
        public bool IsTrue { get; set; }
        public int Amount { get; set; }

        public DialogueVariableData(string dialogueVariableID, bool isTrue, int amount)
        {
            VariableID = dialogueVariableID;
            IsTrue = isTrue;
            Amount = amount;
        }
    }
}