using DialogueSystem.DialogueEditor;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Player
{
    public class PlayerPresenter : IStartable, IDisposable
    {
        [Inject] private readonly PlayerModel _model;

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public void Start()
        {
            _model.LoadPlayerData();

            _model.AddReputation
                .Subscribe(data => AddReputation(data))
                .AddTo(_compositeDisposable);
        }

        private void AddReputation(ReputationData data)
        {
            switch (data.Faction)
            {
                case EFaction.OfficialAuthorities:
                    _model.Reputation_OfficialAuthorities.Value += data.Amount;
                    break;
                case EFaction.Civilian:
                    _model.Reputation_Civilian.Value += data.Amount;
                    break;
                case EFaction.Bandits:
                    _model.Reputation_Bandits.Value += data.Amount;
                    break;
                default:
                    break;
            }

            _model.SavePlayerData();
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}


