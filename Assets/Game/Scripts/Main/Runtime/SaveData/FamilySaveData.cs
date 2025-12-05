using Game.Scripts.Main.Runtime.GameData.World;
using System.Collections.Generic;

namespace Game.Scripts.Main.Runtime.SaveData
{
    public class FamilySaveData
    {
        public long CurrentFamilyId = 0;
        public List<FamilyBaseData> FamilyBaseDataContainer = new();

    }
}