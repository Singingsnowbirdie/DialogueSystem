using NPC;
using UnityEngine;

namespace GM_Tools
{
    public class DebugTool_NPCDatabase : MonoBehaviour
    {
        private NPCDatabase _npcDatabase;

        private void OnValidate()
        {
            _npcDatabase ??= new NPCDatabase();
        }

        public void ResetData()
        {
            _npcDatabase.ResetData();
        }

        public void SetHasMetPlayer(string npcId, bool hasMet)
        {
            _npcDatabase.SetHasMetPlayer(npcId, hasMet);
        }
    }

    //public class DebugTool_Reputation: MonoBehaviour
    //{

    //}
}

