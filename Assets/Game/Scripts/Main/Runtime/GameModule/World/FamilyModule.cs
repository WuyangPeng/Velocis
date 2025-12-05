using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameData.World;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.World
{
    [Module]
    public class FamilyModule : BaseModule
    {
        private FamilyData familyData = new();

        public long GetNextFamilyId()
        {
            return familyData.GetNextFamilyId();
        }

        public void AddFamily(FamilyBaseData familyBaseData)
        {
            familyData.AddFamily(familyBaseData);
        }

        public FamilyData GetFamilyData()
        {
            return familyData;
        }

        public List<FamilyBaseData> GetFamilies()
        {
            return familyData.GetFamilies();
        }


        public long GetCurrentFamilyId()
        {
            return familyData.GetNextFamilyId();
        }

        public void Init(long currentFamilyId, List<FamilyBaseData> familyBaseDataContainer)
        {
            familyData.SetCurrentFamilyId(currentFamilyId);
            foreach (var familyBaseData in familyBaseDataContainer)
            {
                familyData.AddFamily(familyBaseData);
            }
        }
    }
}