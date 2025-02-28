using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.ReactiveViews
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class TextMeshProReactiveView<T> : UIReactiveView<IReadOnlyReactiveProperty<T>>
    {
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
        [field: SerializeField] public string Format { get; private set; } = "{0}";
        [field: SerializeField] public bool AutoResize { get; private set; } = false;

        protected abstract string GetTextValue(T value);

        public void SetUIModel(T value)
        {
            SetUIModel(new ReactiveProperty<T>(value));
        }

        protected override void OnSetModel(IReadOnlyReactiveProperty<T> viewModel)
        {
            viewModel.Subscribe(SetText)
                .AddTo(compositeDisposable);
        }

        void SetText(T text)
        {
            Text.text = string.Format(Format, GetTextValue(text));

            if (AutoResize)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
            }
        }

        private void Reset()
        {
            Text = GetComponent<TextMeshProUGUI>();
        }
    }

}