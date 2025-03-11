using Gameplay.UI.ReactiveViews;
using UnityEngine;

namespace UI.DialogueUI
{
    public class DialogueView : UIView
    {
        [SerializeField] private TextMeshProReactiveStringView _speakerNameTF;
        [SerializeField] private TextMeshProReactiveStringView _dialogueTF;
        [SerializeField] private PlayerAnswersList _playerAnswersList;

        internal void OnDialogueUIModelUpdated(DialogueUIModel uiModel)
        {
            if (uiModel == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                _speakerNameTF.SetUIModel(uiModel.SpeakerName);
                _dialogueTF.SetUIModel(uiModel.DialogueText);
                //_playerAnswersList.SetUIModel(uiModel.PlayerAnswers);

                gameObject.SetActive(true);
            }

        }
    }
}
