using DialogueSystem;
using UnityEngine;
using VContainer;

namespace NPC
{
    public class NPCCollectionView : MonoBehaviour
    {
        [Inject] private DialogueModel _dialogueModel;

        public DialogueModel DialogueModel { get => _dialogueModel; }

        public NPCView[] GetNPC()
        {
            return GetComponentsInChildren<NPCView>();
        }
    }
}
