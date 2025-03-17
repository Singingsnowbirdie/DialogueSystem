using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UniRx;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueVariablesRepository
    {
        private readonly string _saveFilePath;

        public DialogueVariablesRepository(string saveFilePath = "dialogue_variables.json")
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, saveFilePath);
        }

        public void ResetData()
        {
            if (File.Exists(_saveFilePath))
            {
                File.Delete(_saveFilePath);
                Debug.Log("Dialogue variables data has been reset.");
            }
        }

        public List<DialogueVariable> LoadDialogueVariables()
        {
            if (!File.Exists(_saveFilePath))
            {
                Debug.LogWarning("No dialogue variables file found. Starting with empty list.");
                return new List<DialogueVariable>();
            }

            try
            {
                string json = File.ReadAllText(_saveFilePath);
                var dialogueVariables = JsonSerializer.Deserialize<List<DialogueVariable>>(json);
                Debug.Log($"Dialogue variables loaded from {_saveFilePath}");
                return dialogueVariables;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load dialogue variables: {ex.Message}");
                return new List<DialogueVariable>();
            }
        }

        public void SaveDialogueVariables(ReactiveCollection<DialogueVariable> variables)
        {
            try
            {
                List<DialogueVariable> dialogueVariables = new List<DialogueVariable>(variables);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(dialogueVariables, options);

                File.WriteAllText(_saveFilePath, json);
                Debug.Log($"Dialogue variables saved to {_saveFilePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save dialogue variables: {ex.Message}");
            }
        }
    }
}
