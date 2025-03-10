using DialogueSystem;
using UniRx;

namespace NPC
{
    public class NPCModel
    {
        public string Id { get; private set; }
        public ISubject<DialogueData> TryStartDialogue { get; } = new Subject<DialogueData>();

        public NPCModel(string id)
        {
            Id = id;
        }
    }
}

