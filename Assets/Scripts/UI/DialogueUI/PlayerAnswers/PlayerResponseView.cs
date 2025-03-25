using DialogueSystem.DialogueEditor;
using Gameplay.UI.ReactiveViews;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.DialogueUI
{
    public class PlayerResponseView : UIReactiveView<PlayerResponseModel>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProReactiveStringView _playerAnswerTF;
        [SerializeField] private Button _button;

        private Color _normalColor = Color.gray;
        private Color _hoverColor = Color.white;

        private PlayerResponseModel _model;

        protected override void OnSetModel(PlayerResponseModel uiModel)
        {
            _playerAnswerTF.SetUIModel(uiModel.PlayerAnswerText);

            if (_playerAnswerTF != null)
                _playerAnswerTF.Text.color = _normalColor;

            uiModel.IsHovered
                .Subscribe(val => OnHovered(val))
                .AddTo(this);

            _model = uiModel;

            _button.OnClickAsObservable()
                .Subscribe(val => _model.PlayerResponseSelected.OnNext(_model.ResponseNode))
                .AddTo(this);
        }

        private void OnHovered(bool val)
        {
            if (val && _playerAnswerTF != null)
                _playerAnswerTF.Text.color = _hoverColor;
            else if (!val && _playerAnswerTF != null)
                _playerAnswerTF.Text.color = _normalColor;
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

    public class PlayerResponseModel
    {
        public PlayerResponseModel(PlayerResponseNode response, string responseDialogueLine)
        {
            ResponseNode = response;
            PlayerAnswerText.Value = responseDialogueLine;
        }

        public StringReactiveProperty PlayerAnswerText { get; } = new StringReactiveProperty();
        public ReactiveProperty<bool> IsHovered { get; set; } = new ReactiveProperty<bool>(false);
        public PlayerResponseNode ResponseNode { get; }
        public ISubject<PlayerResponseNode> PlayerResponseSelected { get; } = new Subject<PlayerResponseNode>();
    }
}