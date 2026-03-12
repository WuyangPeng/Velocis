using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.World
{
    [Module]
    public class NpcModule : BaseModule
    {
        private NpcData _npcData = new();

        public long GetNextNpcId()
        {
            return _npcData.GetNextNpcId();
        }

        public void AddNpc(NpcBaseData npcBaseData)
        {
            _npcData.AddNpc(npcBaseData);
        }

        public int GetNpcCount()
        {
            return _npcData.GetNpcCount();
        }

        public NpcData GetNpcData()
        {
            return _npcData;
        }

        public void Init(NpcData data)
        {
            _npcData = data;
        }


        public List<NpcBaseData> GetNpc()
        {
            return _npcData.GetNpc();
        }

        public NpcBaseData GetNpcBaseData(long id)
        {
            return _npcData.GetNpcBaseData(id);
        }
    }
}