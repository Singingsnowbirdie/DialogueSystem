using DialogueSystem.DialogueEditor;
using QuestSystem;
using UnityEngine;

namespace GM_Tools
{
    public class QuestsTool : MonoBehaviour
    {
        private QuestsRepository _questsRepository;

        private void OnValidate()
        {
            _questsRepository ??= new QuestsRepository();
        }

        public void ResetData()
        {
            _questsRepository.ResetData();
        }

        public void UpdateQuestValues(string questId, EQuestState questState)
        {
            _questsRepository.SetQuestValues(questId, questState);
        }
    }
}

