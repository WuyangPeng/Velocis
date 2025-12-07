using System;
using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Game;

namespace Game.Scripts.Main.Runtime.Login
{
    [Serializable]
    public class LoginServersResponse
    {
        public string message;
        public List<LoginServerInfo> login_server_info;
        public GameErrorType code = GameErrorType.Unknown;
    }
}