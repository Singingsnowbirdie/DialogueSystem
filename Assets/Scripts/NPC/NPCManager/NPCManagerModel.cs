using System.Collections.Generic;

namespace NPC
{
    public class NPCManagerModel
    {
        public Dictionary<string, NPCView> NPCViews { get; private set; } = new Dictionary<string, NPCView>();
        public Dictionary<string, NPCModel> NPCModels { get; private set; } = new Dictionary<string, NPCModel>();

        private NPCDatabase _npcDatabase;

        public NPCDatabase NpcDatabase
        {
            get
            {
                _npcDatabase ??= new NPCDatabase();
                return _npcDatabase;
            }
        }
    }
}
