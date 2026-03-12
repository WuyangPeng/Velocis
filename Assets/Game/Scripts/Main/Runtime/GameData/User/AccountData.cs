using System.Collections.Generic;
using Game.Scripts.Main.Runtime.SaveData;

namespace Game.Scripts.Main.Runtime.GameData.User
{
    public class AccountData
    {
        private readonly HashSet<int> _unlockAchievements = new();
        private readonly HashSet<int> _unlockTalent = new();

        public void Clear()
        {
            _unlockTalent.Clear();
            _unlockAchievements.Clear();
        }

        public void SetTalentData(TalentSaveData talentSaveData)
        {
            _unlockTalent.UnionWith(talentSaveData.UnlockTalent);
        }

        public bool HasTalent(int talentId)
        {
            return _unlockTalent.Contains(talentId);
        }
    }
}