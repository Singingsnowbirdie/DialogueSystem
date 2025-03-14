using Characters;
using DialogueSystem.DialogueEditor;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace QuestSystem
{
    public class QuestsRepository
    {
        private List<QuestData> _quests = new List<QuestData>();
        private string _jsonFilePath;

        public string JsonFilePath
        {
            get
            {
                _jsonFilePath ??= Path.Combine(Application.persistentDataPath, "Quests.json");
                return _jsonFilePath;
            }
        }

        public QuestsRepository()
        {
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(JsonFilePath))
            {
                string json = File.ReadAllText(JsonFilePath);
                _quests = JsonUtility.FromJson<QuestsDatabaseWrapper>(json).Quests;
                Debug.Log("Quests data loaded from JSON.");
            }
            else
            {
                Debug.Log("No quests data found. Creating new database.");
            }
        }

        public QuestData GetQuestByID(string id)
        {
            QuestData questData = _quests.Find(quest => quest.QuestID == id);

            if (questData == null)
            {
                questData = new QuestData(id);
                _quests.Add(questData);
                SaveData();
            }

            return questData;
        }

        public void ResetData()
        {
            foreach (QuestData questData in _quests)
            {
                questData.QuestState = EQuestState.Available;
            }
            SaveData();
            Debug.Log("Quests database reset.");
        }

        private void SaveData()
        {
            QuestsDatabaseWrapper wrapper = new QuestsDatabaseWrapper { Quests = _quests };
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(JsonFilePath, json);
            Debug.Log("Quests data saved to JSON.");
        }

        internal void SetQuestValues(string questId, EQuestState questState)
        {
            QuestData questData = GetQuestByID(questId);

            questData.QuestState = questState;

            SaveData();
            Debug.Log($"Quest {questId} state updated to {questState}");
        }

        [System.Serializable]
        private class QuestsDatabaseWrapper
        {
            public List<QuestData> Quests;
        }
    }

    public class QuestData
    {
        public string QuestID;
        public EQuestState QuestState;

        public QuestData(string id)
        {
            QuestID = id;
        }
    }
}


