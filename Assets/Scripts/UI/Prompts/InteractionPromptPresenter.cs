using UniRx;
using VContainer;
using VContainer.Unity;

namespace UI
{
    public class InteractionPromptPresenter : IStartable
    {
        [Inject] private InteractionPromptView _view;
        [Inject] private InteractionPromptUIModel _model;

        public void Start()
        {
            _view.OnSetModel(_model);
        }
    }
}
