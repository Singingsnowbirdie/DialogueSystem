namespace DialogueSystem
{
    public class DialogueVariable
    {
        public string VariableID { get; }
        public bool IsTrue { get; set; }
        public int Amount { get; set; }

        public DialogueVariable(string dialogueVariableID, bool isTrue, int amount)
        {
            VariableID = dialogueVariableID;
            IsTrue = isTrue;
            Amount = amount;
        }
    }
}
