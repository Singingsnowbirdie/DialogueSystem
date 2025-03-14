﻿using DialogueSystem;
using DialogueSystem.DialogueEditor;
using QuestSystem;
using System.Collections.Generic;
using System.Linq;
using UniRx;
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

    public class DialogueVariablesTool : MonoBehaviour
    {
        public ReactiveCollection<DialogueVariable> Variables { get; } = new ReactiveCollection<DialogueVariable>();

        private DialogueVariablesRepository _dialogueVariablesRepository;

        private void OnValidate()
        {
            _dialogueVariablesRepository ??= new DialogueVariablesRepository();
            LoadVariables();
        }

        public void LoadVariables()
        {
            List<DialogueVariable> variables = _dialogueVariablesRepository.LoadDialogueVariables();

            Variables.Clear();
            foreach (var item in variables)
            {
                Variables.Add(item);
            }
        }

        public void ResetData()
        {
            _dialogueVariablesRepository.ResetData();
            Debug.Log("Dialogue variables data has been reset.");
        }

        public void UpdateDialogueVariable(string variableId, bool isTrue, int amount)
        {
            DialogueVariable existingVariable = Variables.FirstOrDefault(item => item.VariableID == variableId);

            if (existingVariable != null)
            {
                existingVariable.IsTrue = isTrue;
                existingVariable.Amount = amount;
            }
            else
            {
                Variables.Add(new DialogueVariable(variableId, isTrue, amount));
            }

            _dialogueVariablesRepository.SaveDialogueVariables(Variables);
        }

    }
}

