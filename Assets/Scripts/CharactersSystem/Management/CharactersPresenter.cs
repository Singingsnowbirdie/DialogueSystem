using Characters;
using DialogueSystem;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace NPC
{
    public class CharactersPresenter : IInitializable
    {
        [Inject] private readonly CharactersModel _model;  
        [Inject] private readonly CharactersView _view;
        [Inject] private readonly DialogueModel _dialogueModel;

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

                npcView.Model = npcModel;

                npcModel.TryStartDialogue
                    .Subscribe(data => _dialogueModel.TryStartDialogue.OnNext(data))
                    .AddTo(_view);
            }
        }
    }
}
