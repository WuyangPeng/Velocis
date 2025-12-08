using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Account;
using Game.Scripts.Main.Runtime.GameData.User;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.Login;
using Game.Scripts.Main.Runtime.SaveData;

namespace Game.Scripts.Main.Runtime.GameModule.User
{
    [Module]
    public class AccountModule : BaseModule
    {
        private readonly AccountData accountData = new();
        private readonly Token token = new();
        private LoginServerInfo currentLoginServerInfo;
        private List<LoginServerInfo> loginServerInfo = new();

        public void SetToken(string token, long expireMilliseconds)
        {
            this.token.SetToken(token, expireMilliseconds);
        }

        public void SetLoginServerInfo(List<LoginServerInfo> loginServerInfo)
        {
            this.loginServerInfo = loginServerInfo;
        }

        public string GetToken()
        {
            return token.GetToken();
        }

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