using DataSystem;
using DialogueSystem.DialogueEditor;
using System.IO;
using UnityEngine;

namespace Player
{
    public class PlayerRepository : IRepository
    {
        private const string JsonFilePath = "Player.json";

        public PlayerData PlayerData { get; private set; }

        public void SavePlayerData(PlayerData playerData)
        {
            string json = JsonUtility.ToJson(playerData);
            File.WriteAllText(JsonFilePath, json);
        }

        public void LoadData()
        {
            PlayerData = LoadPlayerData();
        }

        public PlayerData LoadPlayerData()
        {
            if (File.Exists(JsonFilePath))
            {
                string json = File.ReadAllText(JsonFilePath);
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                return playerData;
            }

            return new PlayerData();
        }

        public void ResetData()
        {
            if (File.Exists(JsonFilePath))
            {
                File.Delete(JsonFilePath);
                Debug.Log("Player data reset. Save file deleted.");
            }
            else
            {
                Debug.Log("No player data save file found to delete.");
            }
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public ERace Race = ERace.Human;
        public EGender Gender = EGender.Male;
        public string PlayerName = "John Doe";

        // REPUTATION
        public int Reputation_OfficialAuthorities = 0;
        public int Reputation_Civilian = 0;
        public int Reputation_Bandits = 0;
    }
}


