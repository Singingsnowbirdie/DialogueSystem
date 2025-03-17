using Gameplay.UI.ReactiveViews;
using UnityEngine;

namespace UI
{
    public class CharacterUIView : UIView
    {
        [SerializeField] private TextMeshProReactiveStringView _characterNameTF;
        [SerializeField] private TextMeshProReactiveStringView _characterIDTF;
        [SerializeField] private TextMeshProReactiveStringView _dialogueIDTF;
        [SerializeField] private TextMeshProReactiveStringView _friendshipAmountTF;

        public override void OnSetModel(UIModel uiModel)
        {
            base.OnSetModel(uiModel);

            CharacterUIModel characterUIModel = (CharacterUIModel)uiModel;

            _characterNameTF.SetUIModel(characterUIModel.CharacterName);
            _characterIDTF.SetUIModel(characterUIModel.CharacterID);
            _dialogueIDTF.SetUIModel(characterUIModel.DialogueID);
            _friendshipAmountTF.SetUIModel(characterUIModel.FriendshipAmount);
        }

        internal void LookAtCamera(Vector3 cameraPosition)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - cameraPosition);
        }
    }
}


