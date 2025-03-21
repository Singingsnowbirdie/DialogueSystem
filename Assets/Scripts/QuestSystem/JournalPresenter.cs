using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace QuestSystem
{
    public class JournalPresenter : IStartable, IDisposable
    {
        [Inject] private readonly JournalModel _model;

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        public void Start()
        {
            _model.QuestsRepository.LoadData();

            _model.SetQuestState
                .Subscribe(data => SetQuestState(data))
                    .AddTo(_compositeDisposable);
        }

        private void SetQuestState(QuestStateData data)
        {
            QuestData quest = _model.QuestsRepository.GetQuestByID(data.QuestID);
            quest.QuestState = data.QuestState;
            _model.QuestsRepository.SaveData();
        }
    }
}


