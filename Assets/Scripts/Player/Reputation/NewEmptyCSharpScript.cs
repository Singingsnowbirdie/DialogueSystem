using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace ReputationSystem
{
    public class ReputationModel
    {
        public ReactiveProperty<int> Reputation { get; private set; }

        public ReputationModel(int initialReputation)
        {
            Reputation = new ReactiveProperty<int>(initialReputation);
        }

        public void AddReputation(int amount)
        {
            Reputation.Value += amount;
        }

        public void RemoveReputation(int amount)
        {
            Reputation.Value -= amount;
        }
    }

    public class ReputationView : MonoBehaviour
    {
        [SerializeField] private Text reputationText;

        public void UpdateReputation(int reputation)
        {
            reputationText.text = $"Reputation: {reputation}";
        }
    }

    public class ReputationPresenter
    {
        private readonly ReputationModel _model;
        private readonly ReputationView _view;

        public ReputationPresenter(ReputationModel model, ReputationView view)
        {
            _model = model;
            _view = view;

            _model.Reputation.Subscribe(UpdateView);
        }

        private void UpdateView(int reputation)
        {
            _view.UpdateReputation(reputation);
        }

        public void AddReputation(int amount)
        {
            _model.AddReputation(amount);
        }

        public void RemoveReputation(int amount)
        {
            _model.RemoveReputation(amount);
        }
    }



}


