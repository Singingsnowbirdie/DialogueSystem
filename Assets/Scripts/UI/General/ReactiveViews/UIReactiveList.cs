using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Gameplay.UI.ReactiveViews
{
    public abstract class UIReactiveList<IUIReactiveView, TUIModel> : UIReactiveView<ReactiveCollection<TUIModel>>
    where IUIReactiveView : UIReactiveView<TUIModel>
    {
        [field: SerializeField] public List<IUIReactiveView> UIViews { get; private set; } = new();
        [SerializeField] private IUIReactiveView _template;

        public System.Action OnReload { get; set; } = null;

        private void Start()
        {
            _template.gameObject.SetActive(false);
        }

        protected override void OnSetModel(ReactiveCollection<TUIModel> viewModel)
        {
            ClearUIViewInstances();

            foreach (var uiModel in viewModel)
            {
                var instance = CreateUIViewInstance(uiModel);
                UIViews.Add(instance);
                OnReload?.Invoke();
            }

            viewModel.ObserveReplace()
                .Subscribe(x =>
                {
                    var instance = CreateUIViewInstance(x.NewValue);
                    UIViews[x.Index] = instance;
                })
                .AddTo(compositeDisposable);

            viewModel.ObserveAdd()
                .Subscribe(x =>
                {
                    var instance = CreateUIViewInstance(x.Value);
                    UIViews.Add(instance);
                    OnReload?.Invoke();
                })
                .AddTo(compositeDisposable);

            viewModel.ObserveRemove()
                .Subscribe(x =>
                {
                    Destroy(UIViews[x.Index].gameObject);
                    UIViews.RemoveAt(x.Index);
                    OnReload?.Invoke();
                })
                .AddTo(compositeDisposable);

            viewModel.ObserveReset()
                .Subscribe(_ =>
                {
                    ClearUIViewInstances();
                })
                .AddTo(compositeDisposable);
        }

        private void ClearUIViewInstances()
        {
            foreach (var uiView in UIViews)
            {
                Destroy(uiView.gameObject);
            }
            UIViews.Clear();
        }

        private IUIReactiveView CreateUIViewInstance(TUIModel uiModel)
        {
            IUIReactiveView instance = Instantiate(_template, _template.transform.parent);
            instance.gameObject.SetActive(true);
            instance.SetUIModel(uiModel);
            return instance;
        }
    }
}