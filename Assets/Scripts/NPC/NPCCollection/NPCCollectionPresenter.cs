using DialogueSystem;
using Unity.VisualScripting;
using VContainer;

namespace NPC
{
    public class NPCCollectionPresenter : IInitializable
    {
        [Inject] private NPCCollectionModel _model;
        [Inject] private NPCCollectionView _view;

        public void Initialize()
        {
            _model.NPC = _view.GetNPC();
        }
    }
}
