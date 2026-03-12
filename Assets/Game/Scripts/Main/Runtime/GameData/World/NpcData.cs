using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Main.Runtime.Definition.Constant;
using Game.Scripts.Main.Runtime.RuntimeException;

namespace Game.Scripts.Main.Runtime.GameData.World
{
    public class NpcData
    {
        private readonly Dictionary<long, NpcBaseData> _npcBaseDataContainer = new();
        private long _currentNpcId = Constant.Game.PlayerId;

        public long GetNextNpcId()
        {
            return ++_currentNpcId;
        }

        public NpcBaseData GetNpcBaseData(long id)
        {
            return _npcBaseDataContainer.TryGetValue(id, out var value) ? value : throw new GameException($"npc id = {id} is not exist");
        }

        public void AddNpc(NpcBaseData npcBaseData)
        {
            _npcBaseDataContainer.Add(npcBaseData.ID, npcBaseData);
        }

        public int GetNpcCount()
        {
            return _npcBaseDataContainer.Count;
        }

        public List<NpcBaseData> GetNpc()
        {
            return _npcBaseDataContainer.Select(element => element.Value).ToList();
        }
    }
}