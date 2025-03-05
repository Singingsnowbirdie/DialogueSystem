using VContainer;
using VContainer.Unity;

namespace Player
{
    public class PlayerPresenter : IStartable
    {
        [Inject] private readonly PlayerView _view;
        [Inject] private readonly PlayerModel _model;

        public void Start()
        {
            _model.PlayerName = _view.PlayerName;
            _model.PlayerGender = _view.PlayerGender;
            _model.PlayerRace = _view.PlayerRace;
        }
    }
}


