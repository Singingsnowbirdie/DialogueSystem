using DataSystem;
using NPC;
using System.Collections.Generic;
using UniRx;

namespace Characters
{
    public class CharactersModel
    {
        public Dictionary<string, CharacterView> CharacterViews { get; private set; } = new Dictionary<string, CharacterView>();
        public Dictionary<string, CharacterModel> CharacterModels { get; private set; } = new Dictionary<string, CharacterModel>();
        public ISubject<HasMetData> SetHasMet { get; } = new Subject<HasMetData>();
        public ISubject<FriendshipData> AddFriendship { get; } = new Subject<FriendshipData>();

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

    public readonly struct HasMetData
    {
        public HasMetData(string characterID, bool hasMetPlayer)
        {
            CharacterID = characterID;
            HasMetPlayer = hasMetPlayer;
        }

        public string CharacterID { get; }
        public bool HasMetPlayer { get; }
    }

    public readonly struct FriendshipData
    {
        public FriendshipData(string characterID, int friendshipAmount)
        {
            CharacterID = characterID;
            FriendshipAmount = friendshipAmount;
        }

        public string CharacterID { get; }
        public int FriendshipAmount { get; }
    }
}
