﻿using System.Collections.Generic;

namespace NPC
{
    public class NPCManagerModel
    {
        public Dictionary<string, NPCView> NPCViews { get; private set; } = new Dictionary<string, NPCView>();
        public Dictionary<string, NPCModel> NPCModels { get; private set; } = new Dictionary<string, NPCModel>();

        private NPCRepository _npcRepository;

        public NPCRepository NPCRepository
        {
            get
            {
                _npcRepository ??= new NPCRepository();
                return _npcRepository;
            }
        }
    }
}
