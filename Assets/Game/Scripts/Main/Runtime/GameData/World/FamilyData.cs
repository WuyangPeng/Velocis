using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Main.Runtime.RuntimeException;

namespace Game.Scripts.Main.Runtime.GameData.World
{
    public class FamilyData
    {
        private readonly Dictionary<long, FamilyBaseData> _familyBaseDataContainer = new();
        private long _currentFamilyId;

        public long GetNextFamilyId()
        {
            return ++_currentFamilyId;
        }

        public FamilyBaseData GetFamilyBaseData(long id)
        {
            return _familyBaseDataContainer.TryGetValue(id, out var familyBaseData) ? familyBaseData : throw new GameException($"family id = {id} is not exist");
        }

        public void AddFamily(FamilyBaseData familyBaseData)
        {
            _familyBaseDataContainer.Add(familyBaseData.ID, familyBaseData);
        }

        public List<FamilyBaseData> GetFamilies()
        {
            return _familyBaseDataContainer.Select(element => element.Value).ToList();
        }

        public void SetCurrentFamilyId(long id)
        {
            _currentFamilyId = id;
        }
    }
}