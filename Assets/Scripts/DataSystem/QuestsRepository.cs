using DataSystem;
using DialogueSystem.DialogueEditor;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace QuestSystem
{
    public class QuestsRepository : IRepository
    {
        private List<QuestData> _quests;

        private string _jsonFilePath;

        public string JsonFilePath
        {
            get
            {
                _jsonFilePath ??= Path.Combine(Application.persistentDataPath, "Quests.json");
                return _jsonFilePath;
            }
        }

        public void LoadData()
        {
            _quests = LoadQuests();
        }

        public List<QuestData> LoadQuests()
        {
            List<QuestData> quests = new List<QuestData>();

            if (File.Exists(JsonFilePath))
            {
                string json = File.ReadAllText(JsonFilePath);

                QuestsDatabaseWrapper wrapper = JsonUtility.FromJson<QuestsDatabaseWrapper>(json);
                quests = wrapper.Quests;
            }

            return quests;
        }

        public QuestData GetQuestByID(string id)
        {
            QuestData questData = _quests.Find(quest => quest.QuestID == id);

            if (questData == null)
            {
                questData = new QuestData(id);
                _quests.Add(questData);
                SaveData(_quests);
            }

            return questData;
        }

        public void ResetData()
        {
            if (File.Exists(JsonFilePath))
            {
                File.Delete(JsonFilePath);
                Debug.Log("Quests data reset. Save file deleted.");
            }
            else
            {
                Debug.Log("No quests data save file found to delete.");
            }

        }

        public void SaveData(List<QuestData> quests)
        {
            QuestsDatabaseWrapper wrapper = new QuestsDatabaseWrapper { Quests = quests };
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(JsonFilePath, json);
        }

        [Serializable]
        private class QuestsDatabaseWrapper
        {
            public List<QuestData> Quests;
        }
    }

    [Serializable]
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


