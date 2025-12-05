using System.Collections.Generic;

namespace Game.Scripts.Main.Runtime.SaveData
{
    public class TalentSaveData
    {
        public HashSet<int> UnlockTalent { get; set; } = new();
    }
}