using NPC;
using UnityEngine;

namespace GM_Tools
{
    public class DebugTool_NPC : MonoBehaviour
    {
        private NPCRepository _nPCRepository;

        private void OnValidate()
        {
            _nPCRepository ??= new NPCRepository();
        }

        public void ResetData()
        {
            _nPCRepository.ResetData();
        }

        public void SetHasMetPlayer(string npcId, bool hasMet)
        {
            _nPCRepository.SetHasMetPlayer(npcId, hasMet);
        }
    }
}

