using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DataSystem
{
    public class CharactersRepository : IRepository
    {
        private List<CharacterData> _characters;

        private string _jsonFilePath;

        public string JsonFilePath
        {
            get
            {
                _jsonFilePath ??= Path.Combine(Application.persistentDataPath, "Characters.json");
                return _jsonFilePath;
            }
        }

        public void LoadData()
        {
            _characters = LoadCharactersData();
        }

        public List<CharacterData> LoadCharactersData()
        {
            List<CharacterData> characters = new List<CharacterData>();

            if (File.Exists(JsonFilePath))
            {
                string json = File.ReadAllText(JsonFilePath);
                characters = JsonUtility.FromJson<CharactersDatabaseWrapper>(json).Characters;
            }

            return characters;
        }

        public CharacterData GetCharacterByID(string id)
        {
            CharacterData characterData = _characters.Find(character => character.Id == id);

            if (characterData == null)
            {
                characterData = new CharacterData(id);
                _characters.Add(characterData);
                SaveData(_characters);
            }

            return characterData;
        }

        public void ResetData()
        {
            if (File.Exists(JsonFilePath))
            {
                File.Delete(JsonFilePath);
                Debug.Log("Characters data reset. Save file deleted.");
            }
            else
            {
                Debug.Log("No characters data save file found to delete.");
            }
        }

        public void SaveData()
        {
            SaveData(_characters);
        }

        public void SaveData(List<CharacterData> characters)
        {
            CharactersDatabaseWrapper wrapper = new CharactersDatabaseWrapper { Characters = characters };
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

