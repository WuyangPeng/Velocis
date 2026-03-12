using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.World
{
    [Module]
    public class FamilyModule : BaseModule
    {
        private readonly FamilyData _familyData = new();

        public long GetNextFamilyId()
        {
            return _familyData.GetNextFamilyId();
        }

        public void AddFamily(FamilyBaseData familyBaseData)
        {
            _familyData.AddFamily(familyBaseData);
        }

        public FamilyData GetFamilyData()
        {
            return _familyData;
        }

        public List<FamilyBaseData> GetFamilies()
        {
            return _familyData.GetFamilies();
        }


        public long GetCurrentFamilyId()
        {
            return _familyData.GetNextFamilyId();
        }

        public void Init(long currentFamilyId, List<FamilyBaseData> familyBaseDataContainer)
        {
            _familyData.SetCurrentFamilyId(currentFamilyId);
            foreach (var familyBaseData in familyBaseDataContainer)
            {
                _familyData.AddFamily(familyBaseData);
            }
        }
    }
}