using DialogueSystem;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace NPC
{
    public class NPCManagerPresenter : IInitializable
    {
        [Inject] private readonly NPCManagerModel _model;
        [Inject] private readonly NPCManagerView _view;
        [Inject] private readonly DialogueModel _dialogueModel;

        public void Initialize()
        {
            NPCView[] npcViews = _view.GetComponentsInChildren<NPCView>();
            HashSet<string> uniqueIds = new HashSet<string>();

            foreach (var npcView in npcViews)
            {
                if (uniqueIds.Contains(npcView.UniqueId))
                {
                    Debug.LogError($"Duplicate UniqueId found: {npcView.UniqueId}");
                    npcView.gameObject.SetActive(false);
                    continue;
                }
                uniqueIds.Add(npcView.UniqueId);

                NPCModel npcModel = new NPCModel(npcView.UniqueId);

                _model.NPCViews[npcView.UniqueId] = npcView;
                _model.NPCModels[npcView.UniqueId] = npcModel;

                npcView.Model = npcModel;

                npcModel.TryStartDialogue
                    .Subscribe(data => _dialogueModel.TryStartDialogue.OnNext(data))
                    .AddTo(_view);
            }
        }
    }
}
