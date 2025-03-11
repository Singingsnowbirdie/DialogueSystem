using System;
using System.Collections.Generic;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Player
{
    public class PlayerPresenter : IStartable
    {
        [Inject] private readonly PlayerModel _model;
        private ICollection<IDisposable> _disposables = new List<IDisposable>();

        public void Start()
        {
            _model.LoadPlayerData();
        }

        public void AddReputation(int amount)
        {
            _model.ReputationAmount.Value += amount;
            _model.SavePlayerData();
        }

        public void RemoveReputation(int amount)
        {
            _model.ReputationAmount.Value -= amount;
            _model.SavePlayerData();
        }
    }
}


