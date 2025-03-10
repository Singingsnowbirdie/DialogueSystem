using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NPC
{
    public class NPCDatabase
    {
        private List<NPCData> _npcs = new List<NPCData>();
        private string _jsonFilePath;

        public string JsonFilePath
        {
            get
            {
                _jsonFilePath ??= Path.Combine(Application.persistentDataPath, "npcs.json");
                return _jsonFilePath;
            }
        }

        public NPCDatabase()
        {
            LoadData();
        }

        public NPCData GetNPCByID(string id)
        {
            NPCData npcData = _npcs.Find(npc => npc.Id == id);

            if (npcData == null)
            {
                npcData = new NPCData(id);
                _npcs.Add(npcData);
                SaveData();
            }

            return npcData;
        }

        public void SetHasMetPlayer(string npcId, bool hasMet)
        {
            NPCData npc = GetNPCByID(npcId);

            npc.HasMetPlayer = hasMet;
            SaveData();
            Debug.Log($"NPC {npcId} HasMetPlayer updated to {hasMet}");

        }

        public void ResetData()
        {
            foreach (var npc in _npcs)
            {
                npc.HasMetPlayer = false; 
            }
            SaveData(); 
            Debug.Log("NPC database reset.");
        }

        private void LoadData()
        {
            if (File.Exists(JsonFilePath))
            {
                string json = File.ReadAllText(JsonFilePath);
                _npcs = JsonUtility.FromJson<NPCDatabaseWrapper>(json).NPCs;
                Debug.Log("NPC data loaded from JSON.");
            }
            else
            {
                Debug.Log("No NPC data found. Creating new database.");
            }
        }

        private void SaveData()
        {
            NPCDatabaseWrapper wrapper = new NPCDatabaseWrapper { NPCs = _npcs };
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(JsonFilePath, json);
            Debug.Log("NPC data saved to JSON.");
        }

        [System.Serializable]
        private class NPCDatabaseWrapper
        {
            public List<NPCData> NPCs;
        }
    }

    [System.Serializable]
    public class NPCData
    {
        public string Id;
        public bool HasMetPlayer;

        public NPCData(string id)
        {
            Id = id;
            HasMetPlayer = false;
        }
    }
}

