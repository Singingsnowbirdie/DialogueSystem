using VContainer;
using VContainer.Unity;

namespace QuestSystem
{
    public class JournalPresenter: IStartable
    {
        [Inject] private readonly JournalModel _model;

        public void Start()
        {
            _model.LoadQuestsData();
        }
    }
}


