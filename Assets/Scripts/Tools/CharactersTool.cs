using DataSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GM_Tools
{
    public class CharactersTool : MonoBehaviour
    {
        private CharactersRepository _charactersRepository;

        public CharactersRepository CharactersRepository
        {
            get
            {
                _charactersRepository ??= new CharactersRepository();
                return _charactersRepository;
            }
        }

        public void ResetData()
        {
            CharactersRepository.ResetData();
        }

        public void UpdateCharacterValues(string characterId, bool hasMet, int friendshipAmount)
        {
            List<CharacterData> characters = CharactersRepository.LoadCharactersData();

            CharacterData characterData = FindOrCreate(characters,characterId);

            characterData.HasMetPlayer = hasMet;
            characterData.FriendshipAmount = friendshipAmount;
            CharactersRepository.SaveData(characters);
            Debug.Log($"Character {characterId} updated; hasMet = {hasMet}; friendshipAmount = {friendshipAmount}");
        }

        public CharacterData FindOrCreate(List<CharacterData> characters, string id)
        {
            CharacterData character = characters.Find(c => c.Id == id);

            if (character == null)
            {
                character = new CharacterData(id);
                characters.Add(character);
            }

            return character;
        }
    }
}

