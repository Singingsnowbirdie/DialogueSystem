using Gameplay.UI.ReactiveViews;
using UniRx;
using UnityEngine;

namespace UI
{
    public class InteractionPromptView : UIView
    {
        [SerializeField] private TextMeshProReactiveStringView _promptTF;

        public override void OnSetModel(UIModel uiModel)
        {
            InteractionPromptUIModel model = uiModel as InteractionPromptUIModel;

            _promptTF.SetUIModel(model.PromptText);

            model.PromptText
                .Subscribe(val => OnPromptTextUpdated(val))
                .AddTo(gameObject);
        }

        private void OnPromptTextUpdated(string val)
        {
            gameObject.SetActive(!string.IsNullOrEmpty(val));
        }
    }
}
