using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Main.Runtime.RuntimeException;

namespace Game.Scripts.Main.Runtime.GameData.World
{
    public class SectData
    {
        private long currentSectId;
        private readonly Dictionary<long, SectBaseData> sectBaseDataContainer = new();

        public long GetNextSectId()
        {
            return ++currentSectId;
        }

        public SectBaseData GetSectBaseData(long id)
        {
            return sectBaseDataContainer.TryGetValue(id, out var sectBaseData) ? sectBaseData : throw new GameException($"sect id = {id} is not exist");
        }

        public void AddSect(SectBaseData sectBaseData)
        {
            sectBaseDataContainer.Add(sectBaseData.ID, sectBaseData);
        }

        public List<SectBaseData> GetSects()
        {
            return sectBaseDataContainer.Select(element => element.Value).ToList();
        }
    }
}