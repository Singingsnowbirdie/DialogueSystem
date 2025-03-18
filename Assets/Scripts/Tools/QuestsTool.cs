using DialogueSystem.DialogueEditor;
using QuestSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GM_Tools
{
    public class QuestsTool : MonoBehaviour
    {
        private QuestsRepository _questsRepository;

        public QuestsRepository QuestsRepository
        {
            get
            {
                _questsRepository ??= new QuestsRepository();
                return _questsRepository;
            }
        }

        public void ResetData()
        {
            QuestsRepository.ResetData();
        }

        public void UpdateQuestValues(string questId, EQuestState questState)
        {
            List<QuestData> quests = QuestsRepository.LoadQuests();

            Debug.Log($"quests = {quests}");

            QuestData questData = FindOrCreate(quests, questId);

            questData.QuestState = questState;
            QuestsRepository.SaveData(quests);
            Debug.Log($"Quest {questId} state updated to {questState}");
        }

        public QuestData FindOrCreate(List<QuestData> quests, string id)
        {
            QuestData questData = quests.Find(c => c.QuestID == id);

            if (questData == null)
            {
                questData = new QuestData(id);
                quests.Add(questData);
            }

            return questData;
        }

    }
}

