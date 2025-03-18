using DialogueSystem.DialogueEditor;
using VContainer;
using VContainer.Unity;

namespace Player
{
    public class PlayerPresenter : IStartable
    {
        [Inject] private readonly PlayerModel _model;

        public void Start()
        {
            _model.LoadPlayerData();
        }

        /// <summary>
        /// Value can be negative
        /// </summary>
        public void AddReputation(int amount, EFaction faction)
        {
            switch (faction)
            {
                case EFaction.OfficialAuthorities:
                    _model.Reputation_OfficialAuthorities.Value += amount;
                    break;
                case EFaction.Civilian:
                    _model.Reputation_Civilian.Value += amount;
                    break;
                case EFaction.Bandits:
                    _model.Reputation_Bandits.Value += amount;
                    break;
                default:
                    break;
            }

            _model.SavePlayerData();
        }
    }
}


