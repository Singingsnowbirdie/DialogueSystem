using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Characters
{
    public class CharactersRepository
    {
        private List<CharacterData> _characters = new List<CharacterData>();
        private string _jsonFilePath;

        public string JsonFilePath
        {
            get
            {
                _jsonFilePath ??= Path.Combine(Application.persistentDataPath, "Characters.json");
                return _jsonFilePath;
            }
        }

        public CharactersRepository()
        {
            LoadData();
        }

        public CharacterData GetCharacterByID(string id)
        {
            CharacterData characterData = _characters.Find(character => character.Id == id);

            if (characterData == null)
            {
                characterData = new CharacterData(id);
                _characters.Add(characterData);
                SaveData();
            }

            return characterData;
        }

        public void SetCharacterValues(string characterId, bool hasMet, int friendshipAmount)
        {
            CharacterData characterData = GetCharacterByID(characterId);

            characterData.HasMetPlayer = hasMet;
            characterData.FriendshipAmount = friendshipAmount;
            SaveData();
            Debug.Log($"Character {characterId} HasMetPlayer updated to {hasMet}");
        }

        public void ResetData()
        {
            foreach (CharacterData character in _characters)
            {
                character.HasMetPlayer = false;
                character.FriendshipAmount = 0;
            }
            SaveData();
            Debug.Log("Characters database reset.");
        }

        private void LoadData()
        {
            if (File.Exists(JsonFilePath))
            {
                string json = File.ReadAllText(JsonFilePath);
                _characters = JsonUtility.FromJson<CharactersDatabaseWrapper>(json).Characters;
                Debug.Log("Characters data loaded from JSON.");
            }
            else
            {
                Debug.Log("No characters data found. Creating new database.");
            }
        }

        private void SaveData()
        {
            CharactersDatabaseWrapper wrapper = new CharactersDatabaseWrapper { Characters = _characters };
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(JsonFilePath, json);
            Debug.Log("Characters data saved to JSON.");
        }

        [System.Serializable]
        private class CharactersDatabaseWrapper
        {
            public List<CharacterData> Characters;
        }
    }

    [System.Serializable]
    public class CharacterData
    {
        public string Id;
        public bool HasMetPlayer;
        public int FriendshipAmount;

        public CharacterData(string id)
        {
            Id = id;
            HasMetPlayer = false;
            FriendshipAmount = 0;
        }
    }
}

