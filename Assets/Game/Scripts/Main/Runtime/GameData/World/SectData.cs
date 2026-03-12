using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Main.Runtime.RuntimeException;

namespace Game.Scripts.Main.Runtime.GameData.World
{
    public class SectData
    {
        private readonly Dictionary<long, SectBaseData> _sectBaseDataContainer = new();
        private long _currentSectId;

        public long GetNextSectId()
        {
            return ++_currentSectId;
        }

        public SectBaseData GetSectBaseData(long id)
        {
            return _sectBaseDataContainer.TryGetValue(id, out var sectBaseData) ? sectBaseData : throw new GameException($"sect id = {id} is not exist");
        }

        public void AddSect(SectBaseData sectBaseData)
        {
            _sectBaseDataContainer.Add(sectBaseData.ID, sectBaseData);
        }

        public List<SectBaseData> GetSects()
        {
            return _sectBaseDataContainer.Select(element => element.Value).ToList();
        }
    }
}