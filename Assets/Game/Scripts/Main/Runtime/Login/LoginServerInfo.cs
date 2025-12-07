using System;

namespace Game.Scripts.Main.Runtime.Login
{
    [Serializable]
    public class LoginServerInfo
    {
        private ConnectionInfo connection_info;
        private string game_server_id;
        private PlayerRole player_role;
        private string server_name;
        private ServerStatusType server_status;
    }
}