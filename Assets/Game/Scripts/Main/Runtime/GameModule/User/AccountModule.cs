using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Account;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.Login;

namespace Game.Scripts.Main.Runtime.GameModule.User
{
    [Module]
    public class AccountModule : BaseModule
    {
        private LoginServerInfo _currentLoginServerInfo;
        private List<LoginServerInfo> _loginServerInfo = new();
        private Token _token = new();


        public void SetToken(string token, long expireMilliseconds)
        {
            _token.SetToken(token, expireMilliseconds);
        }

        public void SetLoginServerInfo(List<LoginServerInfo> loginServerInfo)
        {
            _loginServerInfo = loginServerInfo;
            _currentLoginServerInfo = null;
        }

        public string GetToken()
        {
            return _token.GetToken();
        }

        public List<LoginServerInfo> GetLoginServerInfo()
        {
            return _loginServerInfo;
        }

        public void Clear()
        {
            _currentLoginServerInfo = null;
            _loginServerInfo.Clear();
            _token = new Token();
        }

        public LoginServerInfo SetCurrentLoginServerInfo(int index)
        {
            _currentLoginServerInfo = _loginServerInfo[index];

            return _currentLoginServerInfo;
        }
    }
}