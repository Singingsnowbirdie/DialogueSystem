using Characters;
using UnityEngine;

namespace GM_Tools
{
    public class CharactersTool : MonoBehaviour
    {
        private CharactersRepository _charactersRepository;

        private void OnValidate()
        {
            _charactersRepository ??= new CharactersRepository();
        }

        public void ResetData()
        {
            _charactersRepository.ResetData();
        }

        public void UpdateCharacterValues(string characterId, bool hasMet, int friendshipAmount)
        {
            _charactersRepository.SetCharacterValues(characterId, hasMet, friendshipAmount);
        }
    }
}

