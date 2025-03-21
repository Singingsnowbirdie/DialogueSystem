using DialogueSystem;
using DialogueSystem.DialogueEditor;
using UniRx;

namespace QuestSystem
{
    public class JournalModel
    {
        public ISubject<QuestStateData> SetQuestState { get; } = new Subject<QuestStateData>();

        private QuestsRepository _questsRepository;

        public QuestsRepository QuestsRepository
        {
            get
            {
                _questsRepository ??= new QuestsRepository();
                return _questsRepository;
            }
        }
    }

    public readonly struct QuestStateData
    {
        public QuestStateData(string questID, EQuestState questState)
        {
            QuestID = questID;
            QuestState = questState;
        }

        public string QuestID { get; }

        public EQuestState QuestState { get; }
    }
}


