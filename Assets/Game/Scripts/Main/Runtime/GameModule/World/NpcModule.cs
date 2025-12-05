using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.RuntimeException;
using System.Collections.Generic;

namespace Game.Scripts.Main.Runtime.GameModule.World
{
    [Module]
    public class NpcModule : BaseModule
    {
        private NpcData npcData = new();

        public long GetNextNpcId()
        {
            return npcData.GetNextNpcId();
        }

        public void AddNpc(NpcBaseData npcBaseData)
        {
            npcData.AddNpc(npcBaseData);
        }

        public int GetNpcCount()
        {
            return npcData.GetNpcCount();
        }

        public NpcData GetNpcData()
        {
            return npcData;
        }

        public void Init(NpcData data)
        {
            npcData = data;
        }


        public List<NpcBaseData> GetNpc()
        {
            return npcData.GetNpc();
        }

        public NpcBaseData GetNpcBaseData(long id)
        {
            return npcData.GetNpcBaseData(id);
        }
    }
}