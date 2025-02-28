using UniRx;
using UnityEngine;

namespace Gameplay.UI.General.ReactiveViews
{
    public abstract class UIReactiveView<T> : MonoBehaviour, IUIReactiveView<T>
    {
        protected CompositeDisposable compositeDisposable;
        protected T uiModel;

        public T UIViewModel => uiModel;

        public void SetUIModel(T uiModel)
        {
            if (compositeDisposable == default)
            {
                compositeDisposable = new();
                compositeDisposable.AddTo(this);
            }
            compositeDisposable.Clear();
            this.uiModel = uiModel;
            OnSetModel(uiModel);
        }

        protected abstract void OnSetModel(T viewModel);
    }

}