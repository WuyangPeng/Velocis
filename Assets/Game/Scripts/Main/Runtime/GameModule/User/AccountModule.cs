using Game.Scripts.Main.Runtime.GameData.User;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.SaveData;

namespace Game.Scripts.Main.Runtime.GameModule.User
{
    [Module]
    public class AccountModule : BaseModule
    {
        private readonly AccountData accountData = new();

        public void Clear()
        {
            accountData.Clear();
        }

        public void SetTalentData(TalentSaveData talentSaveData)
        {
            accountData.SetTalentData(talentSaveData);
        }

        public bool HasTalent(int talentId)
        {
            return accountData.HasTalent(talentId);
        }
    }
}