using DialogueSystem.DialogueEditor;
using System.IO;
using UnityEngine;

namespace Player
{
    public class PlayerRepository
    {
        private const string SaveFilePath = "player.json";

        public void SavePlayerData(PlayerData playerData)
        {
            string json = JsonUtility.ToJson(playerData);
            File.WriteAllText(SaveFilePath, json);
            Debug.Log("Player data saved.");
        }

        public PlayerData LoadPlayerData()
        {
            if (File.Exists(SaveFilePath))
            {
                string json = File.ReadAllText(SaveFilePath);
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                Debug.Log("Player data loaded.");
                return playerData;
            }

            Debug.LogWarning("Player data file not found. Returning default value.");
            return new PlayerData();
        }

        public void ResetData()
        {
            if (File.Exists(SaveFilePath))
            {
                File.Delete(SaveFilePath);
                Debug.Log("Player data reset. Save file deleted.");
            }
            else
            {
                Debug.LogWarning("No player data save file found to delete.");
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

}


