using DataSystem;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
                Debug.Log("Characters data loaded from JSON.");
            }

            return dialogueVariables;
        }

        public void SaveData(List<DialogueVariableData> dialogueVariables)
        {
            if (dialogueVariables == null)
            {
                Debug.LogWarning("Dialogue variables list is null. Nothing to save.");
                return;
            }

            try
            {
                DialogueVariablesWrapper wrapper = new()
                {
                    Variables = dialogueVariables
                };

                string json = JsonUtility.ToJson(wrapper, true);

                File.WriteAllText(JsonFilePath, json);
                Debug.Log("Dialogue variables data saved to JSON.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save dialogue variables: {ex.Message}");
            }
        }

        public void ResetData()
        {
            if (File.Exists(JsonFilePath))
            {
                File.Delete(JsonFilePath);
                Debug.Log("Dialogue variables data has been reset.");
            }
        }

        internal DialogueVariableData GetDialogueVariable(string id)
        {
            DialogueVariableData variableData = _dialogueVariables.Find(variable => variable.VariableID == id);

            variableData ??= new DialogueVariableData(id);

            return variableData;
        }

        [Serializable]
        private class DialogueVariablesWrapper
        {
            public List<DialogueVariableData> Variables;
        }
    }
    public class DialogueVariableData
    {
        public string VariableID { get; }
        public bool IsTrue { get; set; }
        public int Amount { get; set; }

        public DialogueVariableData(string dialogueVariableID, bool isTrue = false, int amount = 0)
        {
            VariableID = dialogueVariableID;
            IsTrue = isTrue;
            Amount = amount;
        }
    }
}