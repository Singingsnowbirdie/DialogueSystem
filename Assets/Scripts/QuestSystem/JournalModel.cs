using System;

namespace QuestSystem
{
    public class JournalModel
    {
        private QuestsRepository _questsRepository;

        public QuestsRepository QuestsRepository
        {
            get
            {
                _questsRepository ??= new QuestsRepository();
                return _questsRepository;
            }
        }

        internal void LoadQuestsData()
        {
            QuestsRepository.LoadData();
        }
    }
}


