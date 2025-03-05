using Gameplay.UI.ReactiveViews;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.DialogueUI
{
    public class PlayerAnswerView : UIReactiveView<PlayerAnswerView.UIModel>
    {
        [SerializeField] private TextMeshProReactiveStringView _playerAnswerTF;

        private Color normalColor = Color.gray;
        private Color hoverColor = Color.white;

        private UIModel _model;

        public class UIModel
        {
            public StringReactiveProperty PlayerAnswerText { get; private set; }
            public ReactiveProperty<bool> IsHovered { get; set; } = new ReactiveProperty<bool>(false);
        }

        protected override void OnSetModel(UIModel uiModel)
        {
            _playerAnswerTF.SetUIModel(uiModel.PlayerAnswerText);

            if (_playerAnswerTF != null)
                _playerAnswerTF.Text.color = normalColor;

            uiModel.IsHovered
                .Subscribe(val => OnHovered(val))
                .AddTo(this);

            _model = uiModel;
        }

        private void OnHovered(bool val)
        {
            if (val && _playerAnswerTF != null)
                _playerAnswerTF.Text.color = hoverColor;
            else if (!val && _playerAnswerTF != null)
                _playerAnswerTF.Text.color = normalColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _model.IsHovered.Value = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _model.IsHovered.Value = false;
        }
    }

}