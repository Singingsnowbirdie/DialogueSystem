using DataSystem;
using NPC;
using System.Collections.Generic;

namespace Characters
{
    public class CharactersModel
    {
        public Dictionary<string, CharacterView> CharacterViews { get; private set; } = new Dictionary<string, CharacterView>();
        public Dictionary<string, CharacterModel> CharacterModels { get; private set; } = new Dictionary<string, CharacterModel>();

        private CharactersRepository _charactersRepository;

        public CharactersRepository CharactersRepository
        {
            get
            {
                _charactersRepository ??= new CharactersRepository();
                return _charactersRepository;
            }
        }
    }
}
