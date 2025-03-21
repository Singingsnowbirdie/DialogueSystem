using DialogueSystem;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace UI
{
    public class CursorPresenter : IStartable, IDisposable
    {
        [Inject] private readonly CursorModel _model;
        [Inject] private readonly DialogueModel _dialogueModel;

        CompositeDisposable _disposables = new CompositeDisposable();

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Start()
        {
            _model.SetCursorState(false);

            _dialogueModel.IsDialogueStarted
                .Subscribe(val => _model.SetCursorState(val))
                .AddTo(_disposables);
        }
    }
}

