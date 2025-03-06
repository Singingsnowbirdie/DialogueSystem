using DialogueSystem;
using UniRx;

namespace NPC
{
    public class NPCModel
    {
        public string Id { get; private set; }
        public ReactiveProperty<bool> HasMetPlayer { get; private set; } = new ReactiveProperty<bool>(false);

        public ISubject<DialogueData> TryStartDialogue { get; } = new Subject<DialogueData>();

        public NPCModel(string id)
        {
            Id = id;
        }

        public void SetHasMetPlayer(bool hasMet)
        {
            HasMetPlayer.Value = hasMet;
        }
    }
}

