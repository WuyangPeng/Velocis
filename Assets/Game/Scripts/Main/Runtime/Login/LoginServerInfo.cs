using System;

namespace Game.Scripts.Main.Runtime.Login
{
    [Serializable]
    public class LoginServerInfo
    {
        public ConnectionInfo connection_info;
        public string game_server_id;
        public PlayerRole player_role;
        public string server_name;
        public ServerStatusType server_status;
    }
}