using Characters;
using DataSystem;
using DialogueSystem;
using System.Collections.Generic;
using UI;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace NPC
{
    public class CharactersPresenter : IInitializable, ITickable
    {
        [Inject] private readonly CharactersModel _model;
        [Inject] private readonly CharactersView _view;
        [Inject] private readonly DialogueModel _dialogueModel;
        [Inject] private readonly Camera _playerCamera;

        public void Initialize()
        {
            CharacterView[] characterViews = _view.GetComponentsInChildren<CharacterView>();
            HashSet<string> uniqueIds = new HashSet<string>();

            foreach (var npcView in characterViews)
            {
                if (uniqueIds.Contains(npcView.UniqueId))
                {
                    Debug.LogError($"Duplicate UniqueId found: {npcView.UniqueId}");
                    npcView.gameObject.SetActive(false);
                    continue;
                }
                uniqueIds.Add(npcView.UniqueId);

                CharacterModel npcModel = new CharacterModel(npcView.UniqueId);

                _model.CharacterViews[npcView.UniqueId] = npcView;
                _model.CharacterModels[npcView.UniqueId] = npcModel;
                _model.CharactersRepository.LoadData();

                npcView.Model = npcModel;

                npcModel.TryStartDialogue
                    .Subscribe(data => _dialogueModel.TryStartDialogue.OnNext(data))
                    .AddTo(_view);

                _model.SetHasMet
                    .Subscribe(data => SetHasMet(data))
                    .AddTo(_view);

                _model.AddFriendship
                    .Subscribe(data => AddFriendship(data))
                    .AddTo(_view);


                foreach (CharacterView characterView in characterViews)
                {
                    CharacterUIModel characterUIModel = new CharacterUIModel();
                    characterUIModel.CharacterID.Value = $"NPC ID: {characterView.UniqueId}";
                    characterUIModel.CharacterName.Value = characterView.CharacterName;
                    characterUIModel.DialogueID.Value = $"DIALOGUE ID: {characterView.DialogueID}";
                    characterUIModel.FriendshipAmount.Value = GetFriendshipAmount(characterUIModel.CharacterID.Value);

                    characterView.OnSetUIModel(characterUIModel);
                }
            }
        }

        private void AddFriendship(FriendshipData data)
        {
            CharacterData characterData = _model.CharactersRepository.GetCharacterByID(data.CharacterID);
            characterData.FriendshipAmount += data.FriendshipAmount;
            _model.CharactersRepository.SaveData();
        }

        private void SetHasMet(HasMetData data)
        {
            CharacterData characterData = _model.CharactersRepository.GetCharacterByID(data.CharacterID);
            characterData.HasMetPlayer = data.HasMetPlayer;
            _model.CharactersRepository.SaveData();
        }

        public void Tick()
        {
            foreach (KeyValuePair<string, CharacterView> item in _model.CharacterViews)
            {
                item.Value.LookAtCamera(_playerCamera.transform.position);
            }
        }

        private string GetFriendshipAmount(string value)
        {
            CharacterData characterData = _model.CharactersRepository.GetCharacterByID(value);
            return $"FA: {characterData.FriendshipAmount}";
        }
    }
}
