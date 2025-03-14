using DialogueSystem;
using UniRx;

namespace NPC
{
    public class CharacterModel
    {
        public string Id { get; private set; }
        public ISubject<DialogueData> TryStartDialogue { get; } = new Subject<DialogueData>();

        public CharacterModel(string id)
        {
            Id = id;
        }
    }
}

