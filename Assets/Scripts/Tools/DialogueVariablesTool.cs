using DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GM_Tools
{
    public class DialogueVariablesTool : MonoBehaviour
    {

        private DialogueVariablesRepository _dialogueVariablesRepository;

        public DialogueVariablesRepository DialogueVariablesRepository
        {
            get
            {
                _dialogueVariablesRepository ??= new DialogueVariablesRepository();
                return _dialogueVariablesRepository;
            }
        }

        public void ResetData()
        {
            DialogueVariablesRepository.ResetData();
        }

        public void UpdateDialogueVariable(string variableId, bool isTrue, int amount)
        {
            List<DialogueVariableData> variables = DialogueVariablesRepository.LoadDialogueVariables();

            DialogueVariableData variable = FindOrCreate(variables, variableId);

            variable.IsTrue = isTrue;
            variable.Amount = amount;

            DialogueVariablesRepository.SaveData(variables);
            Debug.Log($"Variable {variableId} updated; isTrue = {isTrue}; amount = {amount}");
        }

        private DialogueVariableData FindOrCreate(List<DialogueVariableData> variables, string variableId)
        {
            DialogueVariableData variable = variables.Find(c => c.VariableID == variableId);

            if (variable == null)
            {
                variable = new DialogueVariableData(variableId, false, 0);
                variables.Add(variable);
            }

            return variable;
        }
    }
}

